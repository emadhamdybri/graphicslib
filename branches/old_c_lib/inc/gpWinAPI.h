/* GPLib
* Copyright (c) 2007 - 2008 Jeffrey Myers
*
* This package is free software;  you can redistribute it and/or
* modify it under the terms of the license found in the file
* named COPYING.txt that should have accompanied this file.
*
* THIS PACKAGE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR
* IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
*/

#ifndef _GPWINAPI_H_
#define _GPWINAPI_H_

// common set of windows includes
// consolidated so they arn't spread all over the lib
#ifdef _WIN32

#define WIN32_LEAN_AND_MEAN
#define _WIN32_WINNT 0x0500
#include <Windows.h>

#include <shlobj.h>

#include <commctrl.h>
#include <commoncontrols.h>

#include <io.h>
#include <direct.h>

// utility functions to help windows
// apps link in the libs they need
#ifndef _SUPRESS_AUTO_GP_LIB
  #pragma comment(lib, "comctl32.lib")
  #pragma comment(lib, "GPCore.lib")
#endif

#ifndef _SUPRESS_AUTO_GL_LIB
  #pragma comment(lib, "opengl32.lib")
  #pragma comment(lib, "glu32.lib")
#endif

#endif

#endif //_GPFILE_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
