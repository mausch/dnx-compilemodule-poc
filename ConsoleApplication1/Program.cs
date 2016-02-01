using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    class Program {
        static void Main(string[] args) {
            ClassDeclarationSyntax clazz = null;

            //clazz.AddMembers

            var member = clazz.Members
                .Select(m => m.WithoutTrivia())
                .First()
                //.Where(m => m.)
                ;

            //clazz.AddMembers

            var ctor =
                SyntaxFactory.ConstructorDeclaration("Record")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(SyntaxFactory.Parameter(SyntaxFactory.Identifier(""))
                                                    .WithType(SyntaxFactory.ParseTypeName("string")))
                    .AddBodyStatements(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                kind: SyntaxKind.SimpleAssignmentExpression,
                                left: SyntaxFactory.IdentifierName("Name"),
                                //right: SyntaxFactory.Argument
                                right: SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("John Doe"))
                            )
                        )
                    );

            ctor.AddBodyStatements(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        kind: SyntaxKind.SimpleAssignmentExpression,
                        left: SyntaxFactory.IdentifierName("Name"),
                        right: SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("John Doe"))
                    )
                )
            );

            CSharpCompilation x = null;

            x.ReplaceSyntaxTree(null, null);

            SyntaxTree tree = null;

            var root = tree.GetRoot();

            root.ReplaceNode(root, clazz);
        }
    }
}
