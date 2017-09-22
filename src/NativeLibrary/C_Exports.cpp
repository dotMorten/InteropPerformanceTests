#pragma once

#include "pch.hpp"

// Macro to flag an export function
#ifdef _MSC_VER
#define DLLEXPORT extern "C" __declspec(dllexport)
#else
#ifdef ANDROID
#include <jni.h>
#define DLLEXPORT extern "C" JNIEXPORT
#else // iOS
#define DLLEXPORT extern "C"
#endif
#define WINAPI
#endif

//
//
//DLLEXPORT double WINAPI GetDouble(const ObjectHandle handle)
//{
//	auto return_result = GetDouble(handle);
//	return return_result;
//}

DLLEXPORT int next(int n)
{
	return n + 1;
}

DLLEXPORT void* TestObject_Create()
{
	TestObject* to = new TestObject();
	void* result = to;
	return result;
}

DLLEXPORT void TestObject_Destroy(void* object)
{
	auto to = reinterpret_cast<TestObject*>(object);
	delete(to);
}

DLLEXPORT double TestObject_GetDouble(const void* object)
{
	auto to = reinterpret_cast<const TestObject*>(object);
	return to->GetDouble();
}

DLLEXPORT void TestObject_SetDouble(void* object, double value)
{
	auto to = reinterpret_cast<TestObject*>(object);
	to->SetDouble(value);
}
