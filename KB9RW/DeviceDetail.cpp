#include "StdAfx.h"
#include "DeviceDetail.h"

CDeviceDetail::CDeviceDetail(void)
{
	reset();
}

CDeviceDetail::~CDeviceDetail(void)
{
}

void CDeviceDetail::reset()
{
	 m_deviceID = _T("");
//	 m_installID = _T("");
	 m_devicePath = _T("");
	 m_Descriptioin = _T("");
	 m_Driver = _T("");
	 m_parentID = _T("");
	// m_hwID = _T("");
}

CDeviceDetail *CDeviceDetail::Clone()
{
	CDeviceDetail *dev = new CDeviceDetail();
	CopyTo(dev);
	return dev;

}
bool CDeviceDetail::CopyTo(CDeviceDetail *pDev)
{
	 pDev->m_deviceID = m_deviceID ;
	 //pDev->m_installID = m_installID;
	 pDev->m_devicePath = m_devicePath;
	 pDev->m_Descriptioin = m_Descriptioin;
	 pDev->m_Driver = m_Driver;
	 pDev->m_parentID = m_parentID;
	// pDev->m_hwID = m_hwID;
	 return true;
}

void CDeviceDetail::dump()
{
	TRACE(_T("================================================\n"));
	TRACE(_T("m_deviceID=%s\n"), m_deviceID);
	//TRACE(_T(" m_installID=%s\n"),m_installID);
	TRACE(_T(" m_devicePath=%s\n"),m_devicePath);
	TRACE(_T(" m_Descriptioin=%s\n"),m_Descriptioin);
	TRACE(_T(" m_Driver=%s\n"),m_Driver);
	TRACE(_T(" m_parentID=%s\n"),m_parentID);
	//TRACE(_T(" m_hwID=%s\n"),m_hwID);
	TRACE(_T("================================================\n"));
}