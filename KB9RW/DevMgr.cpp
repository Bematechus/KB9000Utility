#include "StdAfx.h"
#include "DevMgr.h"

CDevMgr::CDevMgr(void)
{
}

CDevMgr::~CDevMgr(void)
{
}

DWORD CDevMgr::GetDevicesCount(HDEVINFO hDevInfoList, GUID guid)
{
	DWORD numDevices = 0;

	// Retrieve device list for GUID that has been specified.
	//HDEVINFO hDevInfoList = SetupDiGetClassDevs (&guid, NULL, NULL, (DIGCF_PRESENT | DIGCF_DEVICEINTERFACE)); 

	//if (hDevInfoList != NULL)
	{
		SP_DEVICE_INTERFACE_DATA deviceInfoData;

		for (int index = 0; index < 256;index++)
		{
			// Clear data structure
			ZeroMemory(&deviceInfoData, sizeof(deviceInfoData));
			deviceInfoData.cbSize = sizeof(SP_DEVICE_INTERFACE_DATA);

			// retrieves a context structure for a device interface of a device information set.
			if (SetupDiEnumDeviceInterfaces (hDevInfoList, 0, &guid, index, &deviceInfoData)) 
			{
				numDevices++;
			}
			else
			{
				if ( GetLastError() == ERROR_NO_MORE_ITEMS ) 
					break;
			}
		}
	}

	// SetupDiDestroyDeviceInfoList() destroys a device information set
	// and frees all associated memory.
	//SetupDiDestroyDeviceInfoList (hDevInfoList);

	return numDevices;
}

CString CDevMgr::GetDriverName(HDEVINFO hDevInfoList, PSP_DEVINFO_DATA pDevData)
{
	TCHAR buffer[MAX_PATH];
	memset(buffer, 0, MAX_PATH * sizeof(TCHAR));
	if (SetupDiGetDeviceRegistryProperty(hDevInfoList,
										pDevData,
										SPDRP_DRIVER,
										0L,
										(PBYTE)buffer,
										MAX_PATH-1,
										0))
	{
		CString s = buffer;

		return s;
	}
	return _T("");

}


CString CDevMgr::GetHardwareID(HDEVINFO hDevInfoList, PSP_DEVINFO_DATA pDevData)
{
	TCHAR buffer[MAX_PATH];
	memset(buffer, 0, MAX_PATH * sizeof(TCHAR));
	if (SetupDiGetDeviceRegistryProperty(hDevInfoList,
										pDevData,
										SPDRP_HARDWAREID,
										0L,
										(PBYTE)buffer,
										MAX_PATH-1,
										0))
	{
		CString s = buffer;

		return s;
	}
	return _T("");

}




bool CDevMgr::GetDeviceInformation(HDEVINFO hDevInfoList,GUID guid, int nIndex, CDeviceDetail *pDev)
{
	SP_DEVICE_INTERFACE_DATA deviceInfoData;
	// Clear data structure
	ZeroMemory(&deviceInfoData, sizeof(deviceInfoData));
	deviceInfoData.cbSize = sizeof(SP_DEVICE_INTERFACE_DATA);

	// retrieves a context structure for a device interface of a device information set.
	if (!SetupDiEnumDeviceInterfaces(hDevInfoList, 0, &guid, nIndex, &deviceInfoData)) 
		return false;
	//SP_DEVINFO_DATA spDevInfoData    = {0};
	// spDevInfoData.cbSize = sizeof(SP_DEVINFO_DATA);
	//if (SetupDiEnumDeviceInfo(hDevInfoList,
	//	nIndex,
	//	&spDevInfoData)) return false;
	//{
	// Must get the detailed information in two steps
	// First get the length of the detailed information and allocate the buffer
	// retrieves detailed information about a specified device interface.
	PSP_DEVICE_INTERFACE_DETAIL_DATA     functionClassDeviceData = NULL;
	ULONG  predictedLength, requiredLength;

	predictedLength = requiredLength = 0;
	SetupDiGetDeviceInterfaceDetail (	hDevInfoList,
		&deviceInfoData,
		NULL,			// Not yet allocated
		0,				// Set output buffer length to zero 
		&requiredLength,// Find out memory requirement
		NULL);			

	predictedLength = requiredLength;
	functionClassDeviceData = (PSP_DEVICE_INTERFACE_DETAIL_DATA)malloc (predictedLength);
	functionClassDeviceData->cbSize = sizeof (SP_DEVICE_INTERFACE_DETAIL_DATA);

	SP_DEVINFO_DATA did = {sizeof(SP_DEVINFO_DATA)};

	// Second, get the detailed information
	if (! SetupDiGetDeviceInterfaceDetail (	hDevInfoList,
		&deviceInfoData,
		functionClassDeviceData,
		predictedLength,
		&requiredLength,
		&did)) 
		return false;

	TCHAR fname[MAX_PATH];

	// Try by friendly name first.
	if (!SetupDiGetDeviceRegistryProperty(hDevInfoList, &did, SPDRP_FRIENDLYNAME, NULL, (PBYTE) fname, sizeof(fname), NULL))
	{	// Try by device description if friendly name fails.
		if (!SetupDiGetDeviceRegistryProperty(hDevInfoList, &did, SPDRP_DEVICEDESC, NULL, (PBYTE) fname, sizeof(fname), NULL))
		{	// Use the raw path information for linkname and friendlyname
			_tcsncpy(fname, functionClassDeviceData->DevicePath, MAX_PATH);
		}
	}
	pDev->m_devicePath = functionClassDeviceData->DevicePath;
	pDev->m_Descriptioin = fname;
	free( functionClassDeviceData );
	//_stprintf( dev.m_linkname, _T("%s"), functionClassDeviceData->DevicePath);

	//_stprintf( dev.m_friendlyname, _T("%s"), fname);
	DEVINST hParent ;
	TCHAR buffer[MAX_PATH];
	if (CM_Get_Parent(&hParent, did.DevInst, 0) == CR_SUCCESS)
	{
		
		if (CM_Get_Device_ID(hParent, buffer, MAX_PATH, 0) == CR_SUCCESS);
		{
			pDev->m_parentID = buffer;
			TRACE(_T("parent=%s\n"), buffer);
		}
	}
	if (CM_Get_Device_ID(did.DevInst, buffer, MAX_PATH, 0) == CR_SUCCESS)
	{
		pDev->m_deviceID = buffer;
		TRACE(_T("myid=%s\n"), buffer);
	}
	pDev->m_Driver = GetDriverName(hDevInfoList, &did);
//	pDev->m_hwID = GetHardwareID(hDevInfoList, &did);
	
	return true;
	
	//}
	//}
}

CString CDevMgr::GetDeviceID(HDEVINFO hDevInfo, SP_DEVINFO_DATA* pspDevInfoData)
{
	TCHAR buffer[MAX_PATH];
	if (!SetupDiGetDeviceInstanceId(hDevInfo,
		pspDevInfoData,
		buffer,
		MAX_PATH,
		0))
		return _T("");
	CString s = buffer;
	return s;
}

CString CDevMgr::GetDeviceInterfaceInfo(HDEVINFO hDevInfo, SP_DEVINFO_DATA *spDevInfoData)
{
	SP_DEVICE_INTERFACE_DATA spDevInterfaceData = {0};
	//
	spDevInterfaceData.cbSize = sizeof(SP_DEVICE_INTERFACE_DATA);
	if (!SetupDiCreateDeviceInterface(hDevInfo,
		spDevInfoData,
		&(spDevInfoData->ClassGuid),
		0L,
		0L,
		&spDevInterfaceData))
		return _T("");
		
	else
	{
		SP_DEVICE_INTERFACE_DETAIL_DATA *pspDevInterfaceDetail = 0L;
		DWORD                           dwRequire              = 0L;
		//
		if (!SetupDiGetDeviceInterfaceDetail(hDevInfo,
			&spDevInterfaceData,
			0L,
			0,
			&dwRequire,
			0L))
		{
			DWORD dwError = GetLastError();
			//
			if (dwError != ERROR_INSUFFICIENT_BUFFER)
			{
				//ShowErrorMsg(_hDlg, dwError, "SetupDiBuildDriverInfoList");
				return _T("");
			};
		};
		//
		pspDevInterfaceDetail = (SP_DEVICE_INTERFACE_DETAIL_DATA*)LocalAlloc(LPTR,
			sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA)*dwRequire);
		pspDevInterfaceDetail->cbSize = sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA);
		if (!SetupDiGetDeviceInterfaceDetail(hDevInfo,
			&spDevInterfaceData,
			pspDevInterfaceDetail,
			dwRequire,
			&dwRequire,
			0L))
		{
			DWORD dwError = GetLastError();
			if (pspDevInterfaceDetail)
				LocalFree(pspDevInterfaceDetail);
			//
			if (dwError != ERROR_INSUFFICIENT_BUFFER)
				return _T("");
				//ShowErrorMsg(_hDlg, dwError, "SetupDiBuildDriverInfoList");
		}
		else
		{
			CString s = pspDevInterfaceDetail->DevicePath;
			if (pspDevInterfaceDetail)
				LocalFree(pspDevInterfaceDetail);
			//memcpy(szPath, pspDevInterfaceDetail->DevicePath,
			//	strlen(pspDevInterfaceDetail->DevicePath));
			//            switch(spDevInterfaceData.                    
			return s;
		};
		//
		//if (pspDevInterfaceDetail)
		//	LocalFree(pspDevInterfaceDetail);
	};
}

CString CDevMgr::GetDeviceFriendlyName(HDEVINFO hDevInfo, SP_DEVINFO_DATA *spDevInfoData)
{
	TCHAR buffer[MAX_PATH];
	memset(buffer, 0, MAX_PATH * sizeof(TCHAR));

	if (SetupDiGetDeviceRegistryProperty(hDevInfo,
		spDevInfoData,
		SPDRP_FRIENDLYNAME,
		0L,
		(PBYTE)buffer,
		MAX_PATH-1,
		0))
	{
		return buffer;

		
	}
	else if (SetupDiGetDeviceRegistryProperty(hDevInfo,
		spDevInfoData,
		SPDRP_DEVICEDESC,
		0L,
		(PBYTE)buffer,
		MAX_PATH-1,
		0))
	{
		return buffer;
		
	}
	return _T("");
}

bool CDevMgr::GetDeviceInformation2(HDEVINFO hDevInfoList, int nIndex, CDeviceDetail *pDev)
{
	SP_DEVINFO_DATA deviceInfoData;
	// Clear data structure
	ZeroMemory(&deviceInfoData, sizeof(deviceInfoData));
	deviceInfoData.cbSize = sizeof(SP_DEVINFO_DATA);

	// retrieves a context structure for a device interface of a device information set.
	//if (!SetupDiEnumDeviceInterfaces(hDevInfoList, 0, &guid, nIndex, &deviceInfoData)) 
	//	return false;
	//SP_DEVINFO_DATA spDevInfoData    = {0};
	// spDevInfoData.cbSize = sizeof(SP_DEVINFO_DATA);
	if (!SetupDiEnumDeviceInfo(hDevInfoList,
		nIndex,
		&deviceInfoData)) 
		return false;

	pDev->m_deviceID = GetDeviceID(hDevInfoList, &deviceInfoData);
	pDev->m_devicePath = GetDeviceInterfaceInfo(hDevInfoList, &deviceInfoData);
	pDev->m_Descriptioin = GetDeviceFriendlyName(hDevInfoList, &deviceInfoData);

	
	DEVINST hParent ;
	TCHAR buffer[MAX_PATH];
	if (CM_Get_Parent(&hParent, deviceInfoData.DevInst, 0) == CR_SUCCESS)
	{

		if (CM_Get_Device_ID(hParent, buffer, MAX_PATH, 0) == CR_SUCCESS);
		{
			pDev->m_parentID = buffer;
			//TRACE(_T("parent=%s\n"), buffer);
		}
	}
	if (CM_Get_Device_ID(deviceInfoData.DevInst, buffer, MAX_PATH, 0) == CR_SUCCESS)
	{
		pDev->m_deviceID = buffer;
		//TRACE(_T("myid=%s\n"), buffer);
	}
	pDev->m_Driver = GetDriverName(hDevInfoList, &deviceInfoData);
	//	pDev->m_hwID = GetHardwareID(hDevInfoList, &did);

	return true;

	//}
	//}
}


/*=============================================================================
enum all devices according the interface GUID value.
Parameters:
@guid: the class guid value
@par: return found device 
	  It is CDeviceDetail array.
return:
	-1: error
	0: nothing
	>0: found how many devices
===============================================================================*/
int CDevMgr::EnumDevicesInterface(GUID guid, CObArray *par)
{

	// Retrieve device list for GUID that has been specified.
	HDEVINFO hDevInfoList = SetupDiGetClassDevs (&guid, NULL, NULL, (DIGCF_PRESENT | DIGCF_DEVICEINTERFACE)); 
	if (hDevInfoList != NULL)
	{
		//int nCount = GetDevicesCount(hDevInfoList, guid);
		//if (nCount <=0) return nCount;
		CDeviceDetail dev;
		for (int i=0; i< 256; i++)
		{
			
			dev.reset();
			if (!GetDeviceInformation(hDevInfoList,guid, i,&dev))
				break;
				//return par->GetCount();
			else
			{
				dev.dump();
				par->Add(dev.Clone());
				
			}
		}
		
		
	}

	SetupDiDestroyDeviceInfoList (hDevInfoList);
	return par->GetCount();
}

/************************************************************************/
/* 
enum devices according to the class guid value

*/
/************************************************************************/
int CDevMgr::EnumDevicesClass(GUID guid, CObArray *par)
{

	// Retrieve device list for GUID that has been specified.
	HDEVINFO hDevInfoList = SetupDiGetClassDevs (&guid, NULL, NULL, (DIGCF_PRESENT));// | DIGCF_DEVICEINTERFACE)); 
	if (hDevInfoList != NULL)
	{
		//int nCount = GetDevicesCount(hDevInfoList, guid);
		//if (nCount <=0) return nCount;
		CDeviceDetail dev;
		for (int i=0; i< 256; i++)
		{

			dev.reset();
			if (!GetDeviceInformation2(hDevInfoList, i,&dev))
				break;
			//return par->GetCount();
			else
			{
				dev.dump();
				par->Add(dev.Clone());
				TRACE(_T("--------------Parent---------------\n"));
				CString devid =dev.m_parentID;
				dev.reset();
				GetDeviceInformationFromID(devid, &dev);
				dev.dump();
				TRACE(_T("--------------End Parent---------------\n"));
			}
		}


	}

	SetupDiDestroyDeviceInfoList (hDevInfoList);
	return par->GetCount();
}
/************************************************************************/
/* 
according the device ID value to get device informations

*/
/************************************************************************/
bool CDevMgr::GetDeviceInformationFromID(CString devID, CDeviceDetail *pDev)
{
	HDEVINFO hDevInfoList = SetupDiGetClassDevs (NULL, NULL, NULL, (DIGCF_PRESENT|DIGCF_ALLCLASSES));// | DIGCF_DEVICEINTERFACE)); 
	if (hDevInfoList == NULL) return false;

	//find it in device tree
	//DEVINST devInst = NULL;
	//if (!CM_Locate_DevNode(&devInst, (DEVINSTID)devID.GetBuffer(), CM_LOCATE_DEVNODE_NORMAL) == CR_SUCCESS)
	//{
	//	SetupDiDestroyDeviceInfoList (hDevInfoList);
	//	return false;
	//}


	SP_DEVINFO_DATA deviceInfoData;
	// Clear data structure
	ZeroMemory(&deviceInfoData, sizeof(deviceInfoData));
	deviceInfoData.cbSize = sizeof(SP_DEVINFO_DATA);

	for (int i=0;SetupDiEnumDeviceInfo(hDevInfoList,i,&deviceInfoData);i++)   
	{
		if (GetDeviceID(hDevInfoList, &deviceInfoData) == devID)
		//if (deviceInfoData.DevInst == devInst)
		{
			if (GetDeviceInformation2(hDevInfoList,  i, pDev))
			{
				SetupDiDestroyDeviceInfoList (hDevInfoList);
				return true;
			}
		}
	}
	
	SetupDiDestroyDeviceInfoList (hDevInfoList);
	return false;
}
DEFINE_GUID( GUID_DEVCLASS_MONITOR,  0x4D36E96E, 0xE325, 0x11CE, 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18 );


int CDevMgr::EnumAllMonitors(CObArray *par)
{
	return EnumDevicesClass(GUID_DEVCLASS_MONITOR,par );
}
DEFINE_GUID( GUID_DEVCLASS_SOUND,  0x4D36E97C, 0xE325, 0x11CE, 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18 );
int CDevMgr::EnumAllAudios(CObArray *par)
{
	return EnumDevicesClass(GUID_DEVCLASS_SOUND,par );
}



BOOL CALLBACK MyInfoEnumProc(
							 HMONITOR hMonitor,
							 HDC hdcMonitor,
							 LPRECT lprcMonitor,
							 LPARAM dwData
							 )
{
	MONITORINFOEX mi;
	ZeroMemory(&mi, sizeof(mi));
	mi.cbSize = sizeof(mi);
	GetMonitorInfo(hMonitor, &mi);
	wprintf(L"DisplayDevice: %s\n", mi.szDevice);

	return TRUE;
}
int CDevMgr::EnumDisplays()
{
	printf("\n\n\EnumDisplayDevices\n\n\n");
	DISPLAY_DEVICE dd;
	ZeroMemory(&dd, sizeof(dd));
	dd.cb = sizeof(dd);
	for(int i=0; EnumDisplayDevices(NULL, i, &dd, 0); i++)
	{
		wprintf(L"\n\nDevice %d:", i);
		wprintf(L"\n   1 DeviceName:   '%s'", dd.DeviceName);
		wprintf(L"\n   1 DeviceString: '%s'", dd.DeviceString);
		wprintf(L"\n   1 DeviceID: '%s'", dd.DeviceID);
		/*wprintf(L"\n    DeviceName:   '%s'", dd.DeviceName);
		wprintf(L"\n    DeviceString: '%s'", dd.DeviceString);*/
		//wprintf(L"\n    StateFlags:   %s%s%s%s",
		//	((dd.StateFlags &
		//	DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) ?
		//	L"desktop " : L""),
		//	((dd.StateFlags &
		//	DISPLAY_DEVICE_PRIMARY_DEVICE     ) ?
		//	L"primary " : L""),
		//	((dd.StateFlags & DISPLAY_DEVICE_VGA_COMPATIBLE) ?
		//	L"vga "     : L""),
		//	((dd.StateFlags &
		//	DISPLAY_DEVICE_MULTI_DRIVER       ) ?
		//	L"multi "   : L""),
		//	((dd.StateFlags &
		//	DISPLAY_DEVICE_MIRRORING_DRIVER   ) ?
		//	L"mirror "  : L""));

		// Get more info about the device
		DISPLAY_DEVICE dd2;
		ZeroMemory(&dd2, sizeof(dd2));
		dd2.cb = sizeof(dd2);
		EnumDisplayDevices(dd.DeviceName, 0, &dd2, 0);
		wprintf(L"\n    DeviceID: '%s'", dd2.DeviceID);
		wprintf(L"\n    Monitor Name: '%s'", dd2.DeviceString);
		wprintf(L"\n    DeviceName:   '%s'", dd2.DeviceName);
		wprintf(L"\n    DeviceString: '%s'", dd2.DeviceString);
		wprintf(L"\n    DeviceKey:   '%s'", dd2.DeviceKey);
		
	}
	printf("\n\n\nEnumDisplayMonitors\n\n\n");
	EnumDisplayMonitors(NULL, NULL, MyInfoEnumProc, 0);

	return 0;
}

bool CDevMgr::DeleteAllObjs(CObArray *par)
{
	int ncount = par->GetCount();

	for (int i=0; i< ncount; i++)
	{
		CObject *p = par->GetAt(i);
		delete p;
	}
	par->RemoveAll();
	return true;
}