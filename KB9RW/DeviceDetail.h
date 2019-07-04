#pragma once
#include "afx.h"

class CDeviceDetail :
	public CObject
{
public:
	CDeviceDetail(void);

	~CDeviceDetail(void);
public:
	void reset();
	CDeviceDetail *Clone();
	bool CopyTo(CDeviceDetail *pDev);
	void dump();

public:
	CString m_deviceID;
	//CString m_installID;
	CString m_devicePath;
	CString m_Descriptioin;
	CString m_Driver;
	CString m_parentID;
	//CString m_hwID;

};

