#include "StdAfx.h"
#include "KB9Manager.h"
#include "KB9PS2.h"
#include "KB9USB.h"
#include "KB9RS232.h"
#include "KB9RWDLL.h"
CKB9Manager::CKB9Manager(void)
{
	m_hwndMessageReceiver = NULL;
	m_pKB9 = NULL;
}

CKB9Manager::~CKB9Manager(void)
{
	if (m_pKB9 != NULL)
		delete m_pKB9;
	m_pKB9 = NULL;
	m_hwndMessageReceiver = NULL;
}

void CKB9Manager::SetMessageReceiver(HWND hWnd)
{
	m_hwndMessageReceiver = hWnd;
}
/************************************************************************/
/* 

*/
/************************************************************************/
bool CKB9Manager::DetectKB9(int nInterface)
{
	if (!CreateDeviceInterface(nInterface))
		return false;

	if (m_pKB9 != NULL)
		return m_pKB9->DetectKB9();
	return false;
}
/************************************************************************/
/*                                                                      */
/************************************************************************/
int CKB9Manager::WriteKB9(LPTSTR data, int nInterface)
{
	return 0;
}
/************************************************************************/
/*                                                                      */
/************************************************************************/
int CKB9Manager::ReadKB9(LPTSTR buffer, int nBufferSize, int nInterface )
{
	return 0;
}
/************************************************************************/
/*                                                                      */
/************************************************************************/
CKB9Device* CKB9Manager::CreateKB9Device(int nInterface)
{
	switch(nInterface)
	{
	case KB9_INTERFACE::PS2:
		{
			return new CKB9PS2();
		}
		break;
	case KB9_INTERFACE::USB:
		return new CKB9USB();
	case KB9_INTERFACE::RS232:
		return new CKB9RS232;
	default:
		return NULL;
	}
}
/************************************************************************/
/*                                                                      */
/************************************************************************/
bool CKB9Manager::CreateDeviceInterface(int nInterface)
{
	if (m_pKB9 != NULL)
	{
		if (m_pKB9->InterfaceType() == nInterface)
			return true;
		else
		{
			delete m_pKB9;

		}
	}
	m_pKB9 = CreateKB9Device(nInterface);
	return (m_pKB9 != NULL);
}

bool CKB9Manager::InformParentProgressPercent(int nPercent)
{
	if (m_hwndMessageReceiver != NULL)
		return PostMessage(m_hwndMessageReceiver, UM_KB9_PROGRESS, nPercent, 0);
	return false;

}