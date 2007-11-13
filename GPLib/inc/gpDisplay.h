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

#ifndef _GPDISPLAY_H_
#define _GPDISPLAY_H_

#include "gpOpenGL.h"
#include <string>

class GPCore;
class GPCaps;

#define	_GP_ANY_SCREEN -1
#define _GP_ANY_BIT_DEPTH -1
#define _GP_DEFAULT_POSTION -1

class GPDisplayParams
{
public:
  int	height, width;
  int	x, y;

  bool	fullscreen;
  int	screen;
  int	bitsPerPixel;

  bool	resizeable;
  bool	caption;
  bool	systemMenus;

  std::string name;

  GPDisplayParams()
  {
    screen = _GP_ANY_SCREEN;
    fullscreen = false;
    resizeable = false;
    caption = true;
    systemMenus = true;
    width = _GP_DEFAULT_POSTION;
    height = _GP_DEFAULT_POSTION;
    x = _GP_DEFAULT_POSTION;
    y = _GP_DEFAULT_POSTION;
    bitsPerPixel = _GP_ANY_BIT_DEPTH;
  }
};

class GPDisplayBase;

class GPDisplay
{
public:
  virtual void init ( const GPDisplayParams &params = GPDisplayParams() );

  // fill out the caps
  static void getCaps ( GPCaps *caps );

  void setQuitOnClose ( bool quit );

protected:
  friend GPCore;

  GPDisplay( );
  virtual ~GPDisplay();

public:
  // events called by the base back into the display class
  virtual void closed ( void );
  virtual void resized ( int x, int y );
  virtual void moved ( int x, int y );
  virtual void invalidated ( void );

  // functions called by the display base to do
  // common GL setup that is not platform specific 
  virtual void setupGL ( void );

  GLViewportInfo  viewportInfo;
  GLContextInfo	  contextInfo;

private:
  GPCore  &core;
  GPDisplayBase	*base;

  GLColor	background;

  bool		quitOnClose;
};

#endif //_GPLIB_H_


// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
