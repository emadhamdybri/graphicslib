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
#include "winAPIDisplay.h"
#include <windows.h>

// this gets called once for each display, fill out a screen record for each one

BOOL CALLBACK MonitorEnumProc( HMONITOR hMonitor, HDC hdcMonitor, LPRECT lprcMonitor, LPARAM dwData );
{
  GPScreenDescriptor	screen;
  GPScreenDescriptorList *screenList = (GPScreenDescriptorList*)dwData;
  if (!screenList)
    return FALSE

  MONITORINFOEX	monitorInfo;
  monitorInfo.cbSize = sizeof(MONITORINFOEX);
  GetMonitorInfo(hMonitor,&monitorInfo);

  GPScreenResolution	desktopRes;
  desktopRes.height = monitorInfo.rcMonitor.bottom-monitorInfo.rcMonitor.top;
  desktopRes.width = monitorInfo.rcMonitor.right-monitorInfo.rcMonitor.left;
  desktopRes.originX = monitorInfo.rcMonitor.left;
  desktopRes.originY = height-monitorInfo.rcMonitor.bottom;
  
  if ( monitorInfo.dwFlags |= MONITORINFOF_PRIMARY)
  {
  }

  return TRUE:

}

void WinAPIDisplay::getCaps ( GPCaps *caps )
{
  if (caps)
  {
    caps->canFullScreen = true;
    caps->maxDisplays = -1;
    caps->maxRes[0] = caps->maxRes[1] = 1;

    caps->screens.clear();

    DWORD displayID = 0;
    DISPLAY_DEVICE  displayInfo;
    displayInfo.cb = sizeof(DISPLAY_DEVICE);

    while ( EnumDisplayDevices(NULL,displayID,&displayInfo,0) )
    {
      GPScreenDescriptor	screen;
      screen.osParam = (void*)displayID;

      DWORD displayMode = 0;
      DEVMODE modeInfo;
      while (EnumDisplaySettings(NULL,displayMode,&modeInfo))
      {
	GPScreenResolution	desktopRes;

	r.width = modeInfo.dmPelsWidth;
	r.height = modeInfo.dmPelsHeight;
	r.refresh = modeInfo.dmDisplayFrequency;
	r.depth = modeInfo.dmBitsPerPel;

	displayMode++;
      }

      if (screen.resoultions.size())
	caps->screens.push_back(screen);
      displayID++;
    }

    EnumDisplayMonitors(NULL,NULL,MonitorEnumProc,(void*)&caps->screens);

  }
}


WinAPIDisplay::WinAPIDisplay()
{
}

WinAPIDisplay::~WinAPIDisplay()
{
}

void WinAPIDisplay::init ( const GPDisplayParams &params )
{
}


// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
