#define CPP_CLI_DLL

#include "TestClass.h"

#include <xstring>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>



////////////////////// Constructors //////////////////////
TestClassI* TestClass::NewTestClass()
{
	return new TestClassCPP(gcnew TestClass());
}

TestClassI* TestClass::NewTestClass(int _a, std::string _b)
{
	return new TestClassCPP(gcnew TestClass(_a, gcnew System::String(_b)));
}


////////////////////// Static Functions //////////////////////
int TestClass::StaticTwo()
{
	try
	{
		return m_TestClass->StaticTwo();
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

std::string TestClass::StaticThree()
{
	try
	{
		return m_TestClass->StaticThree();
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

BaseClassI* TestClass::NewBaseClass()
{
	try
	{
		return m_TestClass->NewBaseClass();
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}





////////////////////// Propeties //////////////////////
std::string TestClass::GetClassName()
{
	return m_TestClass->ClassName;
}

void TestClass::SetClassName(std::string _Value)
{
	m_TestClass->ClassName = gcnew System::String(_Value);
}

int TestClass::GetClassCount()
{
	return m_TestClass->ClassCount;
}

void TestClass::SetClassCount(int _Value)
{
	m_TestClass->ClassCount = _Value;
}


////////////////////// Functions //////////////////////
int TestClass::GetIntGSDF(IInterfaceTestI* _test)
{
	try
	{
		return m_TestClass->GetIntGSDF(gcnew IInterfaceTestProxy(_test));
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

void TestClass::DoStuff(double _val)
{
	try
	{
		m_TestClass->DoStuff(_val);
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

int TestClass::DoMoreStuff(std::string _strVal)
{
	try
	{
		return m_TestClass->DoMoreStuff(gcnew System::String(_strVal));
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

