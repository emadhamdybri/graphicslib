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

      DEVMODE modeInfo;
      modeInfo.dmSize = sizeof(DEVMODE);

      while (EnumDisplaySettings(displayInfo.DeviceName,ENUM_CURRENT_SETTINGS,&modeInfo))
      {
	screen.desktopOffset.x = modeInfo.dmPosition.x;
	screen.desktopOffset.y = modeInfo.dmPosition.y;
	screen.primary = screen.desktopOffset.x ==0 && screen.desktopOffset.y == 0;
      }

      GPPixelSize	enumedRes;

      DWORD displayMode = 0;
      while (EnumDisplaySettings(NULL,displayMode,&modeInfo))
      {
	enumedRes.x = modeInfo.dmPelsWidth;
	enumedRes.y = modeInfo.dmPelsHeight;
	screen.resoultions.push_back(enumedRes);

	displayMode++;
      }
      if (screen.resoultions.size())
	caps->screens.push_back(screen);
      displayID++;
    }
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
