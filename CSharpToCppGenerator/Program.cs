using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using CSharpTestLib;
using CSharpToCpp;

namespace CSharpToCppGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 2)
            {
                Console.WriteLine("C# to C++ Generator. Copyright Mark Chandler 2012");
                Console.WriteLine("Usage:");
                Console.WriteLine("\tCSharpToCppGenerator.exe [Assembly] [OutDir]");
                return;
            }

            WrapperGenerator gen = new WrapperGenerator(args[1], args[0]);

            foreach (var type in gen.GetTypesWith<ExposeToCppAttribute>())
            {
                gen.Generate(type);
            }

			gen.GenerateCombinedFile();
        }
    }
}
