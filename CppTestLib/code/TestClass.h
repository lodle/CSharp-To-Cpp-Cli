#pragma once

using namespace CSharpTestLib;

#include "TestClassI.h"
#include "BaseClass.h"
#include "IInterfaceTest.h"

#include <msclr\gcroot.h>


class TestClassCPP : public virtual TestClassI, public BaseClassCPP
{
public:
	//! Constructors
	TestClassCPP() : m_TestClass(gcnew TestClass()), BaseClassCPP(m_TestClass) 
	{
	}

	TestClassCPP(TestClass^ _Internal) : m_TestClass(_Internal), BaseClassCPP(m_TestClass) 
	{
	}	

	//! Properties
	virtual std::string GetClassName();
	virtual void SetClassName(std::string _Value);
	virtual int GetClassCount();
	virtual void SetClassCount(int _Value);

	//! Methods
	virtual int GetIntGSDF(IInterfaceTestI* _test);
	virtual void DoStuff(double _val);
	virtual int DoMoreStuff(std::string _strVal);

	TestClass^ InternalObject()
	{
		return m_TestClass;
	}

	virtual void Destroy()
	{
		delete this;
	}

private:
	msclr::gcroot<TestClass^> m_TestClass;
};
