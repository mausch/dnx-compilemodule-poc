using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Dnx.Compilation.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace ConsoleApp1.compiler.preprocess
{
    public class MetaProgrammingTest: ICompileModule {
        public void BeforeCompile(BeforeCompileContext context) {
            const string typeName = "Record";

            //Debugger.Launch();

            var xxx = context.Compilation.SyntaxTrees
                        .Select(s => new {
                            Tree = s,
                            Root = s.GetRoot(),
                            Class = s.GetRoot().DescendantNodes()
                              .OfType<ClassDeclarationSyntax>()
                              .Where(cs => cs.Identifier.ValueText == typeName)
                              .SingleOrDefault()
                        })
                        .Where(a => a.Class != null)
                        .Single();

            var members = xxx.Class.Members
                .Select(m => m.WithoutTrivia())
                .OfType<PropertyDeclarationSyntax>()
                .Select(m => {
                    var name = m.ChildTokens()
                        .Where(t => t.Kind() == SyntaxKind.IdentifierToken)
                        .Select(t => t.Text)
                        .First();

                    var type = m.Type.ToString();
                    return new { name, type };
                })
                .ToList();

            var ctorParams = members
                .Select(m => SyntaxFactory.Parameter(SyntaxFactory.Identifier(m.name.ToLowerInvariant()))
                                .WithType(SyntaxFactory.ParseTypeName(m.type)))
                .ToArray();

            var assignments = members
                .Select(m => SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    kind: SyntaxKind.SimpleAssignmentExpression,
                                    left: SyntaxFactory.IdentifierName(m.name),
                                    right: SyntaxFactory.IdentifierName(m.name.ToLowerInvariant())
                                )))
                .ToArray();

            var ctor =
                SyntaxFactory.ConstructorDeclaration(typeName)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(ctorParams)
                    .AddBodyStatements(assignments);

            var newClass = xxx.Class.AddMembers(ctor);
            var newRoot = (CSharpSyntaxNode)xxx.Root.ReplaceNode(xxx.Class, newClass);
            var newTree = CSharpSyntaxTree.Create(newRoot);

            //Debugger.Launch();
            //context.AddError(node.Class.Members.Count().ToString());

            context.Compilation = context.Compilation.ReplaceSyntaxTree(xxx.Tree, newTree);

        }

        public void AfterCompile(AfterCompileContext context) { }
    }

    public static class BeforeCompileContextExtensions {
        public static void AddError(this BeforeCompileContext ctx, string msg) {
            var dd = new DiagnosticDescriptor(
                id: "TEST1",
                messageFormat: msg,
                title: "",
                category: "",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                description: "");

            ctx.Diagnostics.Add(Diagnostic.Create(dd, Location.None));
        }
    }
}
