using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace CSharpToCpp
{
    public class WrapperGenerator
    {
        public String OutPath { get; set; }
        public string DynamicLib { get; set; }

        List<Type> _GeneratedList = new List<Type>();

        public void Generate(Type type)
        {
            if (!type.IsClass)
                return;

            if (_GeneratedList.Contains(type))
                return;

            _GeneratedList.Add(type);

            Directory.CreateDirectory(OutPath);
            String destPath = OutPath + "\\" + type.Name + ".h";


            using (TextWriter header = new StreamWriter(destPath))
            {
                GenerateHeader(type, header);
            }

            destPath = OutPath + "\\" + type.Name + ".cpp";

            using (TextWriter body = new StreamWriter(destPath))
            {
                GenerateBody(type, body);
            }
        }

        private void GenerateHeader(Type type, TextWriter header)
        {
            header.WriteLine("#pragma once");

            List<Type> usedTypes = GetAllUsedTypes(type);

            foreach (var usedType in usedTypes)
            {
                header.WriteLine(String.Format("#include \"{0}.h\"", usedType.Name));
                Generate(type.BaseType);
            }

            header.WriteLine();
            header.WriteLine("#ifndef DLLFN\n\t#ifndef DLL_EXPORT\n\t\t#define DLLFN __declspec( dllimport )\n\t#else\n\t\t#define DLLFN __declspec( dllexport )\n\t#endif\n#endif");

            header.Write("\n\n");
            header.Write(String.Format("class {0}I", type.Name));

            if (type.BaseType != typeof(Object))
                header.WriteLine(String.Format(" : public {0}I", type.BaseType.Name));
            else
                header.WriteLine();

            header.WriteLine("{");
            header.WriteLine("public:");

            foreach (var property in type.GetProperties())
            {
                PrintPropertyDef(header, property, "\tvirtual ", "=0;", "Get");
                PrintPropertyDef(header, property, "\tvirtual ", "=0;", "Set");
                header.WriteLine("");
            }

            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (function.Name.StartsWith("get_") || function.Name.StartsWith("set_"))
                    continue;

                if (function.Name == "GetType" || function.Name == "Equals")
                    continue;

                header.Write("\tvirtual ");
                PrintFunctionDef(header, function);
                header.WriteLine("=0;\n");
            }

            header.WriteLine("};");

            header.WriteLine("");
            header.WriteLine("extern \"C\"");
            header.WriteLine("{");

            foreach (var function in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                header.Write(String.Format("\tDLLFN {0}I* New{0}(", type.Name));

                bool first = true;
                foreach (var paramater in function.GetParameters())
                {
                    if (first == false)
                        header.Write(", ");

                    PrintParamter(header, paramater);
                    first = false;
                }

                header.WriteLine(");");
            }

            header.WriteLine("");
            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                header.Write("\tDLLFN ");
                PrintFunctionDef(header, function, type.Name + "_");
                header.WriteLine(";");
            }

            header.WriteLine();
            header.WriteLine("#ifdef DLL_EXPORT");
            header.WriteLine("\t{0}I* New{0}CPP({1}^ _Internal);", type.Name, type.FullName.Replace(".", "::"));
            header.WriteLine("#endif");

            header.WriteLine("}");
        }

        private List<Type> GetAllUsedTypes(Type type)
        {
            List<Type> types = new List<Type>();

            if (type.BaseType != typeof(Object))
                types.Add(type.BaseType);

            foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (var param in constructor.GetParameters())
                {
                    if (param.ParameterType.IsClass && param.ParameterType != typeof(String))
                        types.Add(param.ParameterType);
                }
            }

            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (function.Name.StartsWith("get_") || function.Name.StartsWith("set_"))
                    continue;

                if (function.Name == "GetType" || function.Name == "Equals")
                    continue;

                foreach (var param in function.GetParameters())
                {
                    if (param.ParameterType.IsClass && param.ParameterType != typeof(String))
                        types.Add(param.ParameterType);
                }

                if (function.ReturnParameter.ParameterType.IsClass && function.ReturnParameter.ParameterType != typeof(String))
                    types.Add(function.ReturnParameter.ParameterType);
            }

            types = types.Distinct().ToList();
            return types;
        }

        private void PrintPropertyDef(TextWriter header, PropertyInfo property, string preFix, string postFix, string type)
        {
            if (type == "Get")
            {
                header.Write(String.Format("{0}", preFix));

                if (property.PropertyType == typeof(String))
                {
                    header.WriteLine(String.Format("void Get{0}(char* szOutBuff, size_t nOutBuffSize){1}", property.Name, postFix));
                }
                else
                {
                    PrintParameterType(header, property.PropertyType);
                    header.WriteLine(String.Format(" Get{0}(){1}", property.Name, postFix));
                }
            }
            else
            {
                header.Write(String.Format("{0}void Set{1}(", preFix, property.Name));
                PrintParameterType(header, property.PropertyType);
                header.WriteLine(String.Format(" _{0}){1}", property.Name, postFix));
            }
        }

        private void PrintFunctionDef(TextWriter header, MethodInfo function, String namePreFix = "")
        {
            bool first = true;

            if (function.ReturnParameter.ParameterType == typeof(String))
            {
                first = false;
                header.Write(String.Format("void {0}(char* szOutBuff, size_t nOutBuffSize", namePreFix + function.Name));
            }
            else
            {
                PrintParameterType(header, function.ReturnParameter.ParameterType);
                header.Write(String.Format(" {0}(", namePreFix + function.Name));
            }

            foreach (var paramater in function.GetParameters())
            {
                if (first == false)
                    header.Write(", ");

                PrintParamter(header, paramater);
                first = false;
            }

            header.Write(")");
        }

        private void PrintParameterType(TextWriter header, Type parameterType)
        {
            if (parameterType == typeof(String))
            {
                header.Write("const char*");
            }
            else if (parameterType == typeof(Int32))
            {
                header.Write("int");
            }
            else if (parameterType == typeof(Double))
            {
                header.Write("double");
            }
            else if (parameterType == typeof(Boolean))
            {
                header.Write("bool");
            }
            else if (parameterType == typeof(Object))
            {
                header.Write("void*");
            }
            else if (parameterType.IsClass)
            {
                header.Write(parameterType.Name + "I*");
                Generate(parameterType);
            }
            else if (parameterType == typeof(void))
            {
                header.Write("void");
            }
            else
            {
                int a = 1;
            }
        }

        private void PrintParamter(TextWriter header, ParameterInfo paramater)
        {
            PrintParameterType(header, paramater.ParameterType);
            header.Write(String.Format(" _{0}", paramater.Name));
        }

        private void GenerateBody(Type type, TextWriter body)
        {
            body.WriteLine("#define DLL_EXPORT");
            body.WriteLine("#include \"{0}.h\"", type.Name);
            body.WriteLine();

            String NameSpace = type.FullName.Substring(0, type.FullName.LastIndexOf('.'));
            NameSpace.Replace(".", "::");

            body.WriteLine("using namespace {0};", NameSpace);
            body.WriteLine();

            body.WriteLine("class {0}CPP : public {0}I", type.Name);
            body.WriteLine("{");
            body.WriteLine("public:");

            body.WriteLine("\t{0}CPP() {{ m_{0} = gcnew {0}(); }}", type.Name);
            body.WriteLine("\t{0}CPP({0}^ _Internal) {{ m_{0} = _Internal; }}", type.Name);
            body.WriteLine();

            foreach (var property in type.GetProperties())
            {
                PrintPropertyDef(body, property, "\tvirtual ", "", "Get");
                body.WriteLine("\t{");

                PrintFunctionReturnSetup(body, property.PropertyType);
                body.WriteLine("m_{0}->{1};", type.Name, property.Name);
                PrintFunctionReturnFinish(body, property.PropertyType);

                body.WriteLine("\t}");
                body.WriteLine("");

                PrintPropertyDef(body, property, "\tvirtual ", "", "Set");
                body.WriteLine("\t{");

                body.Write("\t\tm_{0}->{1} = ", type.Name, property.Name);
                PrintFunctionCallParameter(body, property.Name, property.PropertyType);
                body.WriteLine(";");

                body.WriteLine("\t}");
                body.WriteLine("");
            }

            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (function.Name.StartsWith("get_") || function.Name.StartsWith("set_"))
                    continue;

                if (function.Name == "GetType" || function.Name == "Equals")
                    continue;

                body.Write("\tvirtual ");
                PrintFunctionDef(body, function);
                body.WriteLine("\n\t{");

                PrintFunctionReturnSetup(body, function.ReturnType);
                body.Write(String.Format("m_{0}->{1}(", type.Name, function.Name));

                PrintFunctionCall(body, function);
                body.WriteLine(");");

                PrintFunctionReturnFinish(body, function.ReturnType);

                body.WriteLine("\t}");
                body.WriteLine("");
            }

            body.WriteLine("\t{0}^ InternalObject(){{return m_{0};}}", type.Name);
            body.WriteLine("\tvirtual void Destroy(){delete this;}");
            body.WriteLine();
            body.WriteLine("private:");
            body.WriteLine("\tgcroot<{0}^> m_{0};", type.Name);
            body.WriteLine("};");

            body.WriteLine("");
            body.WriteLine("extern \"C\"");
            body.WriteLine("{");

            foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                body.Write(String.Format("\tDLLFN {0}I* New{0}(", type.Name));

                bool first = true;
                foreach (var paramater in constructor.GetParameters())
                {
                    if (first == false)
                        body.Write(", ");

                    PrintParamter(body, paramater);
                    first = false;
                }

                body.WriteLine(")");
                body.WriteLine("\t{");

                body.Write(String.Format("\t\treturn new {0}CPP(", type.Name));
                PrintConstructorCall(body, constructor);
                body.WriteLine(");");

                body.WriteLine("\t}");
                body.WriteLine("");
            }

            body.WriteLine("");
            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                body.Write("\tDLLFN ");
                PrintFunctionDef(body, function, type.Name + "_");
                body.WriteLine();

                body.WriteLine("\t{");

                PrintFunctionReturnSetup(body, function.ReturnType);
                body.Write(String.Format("{0}::{1}(", type.Name, function.Name));

                PrintFunctionCall(body, function);
                body.WriteLine(");");

                PrintFunctionReturnFinish(body, function.ReturnType);
                body.WriteLine("\t}");
                body.WriteLine();
            }

            body.WriteLine("\t{0}I* New{0}CPP({0}^ _Internal)", type.Name);
            body.WriteLine("\t{");
            body.WriteLine("\t\treturn new {0}CPP(_Internal);", type.Name);
            body.WriteLine("\t}");

            body.WriteLine("}");
        }

        private void PrintFunctionReturnFinish(TextWriter body, Type returnType)
        {
            if (returnType == typeof(String))
            {
                body.WriteLine("\t\tstd::string szRet = marshal_as<std::string>(ret);");
                body.WriteLine("\t\tstrncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());");
            }
            else if (returnType.IsClass)
            {
                body.WriteLine("\t\treturn New{0}CPP(ret);", returnType.Name);
            }
        }

        private void PrintFunctionReturnSetup(TextWriter body, Type returnType)
        {
            body.Write("\t\t");

            if (returnType == typeof(object))
                return;

            if (returnType == typeof(String))
                body.Write("System::String^ ret = ");
            else if (returnType.IsClass)
                body.Write("{0}^ ret = ", returnType.Name);
            else
                body.Write("return ");
        }

        private void PrintConstructorCall(TextWriter body, ConstructorInfo constructor)
        {
            PrintFunctionCallParameters(body, constructor.GetParameters());
        }

        private void PrintFunctionCall(TextWriter body, MethodInfo function)
        {
            int nSkip = 0;

            if (function.ReturnType == typeof(String))
                nSkip = 2;

            PrintFunctionCallParameters(body, function.GetParameters(), nSkip);
        }

        private void PrintFunctionCallParameters(TextWriter body, ParameterInfo[] parameterInfo, int nSkip = 0)
        {
            bool bFirst = true;
            foreach (var parameter in parameterInfo)
            {
                if (nSkip > 0)
                {
                    nSkip--;
                    continue;
                }

                if (!bFirst)
                    body.Write(", ");

                PrintFunctionCallParameter(body, parameter.Name, parameter.ParameterType);

                bFirst = false;
            }
        }

        private static void PrintFunctionCallParameter(TextWriter body, String name, Type type)
        {
            if (type == typeof(String))
                body.Write(String.Format("gcnew System::String(_{0})", name));
            else if (type.IsClass)
                body.Write(String.Format("(({0}*)_{1})->InternalObject()", type.Name, name));
            else
                body.Write(String.Format("_{0}", name));
        }

        private bool HasFunctionReturn(MethodInfo function)
        {
            return (function.ReturnType != typeof(void) && function.ReturnType != typeof(String));
        }

        public IEnumerable<Type> GetTypesWith<TAttribute>() where TAttribute : System.Attribute
        {
            return from t in Assembly.LoadFrom(DynamicLib).GetTypes()
                   where t.IsDefined(typeof(TAttribute), false)
                   select t;
        }   
    }
}
