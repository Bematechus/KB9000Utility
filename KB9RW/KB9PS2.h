#pragma once
#include "kb9device.h"

class CKB9PS2 :
	public CKB9Device
{
public:
	CKB9PS2(void);
	~CKB9PS2(void);
	bool DetectKB9();
public:

};
