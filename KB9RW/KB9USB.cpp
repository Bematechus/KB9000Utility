#include "StdAfx.h"
#include "KB9USB.h"
#include "DevMgr.h"
#include "DeviceDetail.h"
#include "HidDev.h"
#define KB9_USB_VID 0x0000
#define KB9_USB_PID 0x9999

#define KB9_MANUFACTURE _T("Logic Controls")
#define KB9_PRODUCT _T("KB9000")

//class GUID
DEFINE_GUID( GUID_DEVCLASS_HIDCLASS, 
			0x745A17A0, 0x74D3, 0x11D0, 0xB6, 0xFE, 0x00, 0xA0, 0xC9, 0x0F, 0x57, 0xDA );
DEFINE_GUID( GUID_DEVCLASS_KEYBOARD, 
			0x4D36E96B, 0xE325, 0x11CE, 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18 );
DEFINE_GUID( GUID_DEVCLASS_MOUSE,  
			0x4D36E96F, 0xE325, 0x11CE, 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18 );

//interface GUID
DEFINE_GUID(GUID_DEVINTERFACE_HID, 
			0x4D1E55B2, 0xF16F, 0x11CF, 0x88, 0xCB, 0x00, 0x11, 0x11, 0x00, 0x00, 0x30);
DEFINE_GUID(GUID_DEVINTERFACE_KEYBOARD, 
			0x884B96C3, 0x56EF, 0x11D1, 0xBC, 0x8C, 0x00, 0xA0, 0xC9, 0x14, 0x05, 0xDD);
DEFINE_GUID(GUID_DEVINTERFACE_MOUSE, 
			0x378DE44C, 0x56EF, 0x11D1, 0xBC, 0x8C, 0x00, 0xA0, 0xC9, 0x14, 0x05, 0xDD);



CKB9USB::CKB9USB(void)
{
	m_nInterface = KB9_INTERFACE::USB;
}

CKB9USB::~CKB9USB(void)
{
}

bool CKB9USB::DetectKB9()
{
	CDevMgr devMgr;
	CObArray ar;
	m_kb9USB.reset();
	int ncount = devMgr.EnumDevicesClass(GUID_DEVCLASS_HIDCLASS,&ar );
	CHidDev hidDev;

	for (int i=0; i< ncount; i++)
	{
		CDeviceDetail* dev = (CDeviceDetail*) ar.GetAt(i);
		hidDev.Close();
		if (!hidDev.Open(dev->m_devicePath))
			continue;
		CString manufacture = hidDev.GetManufacturerString();
		CString product = hidDev.GetProductString();
		hidDev.Close();
		if (manufacture == KB9_MANUFACTURE &&
			product == KB9_PRODUCT)
		{
			dev->CopyTo(&m_kb9USB);
			return true;
		}

	}
	return false;
}