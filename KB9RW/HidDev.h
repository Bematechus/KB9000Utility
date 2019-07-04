#pragma once
#include "afx.h"

class CHidDev :
	public CObject
{
public:
	CHidDev(void);
public:
	~CHidDev(void);
public://HID device operations
	//
	bool Open(CString strDevicePath);
	bool Close();
	bool IsOpened();
	CString GetManufacturerString();
	CString GetProductString();
	bool GetVidPidVer(USHORT *vid, USHORT *pid, USHORT *ver);
public:
	int ReadBlockData(BYTE nPackageNo, BYTE *data, int nDataBytes);
	int WriteBlockData(BYTE nPackageNo, BYTE *data, int nDataBytes);
	BOOL GetResponce( );
private:
	BOOL SendCmdAddr(BYTE nCmd, BYTE nPackageNo);
private:
	HANDLE m_hHidDevice;
};
