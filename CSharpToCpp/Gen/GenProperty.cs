using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using System.Reflection;

namespace CSharpToCpp.Gen
{
    public class GenProperty : ILiquidizable
    {
        public PropertyInfo Info;
        public String Name;
        public String NativeType;

        public String SetNativeName;
        public String SetManagedName;

        public String ReturnManagedType;
        public String ReturnNativeName;
        public String ReturnManagedName;

        public bool IsString;

        public GenProperty(GenClass gc, PropertyInfo info)
        {
            Info = info;
            Name = info.Name;

            SetNativeName = GenParameter.GetParameterNativeCallName(gc.Name, "Value", info.PropertyType);
            SetManagedName = GenParameter.GetParameterManagedCallName("Value", info.PropertyType);

            NativeType = GenParameter.GetParameterNativeType(info.PropertyType);
            IsString = (info.PropertyType == typeof(String));

            if (info.PropertyType.IsClass)
            {
                ReturnManagedType = info.PropertyType.FullName.Replace(".", "::");
                ReturnManagedType += "^";
            }
            else
            {
                ReturnManagedType = NativeType;
            }

            ReturnNativeName = GenParameter.GetParameterNativeCallName(gc.Name, "res", info.PropertyType);
            ReturnManagedName = GenParameter.GetParameterManagedCallName("res", info.PropertyType);
        }

        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {
                Name = Name,
                NativeType = NativeType,

                SetNativeName = SetNativeName,
                SetManagedName = SetManagedName,

                ReturnManagedType = ReturnManagedType,
                ReturnManagedName = ReturnManagedName,
                ReturnNativeName = ReturnNativeName,

                IsString = IsString,
            });
        }
    }
}
