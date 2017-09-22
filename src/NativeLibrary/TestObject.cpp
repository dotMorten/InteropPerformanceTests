#include "pch.hpp"
#include "TestObject.h"


TestObject::TestObject() : _doubleValue(std::numeric_limits<double>::quiet_NaN())
{
}


TestObject::~TestObject()
{
}

double TestObject::GetDouble()  const { return _doubleValue; }

void TestObject::SetDouble(const double value) { _doubleValue = value; }
