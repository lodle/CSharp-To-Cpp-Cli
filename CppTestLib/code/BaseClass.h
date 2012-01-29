#pragma once

using namespace CSharpTestLib;

#include "BaseClassI.h"

#include <msclr\gcroot.h>


class BaseClassCPP : public virtual BaseClassI, public NativeObjectCPP
{
public:
	//! Constructors
	BaseClassCPP() : m_BaseClass(gcnew BaseClass()), BaseClassCPP(m_BaseClass) 
	{
	}

	BaseClassCPP(BaseClass^ _Internal) : m_BaseClass(_Internal), BaseClassCPP(m_BaseClass) 
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

private:
	msclr::gcroot<BaseClass^> m_BaseClass;
};
