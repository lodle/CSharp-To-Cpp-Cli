using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DotLiquid;

namespace CSharpToCpp.Gen
{
    public class GenFunction : GenConstructor
    {
        public String Name;
        public String ReturnNativeType;
        public String ReturnManagedType;

        public String ReturnManagedName;
        public String ReturnNativeName;

        public String ReturnManagedDefault;
        public String ReturnNativeDefault;

        public String NameUnique;

        public bool IsVoidReturn;
        public bool IsStringReturn;
        public MethodInfo Info;

        public override object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {
                Name = Name,

                IsVoidReturn = IsVoidReturn,
                IsStringReturn = IsStringReturn,
                ReturnNativeType = ReturnNativeType,
                ReturnManagedType = ReturnManagedType,
                ReturnManagedName = ReturnManagedName,
                ReturnNativeName = ReturnNativeName,

                ReturnManagedDefault = ReturnManagedDefault,
                ReturnNativeDefault = ReturnNativeDefault,

                NameUnique = NameUnique,
                Parameters = Parameters
            });
        }

        public GenFunction(GenClass gc, MethodInfo info)
            : base(gc, info)
        {
            Name = info.Name;
            Info = info;

            NameUnique = Name;

            int nCount = 0;
            foreach (var fun in gc.Functions)
            {
                if (fun.Name == Name)
                    nCount++;
            }

            if (nCount > 0)
                NameUnique += nCount.ToString();

            IsVoidReturn = info.ReturnParameter.ParameterType == typeof(void);
            IsStringReturn = info.ReturnParameter.ParameterType == typeof(String);

            ReturnNativeType = GenParameter.GetParameterNativeType(info.ReturnParameter.ParameterType);
            ReturnManagedType = GenParameter.GetParameterManagedType(info.ReturnParameter.ParameterType);

            ReturnManagedName = GenParameter.GetParameterManagedCallName("res", info.ReturnParameter.ParameterType);
            ReturnNativeName = GenParameter.GetParameterNativeCallName(gc.Name, "res", info.ReturnParameter.ParameterType);

            ReturnManagedDefault = GetManagedDefault(info.ReturnParameter.ParameterType);
            ReturnNativeDefault = GetNativeDefault(info.ReturnParameter.ParameterType);
        }

        static public String GetNativeDefault(Type parameterType)
        {
            if (parameterType == typeof(String))
            {
                return "std::string()";
            }
            else if (parameterType == typeof(Int32))
            {
                return "0";
            }
            else if (parameterType == typeof(Double))
            {
                return "0";
            }
            else if (parameterType == typeof(Boolean))
            {
                return "false";
            }
            else if (parameterType == typeof(Object))
            {
                return "NULL";
            }
            else if (parameterType.ContainsGenericParameters)
            {
                int a = 1;
            }
            else if (parameterType.IsClass)
            {
                return "NULL";
            }
            else if (parameterType.IsInterface)
            {
                return "NULL";
            }
            else if (parameterType == typeof(void))
            {
                return "";
            }

            return "[FAIL]";
        }

        static public String GetManagedDefault(Type parameterType)
        {
            if (parameterType == typeof(String))
            {
                return "\"\"";
            }
            else if (parameterType == typeof(Int32))
            {
                return "0";
            }
            else if (parameterType == typeof(Double))
            {
                return "0";
            }
            else if (parameterType == typeof(Boolean))
            {
                return "false";
            }
            else if (parameterType == typeof(Object))
            {
                return "null";
            }
            else if (parameterType.ContainsGenericParameters)
            {
                int a = 1;
            }
            else if (parameterType.IsClass || parameterType.IsInterface)
            {
                return "null";
            }
            else if (parameterType == typeof(void))
            {
                return "";
            }

            return "[FAIL]";
        }
    }
}
