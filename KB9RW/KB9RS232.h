#pragma once
#include "kb9device.h"

class CKB9RS232 :
	public CKB9Device
{
public:
	CKB9RS232(void);
public:
	~CKB9RS232(void);
public:
	bool DetectKB9();
};
