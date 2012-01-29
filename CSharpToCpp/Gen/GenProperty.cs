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
        public String SetName;
        public String ReturnManagedType;
        public String ReturnName;

        public GenProperty(GenClass gc, PropertyInfo info)
        {
            Info = info;
            Name = info.Name;
            SetName = GenParameter.GetParameterCallName(gc.Name, "Value", info.PropertyType);
            NativeType = GenParameter.GetParameterNativeType(info.PropertyType);

            if (info.PropertyType.IsClass)
            {
                ReturnManagedType = info.PropertyType.FullName.Replace(".", "::");
                ReturnManagedType += "^";
            }
            else
            {
                ReturnManagedType = NativeType;
            }

            ReturnName = GenParameter.GetParameterCallManagedName(gc.Name, "res", info.PropertyType);
        }

        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {
                Name = Name,
                NativeType = NativeType,
                SetName = SetName,
                ReturnManagedType = ReturnManagedType,
                ReturnName = ReturnName,
            });
        }
    }
}
