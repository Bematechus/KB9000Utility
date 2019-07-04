#pragma once
#include "afx.h"


typedef enum _KB9_INTERFACE
{
	Unknown = 0,
	PS2, 
	USB, 
	RS232
}KB9_INTERFACE;

class CKB9Device :
	public CObject
{
public:
	CKB9Device(void);
	~CKB9Device(void);
public:
	virtual int Read(){return 0;};
	virtual int Write(){return 0;};
	int InterfaceType(){return m_nInterface;};
	virtual bool DetectKB9(){return false;}
protected:
	KB9_INTERFACE m_nInterface;

};
