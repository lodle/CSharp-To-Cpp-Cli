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
            WrapperGenerator gen = new WrapperGenerator { OutPath = "wrapper", DynamicLib = "CSharpTestLib.dll" };

            foreach (var type in gen.GetTypesWith<ExposeToCppAttribute>())
            {
                gen.Generate(type);
            }
        }
    }
}
