#pragma once

#ifndef DLLFN
	#ifndef DLL_EXPORT
		#define DLLFN __declspec( dllimport )
	#else
		#define DLLFN __declspec( dllexport )
	#endif
#endif


class BaseClassI
{
public:
	virtual int DoMoreStuff(const char* _strVal)=0;

	virtual void ToString(char* szOutBuff, size_t nOutBuffSize)=0;

	virtual int GetHashCode()=0;

};

extern "C"
{
	DLLFN BaseClassI* NewBaseClass();

	DLLFN void BaseClass_StaticOne();

#ifdef DLL_EXPORT
	BaseClassI* NewBaseClassCPP(CSharpTestLib::BaseClass^ _Internal);
#endif
}
