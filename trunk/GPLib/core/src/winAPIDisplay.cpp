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

LRESULT CALLBACK _WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
  WinAPIDisplay* display = (WinAPIDisplay*)GetWindowLong(hWnd,GWLP_USERDATA);

  if (message == WM_CREATE) 
    display = (WinAPIDisplay*)(((CREATESTRUCT*)lParam)->lpCreateParams);

  if (display)
    return display->wndProc(hWnd, message, wParam, lParam);

  return DefWindowProc(hWnd, message, wParam, lParam);
}

static bool gWinClassRegistered = false;
static TCHAR gWindowClass[512];


#define _WIN_CLASS_NAME "gpWinAPIDisplayClass"

void WinAPIDisplay::getCaps ( GPCaps *caps )
{
  if (caps)
  {
    caps->canWindow = true;
    caps->maxDisplays = _GP_NO_DISPLAY_LIMIT;

    caps->screens.clear();

    DWORD displayID = 0;
    DISPLAY_DEVICE  displayInfo;
    displayInfo.cb = sizeof(DISPLAY_DEVICE);

    while ( EnumDisplayDevices(NULL,displayID,&displayInfo,0) )
    {
      GPScreenDescriptor	screen;
      screen.osParam[0] = displayID;

      DEVMODE modeInfo;
      modeInfo.dmSize = sizeof(DEVMODE);

      while (EnumDisplaySettings(displayInfo.DeviceName,ENUM_CURRENT_SETTINGS,&modeInfo))
      {
	screen.desktopOffset.x = modeInfo.dmPosition.x;
	screen.desktopOffset.y = modeInfo.dmPosition.y;
	screen.primary = screen.desktopOffset.x ==0 && screen.desktopOffset.y == 0;
	screen.desktopRes.resolution.x = modeInfo.dmPelsWidth;
	screen.desktopRes.resolution.y = modeInfo.dmPelsHeight;
	screen.desktopRes.bitsPerPixel = modeInfo.dmBitsPerPel;
      }

      GPScreenResolution	enumedRes;

      DWORD displayMode = 0;
      while (EnumDisplaySettings(NULL,displayMode,&modeInfo))
      {
	enumedRes.resolution.x = modeInfo.dmPelsWidth;
	enumedRes.resolution.y = modeInfo.dmPelsHeight;
	enumedRes.bitsPerPixel = modeInfo.dmBitsPerPel;
	screen.resoultions.push_back(enumedRes);

	displayMode++;
      }

      if (screen.resoultions.size())
	caps->screens.push_back(screen);
      displayID++;
    }
  }
}

WinAPIDisplay::WinAPIDisplay() : GPDisplayBase()
{
  hwnd = NULL;
  app = (WinAPIApp*)GPCore::instance().getApp()->base;
  hdc = NULL;
  hglrc = NULL;
  hpalette = NULL;
}

WinAPIDisplay::~WinAPIDisplay()
{
  if (hwnd)
    DestroyWindow(hwnd);
}

void WinAPIDisplay::init ( const GPDisplayParams &params )
{
  if (!app)
    return;

  if (!gWinClassRegistered)
    registerWindowClass();

  DWORD  style = WS_CLIPCHILDREN | WS_CLIPSIBLINGS;
  int	size[4] = {CW_USEDEFAULT,CW_USEDEFAULT,CW_USEDEFAULT,CW_USEDEFAULT};
  if ( params.fullscreen )
  {
    setScreenResolution ( params );
    size[0] = 0;
    size[1] = 0;
    size[2] = params.width;
    size[3] = params.height;
  }
  else
  {
    if ( !params.caption )
      style |= WS_POPUP;
    else
    {
      if( params.systemMenus)
	style |= WS_OVERLAPPEDWINDOW;
      else
	style |= WS_CAPTION;
    }

    if ( params.resizeable )
      style |= WS_THICKFRAME;

    if (params.x > 0)
      size[0] = params.x;
    if (params.y > 0)
      size[1] = params.y;

    if (params.width > 0)
      size[2] = params.width;
    if (params.height > 0)
      size[3] = params.height;
  }

  hwnd = CreateWindow(gWindowClass, params.name.c_str(), style, size[0], size[1], size[2], size[3], NULL, NULL, app->hInstance, (int*)this);

  if (!hwnd)
    return;

  SetWindowLongPtr(hwnd,GWLP_USERDATA,(LONG)this);
  ShowWindow(hwnd, SW_SHOW);
  UpdateWindow(hwnd);
}

void WinAPIDisplay::setCurrent ( void )
{
}

void WinAPIDisplay::resize ( int x, int y )
{
}

void WinAPIDisplay::beginGL ( void )
{
}

void WinAPIDisplay::displayGL ( void )
{

}

void WinAPIDisplay::registerWindowClass ( void )
{
  strncpy(gWindowClass,_WIN_CLASS_NAME,sizeof(_WIN_CLASS_NAME));

  WNDCLASSEX wcex;

  wcex.cbSize	    = sizeof(WNDCLASSEX); 
  wcex.style	    = CS_OWNDC | CS_HREDRAW | CS_VREDRAW;
  wcex.lpfnWndProc  = (WNDPROC)_WndProc;
  wcex.cbClsExtra   = 0;
  wcex.cbWndExtra   = sizeof(WinAPIDisplay*);
  wcex.hInstance    = app->hInstance;
  wcex.hIcon	    = NULL;//LoadIcon(hInstance, (LPCTSTR)IDI_MAPEDIT);
  wcex.hCursor	    = LoadCursor(NULL, IDC_ARROW);
  wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
  wcex.lpszMenuName   = NULL;//(LPCTSTR)IDC_MAPEDIT;
  wcex.lpszClassName  = gWindowClass;
  wcex.hIconSm	      = NULL;//LoadIcon(wcex.hInstance, (LPCTSTR)IDI_SMALL);

  gWinClassRegistered = RegisterClassEx(&wcex) != 0;
}

void WinAPIDisplay::setScreenResolution ( const GPDisplayParams &params )
{
}

void WinAPIDisplay::createWindow ( void )
{
  if (!hwnd)
    return;

  if (hdc)
    ReleaseDC(hwnd,hdc);

  hdc = GetDC(hwnd);
  if (!hdc)
    return;

}


LRESULT WinAPIDisplay::wndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
  // check for anything like a resize here and do what ya gotta do
  switch(message)
  {
    case WM_CREATE:
      createWindow();
      return 0;

    case WM_CLOSE:
      display->closed();
      return 0;
  }
  app->winProcCall(display,message,wParam,lParam);
  return DefWindowProc(hWnd, message, wParam, lParam);
}


// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
