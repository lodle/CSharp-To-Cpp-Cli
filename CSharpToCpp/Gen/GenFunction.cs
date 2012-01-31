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

        public bool IsVoidReturn;
        public MethodInfo Info;

        public override object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {
                Name = Name,

                IsVoidReturn = IsVoidReturn,
                ReturnNativeType = ReturnNativeType,
                ReturnManagedType = ReturnManagedType,
                ReturnManagedName = ReturnManagedName,
                ReturnNativeName = ReturnNativeName,

                Parameters = Parameters
            });
        }

        public GenFunction(GenClass gc, MethodInfo info)
            : base(gc, info)
        {
            Name = info.Name;
            Info = info;

            IsVoidReturn = info.ReturnParameter.ParameterType == typeof(void);

            ReturnNativeType = GenParameter.GetParameterNativeType(info.ReturnParameter.ParameterType);
            ReturnManagedType = GenParameter.GetParameterManagedType(info.ReturnParameter.ParameterType);

            ReturnManagedName = GenParameter.GetParameterManagedCallName(gc.Name, "res", info.ReturnParameter.ParameterType);
            ReturnNativeName = GenParameter.GetParameterNativeCallName(gc.Name, "res", info.ReturnParameter.ParameterType);
        }
    }
}
