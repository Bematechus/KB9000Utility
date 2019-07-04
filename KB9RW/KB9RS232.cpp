#include "StdAfx.h"
#include "KB9RS232.h"

CKB9RS232::CKB9RS232(void)
{
	m_nInterface = KB9_INTERFACE::RS232;
}

CKB9RS232::~CKB9RS232(void)
{
}

bool CKB9RS232::DetectKB9()
{
	return false;
}