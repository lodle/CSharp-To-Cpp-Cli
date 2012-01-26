#pragma once
#include "BaseClass.h"

#ifndef DLLFN
	#ifndef DLL_EXPORT
		#define DLLFN __declspec( dllimport )
	#else
		#define DLLFN __declspec( dllexport )
	#endif
#endif


class TestClassI : public BaseClassI
{
public:
	virtual void GetClassName(char* szOutBuff, size_t nOutBuffSize)=0;
	virtual void SetClassName(const char* _ClassName)=0;

	virtual int GetClassCount()=0;
	virtual void SetClassCount(int _ClassCount)=0;

	virtual int GetInt()=0;

	virtual void DoStuff(double _val)=0;

	virtual int DoMoreStuff(const char* _strVal)=0;

	virtual void ToString(char* szOutBuff, size_t nOutBuffSize)=0;

	virtual int GetHashCode()=0;

};

extern "C"
{
	DLLFN TestClassI* NewTestClass();
	DLLFN TestClassI* NewTestClass(int _a, const char* _b);

	DLLFN int TestClass_StaticTwo(const char* _a, int _b);
	DLLFN void TestClass_StaticThree(char* szOutBuff, size_t nOutBuffSize);
	DLLFN BaseClassI* TestClass_NewBaseClass();

#ifdef DLL_EXPORT
	TestClassI* NewTestClassCPP(CSharpTestLib::TestClass^ _Internal);
#endif
}
