#pragma once
#include "pch.hpp"

class TestObject
{
public:
	TestObject();
	~TestObject();
	double GetDouble() const;
	void SetDouble(const double value);
private:
	double _doubleValue;
};

