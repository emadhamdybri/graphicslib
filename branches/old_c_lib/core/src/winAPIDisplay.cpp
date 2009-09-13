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
  WinAPIDisplay* display = (WinAPIDisplay*)GetWindowLongPtr(hWnd,GWLP_USERDATA);

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

      if (EnumDisplaySettings(displayInfo.DeviceName,ENUM_CURRENT_SETTINGS,&modeInfo))
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
  wglMakeCurrent(hdc, hglrc);
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

  GetClientRect(hwnd,&rClientRect);

  display->viewportInfo.x = rClientRect.right-rClientRect.left;
  display->viewportInfo.y = rClientRect.top-rClientRect.bottom;

  setupPixelFormat();
  setupPalette();
  hglrc = wglCreateContext (hdc);
  wglMakeCurrent(hdc, hglrc);
  display->setupGL();
}

LRESULT WinAPIDisplay::wndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
  // check for anything like a resize here and do what ya gotta do
  switch(message)
  {
    case WM_CREATE:
      createWindow();
      return 0;

    case WM_ERASEBKGND:
      return 1;

    case WM_PALETTECHANGED:
      pallChanged();
      return 0;

    case WM_QUERYNEWPALETTE:
      queryNewPallet();
       return 0;

    case WM_CLOSE:
      display->closed();
      return 0;
  }
  app->winProcCall(display,message,wParam,lParam);
  return DefWindowProc(hWnd, message, wParam, lParam);
}

// basic GL setup stuf

void WinAPIDisplay::queryNewPallet ( void )
{
  if (hglrc && hpalette) 
    redoPalette ();
}

void WinAPIDisplay::pallChanged ( void )
{
  redoPalette ();
}

void WinAPIDisplay::redoPalette ( void )
{
  UnrealizeObject (hpalette);
  SelectPalette (hdc, hpalette, false);
  RealizePalette (hdc);
}

void WinAPIDisplay::setupPalette ( void )
{
  int pixelFormat = GetPixelFormat(hdc);
  PIXELFORMATDESCRIPTOR pfd;
  LOGPALETTE* pPal;
  int paletteSize;

  DescribePixelFormat(hdc, pixelFormat, sizeof(PIXELFORMATDESCRIPTOR), &pfd);

  if (pfd.dwFlags & PFD_NEED_PALETTE)
    paletteSize = 1 << pfd.cColorBits;
  else 
    return;

  pPal = (LOGPALETTE*)malloc(sizeof(LOGPALETTE) + paletteSize * sizeof(PALETTEENTRY));
  pPal->palVersion = 0x300;
  pPal->palNumEntries = paletteSize;

  // build a simple RGB color palette 
  {
    int redMask = (1 << pfd.cRedBits) - 1;
    int greenMask = (1 << pfd.cGreenBits) - 1;
    int blueMask = (1 << pfd.cBlueBits) - 1;
    int i;

    for (i=0; i<paletteSize; ++i)
    {
      pPal->palPalEntry[i].peRed =
	(((i >> pfd.cRedShift) & redMask) * 255) / redMask;
      pPal->palPalEntry[i].peGreen =
	(((i >> pfd.cGreenShift) & greenMask) * 255) / greenMask;
      pPal->palPalEntry[i].peBlue =
	(((i >> pfd.cBlueShift) & blueMask) * 255) / blueMask;
      pPal->palPalEntry[i].peFlags = 0;
    }
  }

  hpalette = (HPALETTE)CreatePalette(pPal);
  free(pPal);

  if (hpalette)
  {
    SelectPalette(hdc, hpalette, false);
    RealizePalette(hdc);
  }
}

void WinAPIDisplay::setupPixelFormat ( void )
{
  HANDLE hHeap;
  int nColors, i;
  LPLOGPALETTE lpPalette;
  BYTE byRedMask, byGreenMask, byBlueMask;

  static PIXELFORMATDESCRIPTOR pfd = 
  {
    sizeof (PIXELFORMATDESCRIPTOR),             // Size of this structure
      1,                                          // Version number
      PFD_DRAW_TO_WINDOW |                        // Flags
      PFD_SUPPORT_OPENGL |
      PFD_DOUBLEBUFFER ,
      display->contextInfo.mode,                              // RGBA pixel values
      display->contextInfo.frame,                             // 24-bit color
      0, 0, 0, 0, 0, 0,                           // Don't care about these
      display->contextInfo.alpha, 0,                          // No alpha buffer
      0, 0, 0, 0, 0,                              // No accumulation buffer
      display->contextInfo.depth,                             // 32-bit depth buffer
      display->contextInfo.stencel,                           // No stencil buffer
      display->contextInfo.aux,                               // No auxiliary buffers
      PFD_MAIN_PLANE,                             // Layer type
      0,                                          // Reserved (must be 0)
      0, 0, 0                                     // No layer masks
  };

  int nPixelFormat;

  nPixelFormat = ChoosePixelFormat (hdc, &pfd);
  SetPixelFormat (hdc, nPixelFormat, &pfd);

  DescribePixelFormat (hdc, nPixelFormat, sizeof (PIXELFORMATDESCRIPTOR),&pfd);

  display->contextInfo.mode = pfd.iPixelType;
  display->contextInfo.depth = pfd.cDepthBits;
  display->contextInfo.depth = pfd.cColorBits;
  display->contextInfo.stencel = pfd.cStencilBits;
  display->contextInfo.alpha = pfd.cAlphaBits;
  display->contextInfo.aux = pfd.cAuxBuffers; 

  if (pfd.dwFlags & PFD_NEED_PALETTE)
  {
    nColors = 1 << pfd.cColorBits;
    hHeap = GetProcessHeap ();

    (LPLOGPALETTE) lpPalette = (LPLOGPALETTE)HeapAlloc (hHeap, 0,
      sizeof (LOGPALETTE) + (nColors * sizeof (PALETTEENTRY)));

    lpPalette->palVersion = 0x300;
    lpPalette->palNumEntries = nColors;

    byRedMask = (1 << pfd.cRedBits) - 1;
    byGreenMask = (1 << pfd.cGreenBits) - 1;
    byBlueMask = (1 << pfd.cBlueBits) - 1;

    for (i=0; i<nColors; i++)
    {
      lpPalette->palPalEntry[i].peRed = (((i >> pfd.cRedShift) & byRedMask) * 255) / byRedMask;
      lpPalette->palPalEntry[i].peGreen = (((i >> pfd.cGreenShift) & byGreenMask) * 255) / byGreenMask;
      lpPalette->palPalEntry[i].peBlue = (((i >> pfd.cBlueShift) & byBlueMask) * 255) / byBlueMask;
      lpPalette->palPalEntry[i].peFlags = 0;
    }

    hpalette = CreatePalette (lpPalette);
    HeapFree (hHeap, 0, lpPalette);

    if (hpalette != NULL)
    {
      SelectPalette (hdc, hpalette, FALSE);
      RealizePalette (hdc);
    }
  }
}


// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
