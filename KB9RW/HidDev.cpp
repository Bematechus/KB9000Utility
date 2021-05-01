#include "StdAfx.h"
#include "HidDev.h"
extern "C" {
#include <Hidsdi.h>
}
#pragma comment (lib, "hid.lib")//Add win DDK path to project "lib path"

CHidDev::CHidDev(void)
{
	m_hHidDevice = INVALID_HANDLE_VALUE;

}

CHidDev::~CHidDev(void)
{
}
/************************************************************************/
/*                                                                      
	Open hid device
Parameters:
	@strDevicePath:
		The device path comes from enum hid device.
*/
/************************************************************************/
bool CHidDev::Open(CString strDevicePath)
{
	if (m_hHidDevice != INVALID_HANDLE_VALUE)
		if (!Close()) return false;

	m_hHidDevice = INVALID_HANDLE_VALUE;
	HANDLE h;

	h = CreateFile(
		strDevicePath,
		GENERIC_WRITE,
		FILE_SHARE_WRITE | FILE_SHARE_READ,
		NULL,
		OPEN_EXISTING,
		0,
		NULL);

	if (h == INVALID_HANDLE_VALUE)
	{
		return false;
	} 
	else
	{
		m_hHidDevice = h;
		return true;
	}           

}
bool CHidDev::Close()
{

	if (m_hHidDevice != INVALID_HANDLE_VALUE)
	{
		CloseHandle(m_hHidDevice);
		m_hHidDevice = INVALID_HANDLE_VALUE;
	}
	return true;
}

bool CHidDev::IsOpened()
{
	return (m_hHidDevice != INVALID_HANDLE_VALUE);
}

CString CHidDev::GetManufacturerString()
{
	TCHAR buffer[MAX_PATH];
	memset(buffer, 0, MAX_PATH * sizeof(TCHAR));

	if (!HidD_GetManufacturerString(m_hHidDevice,buffer,sizeof(buffer)))
		return false;
	CString s = buffer;
	return s;
	
}
CString CHidDev::GetProductString()
{
	TCHAR buffer[MAX_PATH];
	memset(buffer, 0, MAX_PATH * sizeof(TCHAR));

	if (!HidD_GetProductString(m_hHidDevice,buffer,sizeof(buffer)))
		return false;
	CString s = buffer;
	return s;
}
bool CHidDev::GetVidPidVer(USHORT *vid, USHORT *pid, USHORT *ver)
{
	HIDD_ATTRIBUTES Hid_Attributes;
	if (!HidD_GetAttributes(m_hHidDevice,&Hid_Attributes))
	{
		return false;
	}
	*vid = Hid_Attributes.VendorID;
	*pid = Hid_Attributes.ProductID;
	*ver = Hid_Attributes.VersionNumber;
	return true;

}

int CHidDev::ReadBlockData(BYTE nPackageNo, BYTE *data, int nDataBytes)
{
	BYTE Buffer[9], cData[72], nLRC = 0;
	int nIndex = 0, i, j;
	//{
	//	char text[100];
	//	CString string = "";
	//	wsprintf(text, "GetHIDBlockData:  nPackageNo = %d\n", nPackageNo);
	//	string += text;
	//	//AfxMessageBox(string);
	//}

	if (!SendCmdAddr(0x0e6, nPackageNo))
		return 0;

	Sleep(200);
	//CheckHIDParam( );
	for (i=0;i<9;i++)
	{
		Buffer[0] = (BYTE)0x00;
		if (HidD_GetFeature(m_hHidDevice, Buffer, sizeof(Buffer)))
		{
			for (j=0;j<8;j++)
				cData[nIndex++] = Buffer[9-j-1];

		}else
			return 0;
	}

	for(i=0;i<33;i++)
	{
		data[i] = ((cData[2*i]&0x0f)<<4)+(cData[2*i+1]&0x0f);
		nLRC ^= data[i];
	}

	if (nLRC != 0)
	{
		return 0;//LRC is error
	}
	/*{
	char text[100];
	CString string = "";
	wsprintf(text, "GetHIDBlockData:  nPackageNo = %d\n", nPackageNo);
	string += text;
	for (int i=0;i<33;i++)
	{
	wsprintf(text, "%2XH, ", m_cData[i]);
	string += text;
	}
	wsprintf(text, "\n");
	string += text;
	AfxMessageBox(string);
	}*/
	return 33;
}
int CHidDev::WriteBlockData(BYTE nPackageNo, BYTE *data, int nDataBytes)
{
	BYTE Buffer[9], cData[72], nLRC = 0;
	int nIndex = 0, i, j;

	if (!SendCmdAddr(0x0e7, nPackageNo))
		return 0;

	for(i=0;i<32;i++)
	{
		cData[2*i] = (data[i] & 0x0f) + 0x0d0;
		cData[2*i+1] = ((data[i] & 0x0f0)>>4) + 0x0d0;
		nLRC ^= data[i];
	}
	cData[2*i] = (nLRC & 0x0f) + 0x0d0;
	cData[2*i+1] = ((nLRC & 0x0f0)>>4) + 0x0d0;

	for (j=0;j<9;j++)
	{
		Buffer[0] = (BYTE)0x00;
		for (i=1;i<9;i++)
			Buffer[9-i] = (BYTE)cData[nIndex++];
		if (!HidD_SetFeature(m_hHidDevice, Buffer, sizeof(Buffer)))
		{
			return 0;
		}
	}
	/*{
	char text[100];
	CString string = "";
	wsprintf(text, "PutHIDBlockData:  nPackageNo = %d\n", nPackageNo);
	string += text;
	for (int i=0;i<32;i++)
	{
	wsprintf(text, "%2XH, ", m_cData[i]);
	string += text;
	}
	wsprintf(text, "\n");
	string += text;
	AfxMessageBox(string);
	}*/

	return nDataBytes;
}
BOOL CHidDev::SendCmdAddr(BYTE nCmd, BYTE nPackageNo)
{
	BYTE Buffer[9];
	long nEEROM_Address = nPackageNo * 0x20;

	memset(Buffer, 0, 9);

	Buffer[1] = nCmd;//command code
	Buffer[2] = (BYTE)nEEROM_Address & 0x00ff;// the address(low)
	Buffer[3] = (BYTE)(nEEROM_Address>>8) & 0x00ff;// the address(high)

	return HidD_SetFeature(m_hHidDevice, Buffer, sizeof(Buffer));

	
}

BOOL CHidDev::GetResponce( )
{
	int i;
	BYTE Buffer[9];

	memset(Buffer, 0, 0);

	if (HidD_GetFeature(m_hHidDevice, Buffer, sizeof(Buffer)))
	{
		/*
		char text[100];
		CString string = "";
		wsprintf(text, "GetHIDResponce: HidD_GetFeature OK\n");
		string += text;
		for (i=0;i<9;i++)
		{
		wsprintf(text, "%2XH, ", Buffer[i]);
		string += text;
		}
		wsprintf(text, "\n");
		string += text;
		AfxMessageBox(string);
		*/
		if (Buffer[1] == 0x0c0)
			return true;
	}else
		return false;

	return true;
}