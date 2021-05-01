#pragma once
#include "kb9device.h"
#include "DeviceDetail.h"

class CKB9USB :
	public CKB9Device
{

public:
	CKB9USB(void);
	~CKB9USB(void);
public:
	bool DetectKB9();
private:
	CDeviceDetail m_kb9USB;
};
