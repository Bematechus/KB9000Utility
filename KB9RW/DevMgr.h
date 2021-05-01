#pragma once
#include "afx.h"
#include "DeviceDetail.h"

#include <Cfgmgr32.h>
// get setup API functions (only available in Win98 and Win2K)
#include <setupapi.h>
// requires to link with setupapi.lib
// Link with SetupAPI.Lib.
#pragma comment (lib, "setupapi.lib")

#include "usb100.h"
#include <string>

#include   "objbase.h"   
#include   "initguid.h"

class CDevMgr :
	public CObject
{
public:
	CDevMgr(void);
public:
	~CDevMgr(void);
public:
	int EnumAllMonitors(CObArray *par);
	int EnumAllAudios(CObArray *par);
	int EnumDevicesInterface(GUID guid, CObArray *par);
	int EnumDevicesClass(GUID guid, CObArray *par);
	int EnumDisplays();

	static bool DeleteAllObjs(CObArray *par);

private:
	DWORD GetDevicesCount(HDEVINFO hDevInfoList,GUID guid);
	bool GetDeviceInformation(HDEVINFO hDevInfoList,GUID guid, int nIndex, CDeviceDetail *pDev);
	CString GetDriverName(HDEVINFO hDevInfoList, PSP_DEVINFO_DATA pDevData);
	CString GetHardwareID(HDEVINFO hDevInfoList, PSP_DEVINFO_DATA pDevData);
	bool GetDeviceInformation2(HDEVINFO hDevInfoList, int nIndex, CDeviceDetail *pDev);
	CString GetDeviceID(HDEVINFO hDevInfo, SP_DEVINFO_DATA* pspDevInfoData);
	CString GetDeviceInterfaceInfo(HDEVINFO hDevInfo, PSP_DEVINFO_DATA spDevInfoData);
	CString GetDeviceFriendlyName(HDEVINFO hDevInfo, PSP_DEVINFO_DATA spDevInfoData);
	bool GetDeviceInformationFromID(CString devID, CDeviceDetail *pDev);
	
};
