#define CPP_CLI_DLL

#include "BaseClass.h"

#include <xstring>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>



////////////////////// Constructors //////////////////////
BaseClassI* BaseClassI::NewBaseClass()
{
	return new BaseClassCPP(gcnew BaseClass());
}


////////////////////// Static Functions //////////////////////
void BaseClassI::StaticOne()
{
	try
	{
		BaseClass::StaticOne();
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}





////////////////////// Propeties //////////////////////

////////////////////// Functions //////////////////////
int BaseClassCPP::DoMoreStuff(std::string _strVal)
{
	try
	{
		int _res = m_BaseClass->DoMoreStuff(gcnew System::String(_strVal.c_str()));
		return _ret;
	}
	catch (System::Exception^ e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

