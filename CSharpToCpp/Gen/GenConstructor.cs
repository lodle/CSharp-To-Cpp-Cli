using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using System.Reflection;

namespace CSharpToCpp.Gen
{
    public class GenConstructor : ILiquidizable
    {
        public List<GenParameter> Parameters = new List<GenParameter>();
        public ConstructorInfo Info;

        public virtual object ToLiquid()
        {
            return Hash.FromAnonymousObject(new
            {

                Parameters = Parameters
            });
        }

        public GenConstructor(GenClass gc, MethodBase info)
        {
            foreach (var param in info.GetParameters())
                Parameters.Add(new GenParameter(gc, param));
        }

        public GenConstructor(GenClass gc, ConstructorInfo info)
            : this(gc, info as MethodBase)
        {
            Info = info;
        }
    }
}
