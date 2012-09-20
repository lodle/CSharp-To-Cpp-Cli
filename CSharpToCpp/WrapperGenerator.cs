using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using DotLiquid;
using CSharpToCpp.Gen;

namespace CSharpToCpp
{
    public class WrapperGenerator
    {
        String OutPath { get; set; }
        String DynamicLib { get; set; }

        Dictionary<Type, GenClass> _GeneratedList = new Dictionary<Type, GenClass>();
        Dictionary<String, Template> _TemplateList = new Dictionary<string, Template>();

		List<String> _BuildFiles = new List<String>();

        public WrapperGenerator(string outPath, string lib)
        {
            OutPath = outPath;
            DynamicLib = lib;

            Directory.CreateDirectory(outPath + "\\include");
            Directory.CreateDirectory(outPath + "\\code");

            Template.RegisterTag<ParametersTag>("parameters");
            Template.RegisterTag<ParametersCallTag>("parametersCall");
            Template.RegisterTag<FunctionCallTag>("functionCall");
            Template.RegisterTag<FunctionReturnTag>("functionReturn");
            Template.RegisterTag<ParametersManagedTag>("parametersManaged");


            GenerateTemplate(null, "NativeObjectInterface.txt", OutPath + "\\include\\NativeObjectI.h");
            GenerateTemplate(null, "NativeObjectHeader.txt", OutPath + "\\code\\NativeObject.h");
            GenerateTemplate(null, "NativeObjectBody.txt", OutPath + "\\code\\NativeObject.cpp");

            GenerateTemplate(null, "NativeObjectProxyInterface.txt", OutPath + "\\include\\NativeObjectProxyI.h");
            GenerateTemplate(null, "NativeObjectProxyHeader.txt", OutPath + "\\code\\NativeObjectProxy.h");
        }

        public void Generate(Type type)
        {
            if (!type.IsClass && !type.IsInterface)
                return;

            if (_GeneratedList.ContainsKey(type))
                return;

            GenClass gc = new GenClass(type);
            _GeneratedList.Add(type, gc);

            if (type.IsInterface)
            {
                GenerateTemplate(gc, "ProxyInterface.txt", OutPath + "\\include\\" + type.Name + "ProxyI.h");
                GenerateTemplate(gc, "ProxyHeader.txt", OutPath + "\\code\\" + type.Name + "Proxy.h");
                GenerateTemplate(gc, "ProxyBody.txt", OutPath + "\\code\\" + type.Name + "Proxy.cpp");
            }
            else
            {
                GenerateTemplate(gc, "WrapperInterface.txt", OutPath+"\\include\\" + type.Name + "I.h");
                GenerateTemplate(gc, "WrapperHeader.txt", OutPath + "\\code\\" + type.Name + ".h");
                GenerateTemplate(gc, "WrapperBody.txt", OutPath + "\\code\\" + type.Name + ".cpp");
            }

            foreach (var usedType in gc.UsedTypes)
                Generate(usedType);
        }

        private void GenerateTemplate(GenClass gc, string templateName, string outPath)
        {
            if (!_TemplateList.ContainsKey(templateName))
            {
                String tempContent = File.ReadAllText("Templates\\" + templateName);
                _TemplateList.Add(templateName, Template.Parse(tempContent));
            }

            String output = _TemplateList[templateName].Render(Hash.FromAnonymousObject(new { c = gc }));

            ////tfs issues
            if (File.Exists(outPath))
            {
                File.SetAttributes(outPath, File.GetAttributes(outPath) & ~FileAttributes.ReadOnly);
                File.Delete(outPath);
            }

            using (StreamWriter outFile = new StreamWriter(outPath))
            {
                outFile.Write(output);
            }

			_BuildFiles.Add(outPath);
        }

        public IEnumerable<Type> GetTypesWith<TAttribute>() where TAttribute : System.Attribute
        {
            return from t in Assembly.LoadFrom(DynamicLib).GetTypes()
                   where t.IsDefined(typeof(TAttribute), false)
                   select t;
        }

		public void GenerateCombinedFile()
		{
			var outPath = OutPath + "\\GeneratedBuild.cpp";
			var lines = new List<String>();

			foreach (var output in _BuildFiles)
				lines.Add("#include \"" + Path.GetFileName(Path.GetDirectoryName(output)) + "\\" + Path.GetFileName(output) + "\"\n");

			if (File.Exists(outPath))
			{
				File.SetAttributes(outPath, File.GetAttributes(outPath) & ~FileAttributes.ReadOnly);
				File.Delete(outPath);
			}

			File.WriteAllLines(outPath, lines.ToArray());
		}
	}
}
