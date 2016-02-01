using System;

namespace ConsoleApp1
{
    public class Record {
        public string Name { get; }
        public DateTime DateOfBirth { get; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var record = new Record(name: "Juan", dateofbirth: DateTime.Now);
            Console.WriteLine(record.Name);
        }
    }
}
