#pragma once

#include "NativeObjectI.h"
#include "BaseClassI.h"
#include "IInterfaceTestProxyI.h"

class DLLEXPORT TestClassI : public BaseClassI
{
public:
	//! Constructors
	static TestClassI* NewTestClass();
	static TestClassI* NewTestClass(int _a, std::string _b);

	//! Static
	static int StaticTwo(std::string _a, int _b);
	static std::string StaticThree();
	static BaseClassI* NewBaseClass();

	//! Properties
	virtual std::string GetClassName()=0;
	virtual void SetClassName(std::string _Value)=0;
	virtual int GetClassCount()=0;
	virtual void SetClassCount(int _Value)=0;

	//! Methods
	virtual int GetIntGSDF(IInterfaceTestProxyI* _test)=0;
	virtual void DoStuff(double _val)=0;
	virtual int DoMoreStuff(std::string _strVal)=0;
};