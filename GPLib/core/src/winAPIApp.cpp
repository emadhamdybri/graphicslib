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

#include "gpLib.h"
#include "winAPIApp.h"

WinAPIApp::WinAPIApp()
{
  // get our instance handle
  hInstance = (HINSTANCE)GetModuleHandle(NULL);

  // init the common controlls, we may need them
  INITCOMMONCONTROLSEX InitCtrls;
  InitCtrls.dwICC = ICC_LISTVIEW_CLASSES;
  InitCtrls.dwSize = sizeof(INITCOMMONCONTROLSEX);
  InitCommonControlsEx(&InitCtrls);
}

WinAPIApp::~WinAPIApp()
{
}

bool WinAPIApp::update ( void )
{
  MSG msg;

  if (GetMessage(&msg, NULL, 0, 0)) 
  {
    TranslateMessage(&msg);
    DispatchMessage(&msg);
  }
  else
    return true;

  // we didn't bail, so here check for any events that were passed back to us by the
  // various window procs
  WindowsEventList::iterator itr = pendingWindowsEvents.begin();

  while (itr != pendingWindowsEvents.end())
  {
    // parse the event here
    itr++;
  }

  pendingWindowsEvents.clear();
  return false;
}

void WinAPIApp::winProcCall ( WinAPIDisplay *display, unsigned int message, WPARAM wParam, LPARAM lParam )
{
  WindowsEventRecord  record;
  record.display = display;
  record.message = message;
  record.wParam = wParam;
  record.lParam = lParam;
  pendingWindowsEvents.push_back(record);
}




// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
