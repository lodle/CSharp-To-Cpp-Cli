#pragma once
using namespace CSharpTestLib;

#include "BaseClassI.h"
#include <msclr\gcroot.h>


class BaseClassCPP : public virtual BaseClassI
{
public:
	BaseClassCPP() { m_BaseClass = gcnew BaseClass(); }
	BaseClassCPP(BaseClass^ _Internal) { m_BaseClass = _Internal; }

	virtual int DoMoreStuff(const char* _strVal);

	virtual void ToString(char* szOutBuff, size_t nOutBuffSize);

	virtual int GetHashCode();

	BaseClass^ InternalObject(){return m_BaseClass;}
	virtual void Destroy(){delete this;}

private:
	msclr::gcroot<BaseClass^> m_BaseClass;
};
