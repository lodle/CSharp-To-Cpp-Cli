#pragma once
using namespace CSharpTestLib;

#include "TestClassI.h"
#include <msclr\gcroot.h>
#include "BaseClass.h"


class TestClassCPP : public virtual TestClassI, public BaseClassCPP
{
public:
	TestClassCPP() { m_TestClass = gcnew TestClass(); }
	TestClassCPP(TestClass^ _Internal) { m_TestClass = _Internal; }

	virtual void GetClassName(char* szOutBuff, size_t nOutBuffSize);
	virtual void SetClassName(const char* _ClassName);

	virtual int GetClassCount();
	virtual void SetClassCount(int _ClassCount);

	virtual int GetInt();

	virtual void DoStuff(double _val);

	virtual int DoMoreStuff(const char* _strVal);

	virtual void ToString(char* szOutBuff, size_t nOutBuffSize);

	virtual int GetHashCode();

	TestClass^ InternalObject(){return m_TestClass;}
	virtual void Destroy(){delete this;}

private:
	msclr::gcroot<TestClass^> m_TestClass;
};
