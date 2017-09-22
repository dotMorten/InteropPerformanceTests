// COPYRIGHT 1995-2012 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com
/// \file pch.hpp

// Windows API definitions
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
#endif

#if defined(_MSC_VER)
#include <Windows.h>
#endif

// C
#include <stdlib.h>
#if !__APPLE__
#include <malloc.h>
#endif
#include <assert.h>
#include <ctype.h>
#if defined(_MSC_VER)
#include <io.h>
#else
#include <sys/uio.h>
#endif
#include <float.h>
#include <memory.h>
#include <stdio.h>
#include <time.h>
#include <wctype.h>
#if defined(_MSC_VER)
#include <mmsystem.h>
#include <tchar.h>
#include <strsafe.h>

// Get Windows ComPtr
#include <wrl/client.h>

// Windows Shell header files
#if !__cplusplus_winrt
#include <Shlobj.h>
#include <Shlwapi.h>
#endif

// DirectX
#if !__cplusplus_winrt
#include <d3d9.h>
#endif
#include <d3d11.h>

#endif

// C++ Std libraries
#include <mutex>
#include <map>
#include <algorithm>
#include <atomic>
#include <array>
#include <cctype>
#include <cmath>
#include <deque>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <limits>
#include <list>
#include <map>
#include <memory>
#include <set>
#include <sstream>
#include <stack>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>
#include <unordered_map>
#include <unordered_set>
#include <thread>
#include <condition_variable>

// Macro for marking exported methods for use by p/invoke from WPF
#if defined(_MSC_VER)
#define EXPORT extern "C" __declspec(dllexport)
#else
#define EXPORT extern "C" 
#define WINAPI
#define _S_IFDIR 0x4000
#define HRESULT long
typedef const wchar_t *LPCWSTR;
typedef const char *LPCSTR;
typedef unsigned char BYTE;
typedef BYTE *LPBYTE;
typedef void *LPVOID;

#ifndef DCONST
#define DCONST
typedef union
{	/* pun float types as integer array */
	unsigned short _Word[8];
	float _Float;
	double _Double;
	long double _Long_double;
} _Dconst; 
#endif
#endif
#define EXT_CLASS __declspec(dllexport)
#define EXPORTED EXPORT bool WINAPI 

// WinRT API
#if __cplusplus_winrt
#include <collection.h>
#include <ppltasks.h>
#include <algorithm>
#endif

#if defined(_MSC_VER)
// windows sockets - do we need this?
#pragma comment(lib, "Ws2_32.lib")

// WRL
#include <wrl.h>
#include <wrl\client.h>
#if __cplusplus_winrt
#include <agile.h>
#endif

// DirectX
#include <d3d11_1.h>
#include <DirectXMath.h>

#include <dxgi.h>
#include <dxgi1_2.h>
#include <dxgi1_3.h>

// WinStore specific 
#if __cplusplus_winrt
#include <windows.ui.xaml.media.dxinterop.h>
#endif
#endif

#define RT_PUBLIC __declspec(dllimport)

#include "TestObject.h"
