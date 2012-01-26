using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpToCpp
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExposeToCppAttribute : Attribute
    {
        public ExposeToCppAttribute()
        {
        }
    }
}
