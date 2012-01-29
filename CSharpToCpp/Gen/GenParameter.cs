using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DotLiquid;

namespace CSharpToCpp.Gen
{
    public class GenParameter : ILiquidizable
    {
        public String NativeType;
        public String Name;
        public String CallName;

        public Type Type;
        public ParameterInfo Info;

        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {
                Name = Name,
                CallName = CallName,
                NativeType = NativeType
            });
        }

        public GenParameter(GenClass gc, ParameterInfo info)
        {
            Info = info;
            Type = info.ParameterType;

            NativeType = GetParameterNativeType(Type);
            Name = info.Name;
            CallName = GetParameterCallName(gc.Name, info.Name, info.ParameterType);
        }

        static public string GetParameterCallName(String className, String paramName, Type paramType)
        {
            if (paramType == typeof(String))
                return String.Format("gcnew System::String(_{0})", paramName);
            else if (paramType.IsClass)
                return String.Format("(({0}*)_{1})->InternalObject()", paramName, className);
            else if (paramType.IsInterface)
                return String.Format("gcnew {0}Proxy(_{1})", paramType.Name, paramName);

            return String.Format("_{0}", paramName);
        }

        static public String GetParameterNativeType(Type parameterType)
        {
            if (parameterType == typeof(String))
            {
                return "std::string";
            }
            else if (parameterType == typeof(Int32))
            {
                return "int";
            }
            else if (parameterType == typeof(Double))
            {
                return "double";
            }
            else if (parameterType == typeof(Boolean))
            {
                return "bool";
            }
            else if (parameterType == typeof(Object))
            {
                return "NativeObjectI*";
            }
            else if (parameterType.ContainsGenericParameters)
            {
                int a = 1;
            }
            else if (parameterType.IsClass | parameterType.IsInterface)
            {
                return parameterType.Name + "I*";
            }
            else if (parameterType == typeof(void))
            {
                return "void";
            }

            return "[FAIL]";
        }
    }

}
