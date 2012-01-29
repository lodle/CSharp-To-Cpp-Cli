#define CPP_CLI_DLL

#include "TestClass.h"

#include <xstring>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>



////////////////////// Constructors //////////////////////
TestClassI* TestClassI::NewTestClass()
{
	return new TestClassCPP(gcnew TestClass());
}

TestClassI* TestClassI::NewTestClass(int _a, std::string _b)
{
	return new TestClassCPP(gcnew TestClass(_a, gcnew System::String(_b.c_str())));
}


////////////////////// Static Functions //////////////////////
int TestClassI::StaticTwo(std::string _a, int _b)
{
	try
	{
		int _res = TestClass::StaticTwo(gcnew System::String(_a.c_str()), _b);
		return _res;
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

std::string TestClassI::StaticThree()
{
	try
	{
		System::String^ _res = TestClass::StaticThree();
		return msclr::interop::marshal_as<std::string>(_res);
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

BaseClassI* TestClassI::NewBaseClass()
{
	try
	{
		CSharpTestLib::BaseClass^ _res = TestClass::NewBaseClass();
		return new BaseClassCPP(_res);
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}





////////////////////// Propeties //////////////////////
std::string TestClassCPP::GetClassName()
{
	System::String^ _res = m_TestClass->ClassName;
	return msclr::interop::marshal_as<std::string>(_res);
}

void TestClassCPP::SetClassName(std::string _Value)
{
	m_TestClass->ClassName = gcnew System::String(_Value.c_str());
}

int TestClassCPP::GetClassCount()
{
	int _res = m_TestClass->ClassCount;
	return _res;
}

void TestClassCPP::SetClassCount(int _Value)
{
	m_TestClass->ClassCount = _Value;
}


////////////////////// Functions //////////////////////
int TestClassCPP::GetIntGSDF(IInterfaceTestProxyI* _test)
{
	try
	{
		int _res = m_TestClass->GetIntGSDF(gcnew IInterfaceTestProxyCPP(_test));
		return _res;
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

void TestClassCPP::DoStuff(double _val)
{
	try
	{
		m_TestClass->DoStuff(_val);
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

int TestClassCPP::DoMoreStuff(std::string _strVal)
{
	try
	{
		int _res = m_TestClass->DoMoreStuff(gcnew System::String(_strVal.c_str()));
		return _res;
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

