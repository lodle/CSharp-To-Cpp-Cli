#define DLL_EXPORT
#include "TestClass.h"
#include <xstring>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

void TestClassCPP::GetClassName(char* szOutBuff, size_t nOutBuffSize)
{
		System::String^ ret = m_TestClass->ClassName;
		std::string szRet = msclr::interop::marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
}

void TestClassCPP::SetClassName(const char* _ClassName)
{
	m_TestClass->ClassName = gcnew System::String(_ClassName);
}

int TestClassCPP::GetClassCount()
{
		return m_TestClass->ClassCount;
}

void TestClassCPP::SetClassCount(int _ClassCount)
{
	m_TestClass->ClassCount = _ClassCount;
}

int TestClassCPP::GetInt()
{
	try
	{
		return m_TestClass->GetInt();
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
		return m_TestClass->DoStuff(_val);
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());
	}
}

int TestClassCPP::DoMoreStuff(const char* _strVal)
{
	try
	{
		return m_TestClass->DoMoreStuff(gcnew System::String(_strVal));
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());
	}
}

void TestClassCPP::ToString(char* szOutBuff, size_t nOutBuffSize)
{
	try
	{
		System::String^ ret = m_TestClass->ToString();
		std::string szRet = msclr::interop::marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());
	}
}

int TestClassCPP::GetHashCode()
{
	try
	{
		return m_TestClass->GetHashCode();
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());
	}
}


extern "C"
{
	DLLFN TestClassI* NewTestClass()
	{
		return new TestClassCPP(gcnew TestClass());
	}

	DLLFN TestClassI* NewTestClass1(int _a, const char* _b)
	{
		return new TestClassCPP(gcnew TestClass(_a, gcnew System::String(_b)));
	}


	DLLFN int TestClass_StaticTwo(const char* _a, int _b)
	{
		return TestClass::StaticTwo(gcnew System::String(_a), _b);
	}

	DLLFN void TestClass_StaticThree(char* szOutBuff, size_t nOutBuffSize)
	{
		System::String^ ret = TestClass::StaticThree();
		std::string szRet = msclr::interop::marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
	}

	DLLFN BaseClassI* TestClass_NewBaseClass()
	{
		BaseClass^ ret = TestClass::NewBaseClass();
		return new BaseClassCPP(ret);
	}

}
