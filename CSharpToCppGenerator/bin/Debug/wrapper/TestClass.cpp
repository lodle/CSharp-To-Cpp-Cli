#define DLL_EXPORT
#include "TestClass.h"

using namespace CSharpTestLib;

class TestClassCPP : public TestClassI
{
public:
	TestClassCPP(TestClass^ _Internal) { m_TestClass = _Internal; }

	virtual void GetClassName(char* szOutBuff, size_t nOutBuffSize)
	{
		System::String^ ret = m_TestClass->ClassName;
		std::string szRet = marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
	}

	virtual void SetClassName(const char* _ClassName)
	{
		m_TestClass->ClassName = gcnew System::String(_ClassName);
	}

	virtual int GetClassCount()
	{
		return m_TestClass->ClassCount;
	}

	virtual void SetClassCount(int _ClassCount)
	{
		m_TestClass->ClassCount = _ClassCount;
	}

	virtual int GetInt()
	{
		return m_TestClass->GetInt();
	}

	virtual void DoStuff(double _val)
	{
		return m_TestClass->DoStuff(_val);
	}

	virtual int DoMoreStuff(const char* _strVal)
	{
		return m_TestClass->DoMoreStuff(gcnew System::String(_strVal));
	}

	virtual void ToString(char* szOutBuff, size_t nOutBuffSize)
	{
		System::String^ ret = m_TestClass->ToString();
		std::string szRet = marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
	}

	virtual int GetHashCode()
	{
		return m_TestClass->GetHashCode();
	}

	TestClass^ InternalObject(){return m_TestClass;}
	virtual void Destroy(){delete this;}

private:
	gcroot<TestClass^> m_TestClass;
};

extern "C"
{
	DLLFN TestClassI* NewTestClass()
	{
		return new TestClassCPP();
	}

	DLLFN TestClassI* NewTestClass(int _a, const char* _b)
	{
		return new TestClassCPP(_a, gcnew System::String(_b));
	}


	DLLFN int TestClass_StaticTwo(const char* _a, int _b)
	{
		return TestClass::StaticTwo(gcnew System::String(_a), _b);
	}

	DLLFN void TestClass_StaticThree(char* szOutBuff, size_t nOutBuffSize)
	{
		System::String^ ret = TestClass::StaticThree();
		std::string szRet = marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
	}

	DLLFN BaseClassI* TestClass_NewBaseClass()
	{
		BaseClass^ ret = TestClass::NewBaseClass();
		return NewBaseClassCPP(ret);
	}

	TestClassI* NewTestClassCPP(TestClass^ _Internal)
	{
		return new TestClassCPP(_Internal);
	}
}
