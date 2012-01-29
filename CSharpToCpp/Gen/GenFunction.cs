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
        public String ReturnType;
        public String ReturnManagedType;
        public String ReturnName;

        public bool IsVoidReturn;
        public MethodInfo Info;

        public override object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {
                Name = Name,
                IsVoidReturn = IsVoidReturn,
                ReturnType = ReturnType,
                ReturnManagedType = ReturnManagedType,
                ReturnName = ReturnName,
                Parameters = Parameters
            });
        }

        public GenFunction(GenClass gc, MethodInfo info)
            : base(gc, info)
        {
            Name = info.Name;
            Info = info;

            IsVoidReturn = info.ReturnParameter.ParameterType == typeof(void);
            ReturnType = GenParameter.GetParameterNativeType(info.ReturnParameter.ParameterType);

            if (info.ReturnParameter.ParameterType.IsClass)
            {
                ReturnManagedType = info.ReturnParameter.ParameterType.FullName.Replace(".", "::");
                ReturnManagedType += "^";
            }
            else
            {
                ReturnManagedType = ReturnType;
            }

            ReturnName = GenParameter.GetParameterCallManagedName(gc.Name, "res", info.ReturnParameter.ParameterType);
        }
    }
}
