#define DLL_EXPORT
#include "BaseClass.h"

using namespace CSharpTestLib;

class BaseClassCPP : public BaseClassI
{
public:
	BaseClassCPP(BaseClass^ _Internal) { m_BaseClass = _Internal; }

	virtual int DoMoreStuff(const char* _strVal)
	{
		return m_BaseClass->DoMoreStuff(gcnew System::String(_strVal));
	}

	virtual void ToString(char* szOutBuff, size_t nOutBuffSize)
	{
		System::String^ ret = m_BaseClass->ToString();
		std::string szRet = marshal_as<std::string>(ret);
		strncpy_s(szOutBuff, nOutBuffSize, szRet.c_str(), szRet.size());
	}

	virtual int GetHashCode()
	{
		return m_BaseClass->GetHashCode();
	}

	BaseClass^ InternalObject(){return m_BaseClass;}
	virtual void Destroy(){delete this;}

private:
	gcroot<BaseClass^> m_BaseClass;
};

extern "C"
{
	DLLFN BaseClassI* NewBaseClass()
	{
		return new BaseClassCPP();
	}


	DLLFN void BaseClass_StaticOne()
	{
		return BaseClass::StaticOne();
	}

	BaseClassI* NewBaseClassCPP(BaseClass^ _Internal)
	{
		return new BaseClassCPP(_Internal);
	}
}
