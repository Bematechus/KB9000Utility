#pragma once
#include "afx.h"
#include "KB9Device.h"

class CKB9Manager :
	public CObject
{
public:
	CKB9Manager(void);
	~CKB9Manager(void);
public:
	void SetMessageReceiver(HWND hWnd);
	bool DetectKB9(int nInterface);
	int WriteKB9(LPTSTR data, int nInterface);
	int ReadKB9(LPTSTR buffer, int nBufferSize, int nInterface );
private:
	CKB9Device* CreateKB9Device(int nInterface);
	bool CreateDeviceInterface(int nInterface);
	bool InformParentProgressPercent(int nPercent);
private:
	HWND m_hwndMessageReceiver;
	CKB9Device *m_pKB9;
};
