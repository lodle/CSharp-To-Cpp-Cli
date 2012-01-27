#define DLL_EXPORT
#include "BaseClass.h"
#include <xstring>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

int BaseClassCPP::DoMoreStuff(const char* _strVal)
{
	try
	{
		return m_BaseClass->DoMoreStuff(gcnew System::String(_strVal));
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());
	}
}

void BaseClassCPP::ToString(char* szOutBuff, size_t nOutBuffSize)
{
	try
	{
		System::String^ ret = m_BaseClass->ToString();
		std::string szRet = msclr::interop::marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());
	}
}

int BaseClassCPP::GetHashCode()
{
	try
	{
		return m_BaseClass->GetHashCode();
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());
	}
}


extern "C"
{
	DLLFN BaseClassI* NewBaseClass()
	{
		return new BaseClassCPP(gcnew BaseClass());
	}


	DLLFN void BaseClass_StaticOne()
	{
		return BaseClass::StaticOne();
	}

}
