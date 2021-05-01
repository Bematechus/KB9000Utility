// KB9RW.h : main header file for the KB9RW DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CKB9RWApp
// See KB9RW.cpp for the implementation of this class
//

class CKB9RWApp : public CWinApp
{
public:
	CKB9RWApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
