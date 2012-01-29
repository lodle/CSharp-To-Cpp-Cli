using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using System.Reflection;

namespace CSharpToCpp.Gen
{
    public class GenClass : ILiquidizable
    {
        public Type Type;

        public String Namespace;
        public String Name;
        public String BaseName = "NativeObject";
        public List<String> Headers = new List<string>();

        public List<GenConstructor> Constructors = new List<GenConstructor>();
        public List<GenFunction> Functions = new List<GenFunction>();
        public List<GenFunction> StaticFunctions = new List<GenFunction>();
        public List<GenProperty> Properties = new List<GenProperty>();

        public List<Type> UsedTypes;

        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {
                Name = Name,
                BaseName = BaseName,
                ManagedName = Name.Replace("Proxy", ""),
                Namespace = Namespace,
                Headers = Headers,
                Constructors = Constructors,
                Functions = Functions,
                StaticFunctions = StaticFunctions,
                Properties = Properties
            });
        }

        public GenClass(Type type)
        {
            Type = type;
            Name = type.Name;



            Namespace = type.FullName.Substring(0, type.FullName.LastIndexOf('.')).Replace(".", "::");

            if (type.BaseType != null && type.BaseType != typeof(Object))
                BaseName = type.BaseType.Name;

            if (type.IsInterface)
            {
                Name += "Proxy";
                BaseName += "Proxy";
            }

            UsedTypes = GetAllUsedTypes(type);

            foreach (var usedType in UsedTypes)
            {
                if (usedType.IsInterface)
                    Headers.Add(usedType.Name + "Proxy");
                else
                    Headers.Add(usedType.Name);
            }

            foreach (var con in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
                Constructors.Add(new GenConstructor(this, con));

            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!ShouldFilterFunction(function.Name))
                    Functions.Add(new GenFunction(this, function));
            }

            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                StaticFunctions.Add(new GenFunction(this, function));

            foreach (var prop in type.GetProperties())
                Properties.Add(new GenProperty(this, prop));
        }

        public static bool ShouldFilterFunction(String name)
        {
            if (name.StartsWith("get_") || name.StartsWith("set_"))
                return true;

            if (name == "ToString")
                return true;

            if (name == "GetHashCode")
                return true;

            if (name == "Equals")
                return true;

            if (name == "GetType")
                return true;

            return false;
        }

        public static List<Type> GetAllUsedTypes(Type type)
        {
            List<Type> types = new List<Type>();

            if (type.BaseType != null && type.BaseType != typeof(Object))
                types.Add(type.BaseType);

            foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (var param in constructor.GetParameters())
                {
                    if ((param.ParameterType.IsClass || param.ParameterType.IsInterface) && param.ParameterType != typeof(String))
                        types.Add(param.ParameterType);
                }
            }

            foreach (var function in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (ShouldFilterFunction(function.Name))
                    continue;

                foreach (var param in function.GetParameters())
                {
                    if ((param.ParameterType.IsClass || param.ParameterType.IsInterface) && param.ParameterType != typeof(String))
                        types.Add(param.ParameterType);
                }

                if ((function.ReturnParameter.ParameterType.IsClass || function.ReturnParameter.ParameterType.IsInterface) && function.ReturnParameter.ParameterType != typeof(String))
                    types.Add(function.ReturnParameter.ParameterType);
            }

            types = types.Distinct().ToList();
            return types;
        }
    }

}
