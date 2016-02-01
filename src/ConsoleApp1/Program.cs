using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Record {
        public string Name { get; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var record = new Record(name: "Juan"); // this should not compile
            Console.WriteLine(record.Name);
            //var record = new Record(name: "John Doe"); // this should compile
        }
    }
}
