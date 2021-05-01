#include "StdAfx.h"
#include "KB9PS2.h"

CKB9PS2::CKB9PS2(void)
{
	m_nInterface = KB9_INTERFACE::PS2;
}

CKB9PS2::~CKB9PS2(void)
{
}

bool CKB9PS2::DetectKB9()
{
	return false;
}