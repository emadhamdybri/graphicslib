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

#ifndef _WINAPI_APP_H_
#define _WINAPI_APP_H_

#include "gpAPPBase.h"

#define _WIN32_WINNT 0x0500
#include <windows.h>
#include <commctrl.h>
#include <commoncontrols.h>

#include <list>

class GPDisplay;

class WinAPIApp : public GPAppBase
{
public:
  WinAPIApp();
  virtual ~WinAPIApp();

  virtual bool update ( void );

  void winProcCall ( GPDisplay *display, unsigned int message, WPARAM wParam, LPARAM lParam );

  HINSTANCE hInstance;
protected:

private:
  GPApp			    *app;

  typedef struct
  {
    GPDisplay		     *display;
    unsigned int	      message;
    WPARAM		      wParam;
    LPARAM		      lParam;
  }WindowsEventRecord;

  typedef std::list<WindowsEventRecord> WindowsEventList;

  WindowsEventList pendingWindowsEvents;
};

#endif _WINAPI_DISPLAY_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
