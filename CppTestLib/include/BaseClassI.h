#pragma once

#include "NativeObjectI.h"

class DLLEXPORT BaseClassI : public NativeObjectI
{
public:
	//! Constructors
	static BaseClassI* NewBaseClass();

	//! Static
	static void StaticOne();

	//! Properties

	//! Methods
	virtual int DoMoreStuff(std::string _strVal)=0;
};