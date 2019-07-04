
#ifndef _KB9RWDLL_
#define _KB9RWDLL_

#define DLL_EXPORTS

#ifdef DLL_EXPORTS
#define KB9RW_API __declspec(dllexport)
#else
#define KB9RW_API __declspec(dllimport)
#endif
//--------------------------------------------------------------------------
/* Ver: 0.0.0.1
   Notes:
		!!!! Please use the UNICODE to code DLL.!!!!
		If you have to use ANSI, please let me know.
   History: 
		1. 2013-10-28, first version.
	
	

*/



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
KB9RW_API int ReadKB9(LPTSTR buffer, int nBufferSize, int nInterface );

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
KB9RW_API int WriteKB9(LPTSTR data, int nInterface);


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
KB9RW_API bool DetectKB9(int nInterface);


/************************************************************************/
/* 
	Cancel current operation.
		e.g: detect, read/write.
Parameters:
	@nTimeout:
		The timeout ms.

return:
		True: canceled.
		false: Time out
		

*/
/************************************************************************/
KB9RW_API bool Cancel(int nTimeout);


/************************************************************************/
/* 
Send some user message to given window.
e.g: the detect/download/upload progress value.

*/
/************************************************************************/
KB9RW_API void SetMessageReceiver(HWND hWnd);

/************************************************************************/
/* 
send this message to given window while the progress value changed.
The progress value range [0, 100] %.
There are two place we need this message:
1. Detect KB, we need the progress.
2. Download/Upload firmware data.

Usage:
	SendMessage(hWnd, UM_KB9_PROGRESS, progress_value, 0);
	or	
	PostMessage(hWnd, UM_KB9_PROGRESS, progress_value, 0);
*/
/************************************************************************/
#define UM_KB9_PROGRESS WM_USER + 1001


#endif