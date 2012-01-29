#pragma once

using namespace CSharpTestLib;

#include "BaseClassI.h"
#include "NativeObject.h"


#include <msclr\gcroot.h>


class BaseClassCPP : public virtual BaseClassI, public NativeObjectCPP
{
public:
	//! Constructors
	BaseClassCPP() : m_BaseClass(gcnew BaseClass()), NativeObjectCPP(m_BaseClass) 
	{
	}

	BaseClassCPP(BaseClass^ _Internal) : m_BaseClass(_Internal), NativeObjectCPP(m_BaseClass) 
	{
	}	

	//! Properties

	//! Methods
	virtual int DoMoreStuff(std::string _strVal);

	BaseClass^ InternalObject()
	{
		return m_BaseClass;
	}

	virtual void Destroy()
	{
		delete this;
	}

	virtual int GetHashCode()
	{
		return NativeObjectCPP::GetHashCode();
	}
	
	virtual std::string ToString()
	{
		return NativeObjectCPP::ToString();
	}

private:
	msclr::gcroot<BaseClass^> m_BaseClass;
};
