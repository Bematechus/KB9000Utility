// KB9RW.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"
#include "KB9RW.h"
#include "KB9RWDLL.h"
#include "KB9Manager.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//
//TODO: If this DLL is dynamically linked against the MFC DLLs,
//		any functions exported from this DLL which call into
//		MFC must have the AFX_MANAGE_STATE macro added at the
//		very beginning of the function.
//
//		For example:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// normal function body here
//		}
//
//		It is very important that this macro appear in each
//		function, prior to any calls into MFC.  This means that
//		it must appear as the first statement within the 
//		function, even before any object variable declarations
//		as their constructors may generate calls into the MFC
//		DLL.
//
//		Please see MFC Technical Notes 33 and 58 for additional
//		details.
//


// CKB9RWApp

BEGIN_MESSAGE_MAP(CKB9RWApp, CWinApp)
END_MESSAGE_MAP()


// CKB9RWApp construction

CKB9RWApp::CKB9RWApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CKB9RWApp object

CKB9RWApp theApp;

CKB9Manager g_kb9Manager;

// CKB9RWApp initialization

BOOL CKB9RWApp::InitInstance()
{
	CWinApp::InitInstance();

	if (!AfxSocketInit())
	{
		AfxMessageBox(IDP_SOCKETS_INIT_FAILED);
		return FALSE;
	}

	return TRUE;
}



/************************************************************************/
/* 
Function: ReadKB9
Description:
Read the EEPROM content from the keypad via the specified interface, 
convert the data to the template cvs text format and store into 
the data buffer.
Parameters:
@buffer: Save data in this user buffer.
@nBufferSize: the buffer size, in bytes
@nInterface:
0 - PS2, 
1 - USB, 
2 - RS232
return:
>=0 The really read size. 
-1: - keypad not found at specified interface
-2: - keypad data read error
-3: - data buffer error (data buffer passed to API is invalid)
*/
/************************************************************************/
KB9RW_API int ReadKB9(LPTSTR buffer, int nBufferSize, int nInterface )
{
	return g_kb9Manager.ReadKB9(buffer, nBufferSize, nInterface);
	
}

/************************************************************************/
/* 
Function: WriteKB9
Descripton:
Takes the data structure input, convert it to the EEPROM storage format 
and send to keypad via the specified interface.
Parameters:
@data:
The CSV format KB9 configuration text string.
@nInterface:
0 - PS2, 
1 - USB, 
2 - RS232
Return error codes:
>=0 The really write length. 
-1 - keypad not found at specified interface
-2 - keypad data write error
-3 - data buffer error (data buffer passed to API is invalid)
-4 - data format error (data in buffer cannot be converted correctly)


*/
/************************************************************************/
KB9RW_API int WriteKB9(LPTSTR data, int nInterface)
{
	return g_kb9Manager.WriteKB9(data, nInterface);
}


/************************************************************************/
/* 
Detect if the KB9000 keyboard connected.
Parameters:
@nInterface:
0 - PS2, 
1 - USB, 
2 - RS232
return:
TRUE: KB connected.
FALSE: KB don't existed.
*/
/************************************************************************/
KB9RW_API bool DetectKB9(int nInterface)
{
	return g_kb9Manager.DetectKB9(nInterface);
}

/************************************************************************/
/* 
Send some user message to given window.
e.g: the download/upload progress value.

*/
/************************************************************************/
KB9RW_API void SetMessageReceiver(HWND hWnd)
{
	g_kb9Manager.SetMessageReceiver(hWnd);
}