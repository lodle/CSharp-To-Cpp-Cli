using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpToCpp;

namespace CSharpTestLib
{
    public class BaseClass
    {
        public int DoMoreStuff(String strVal)
        {
            return 10;
        }

        static public void StaticOne()
        {
        }
    }

    public interface IInterfaceTest
    {
        void Abcd(String d);
    }

    [ExposeToCpp]
    public class TestClass : BaseClass
    {
        public String ClassName { get; set; }
        public int ClassCount { get; set; }

        public TestClass()
        {
        }

        public TestClass(int a, String b)
        {
        }

        public int GetIntGSDF(IInterfaceTest test)
        {
            return 0;
        }

        public void DoStuff(double val)
        {

        }

        static public int StaticTwo(String a, int b)
        {
            return 0;
        }

        static public String StaticThree()
        {
            return "";
        }

        static public BaseClass NewBaseClass()
        {
            return new BaseClass();
        }
    }
}
