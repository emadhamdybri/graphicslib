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

#ifndef _WINAPI_DISPLAY_H_
#define _WINAPI_DISPLAY_H_

#include "gpDisplayBase.h"
#include "winAPIApp.h"
#include "gpOpenGL.h"

class WinAPIDisplay : public GPDisplayBase
{
public:
  virtual void init ( const GPDisplayParams &params );

  static void getCaps ( GPCaps *caps );

  virtual void setCurrent ( void );
  virtual void resize ( int x, int y );
  virtual void beginGL ( void );
  virtual void displayGL ( void );

  LRESULT wndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
protected:
  friend GPDisplay;
 
  WinAPIDisplay();
  virtual~WinAPIDisplay();

private:
  HWND	    hwnd;
  HDC	    hdc;
  HGLRC	    hglrc;
  HPALETTE  hpalette;
  RECT	    rClientRect;

  void registerWindowClass ( void );
  void setScreenResolution ( const GPDisplayParams &params );

  void createWindow ( void );

  // core GL setup code
  void setupPalette ( void );
  void setupPixelFormat ( void );
  void redoPalette ( void );
  void queryNewPallet ( void );
  void pallChanged ( void );



  WinAPIApp *app;
};

#endif _WINAPI_DISPLAY_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
