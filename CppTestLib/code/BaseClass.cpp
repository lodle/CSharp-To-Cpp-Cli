#define CPP_CLI_DLL

#include "BaseClass.h"

#include <xstring>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>



////////////////////// Constructors //////////////////////
BaseClassI* BaseClass::NewBaseClass()
{
	return new BaseClassCPP(gcnew BaseClass());
}


////////////////////// Static Functions //////////////////////
void BaseClass::StaticOne()
{
	try
	{
		m_BaseClass->StaticOne();
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}





////////////////////// Propeties //////////////////////

////////////////////// Functions //////////////////////
int BaseClass::DoMoreStuff(std::string _strVal)
{
	try
	{
		return m_BaseClass->DoMoreStuff(gcnew System::String(_strVal));
	}
	catch (System::Exception e)
	{
		std::string msg = msclr::interop::marshal_as<std::string>(e->Message);
		throw std::exception(msg.c_str());		
	}
}

