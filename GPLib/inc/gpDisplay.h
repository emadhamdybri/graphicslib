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

#include <string>

class GPCore;
class GPCaps;

#define  _GP_ANY_SCREEN -1

class GPDisplayParams
{
public:
  bool	fullscreen;
  bool	resizeable;
  bool	noBorder;
  int	height, width;
  int	screen;

  std::string name;

  GPDisplayParams()
  {
    screen = _GP_ANY_SCREEN;
    fullscreen = false;
    resizeable = false;
    noBorder = false;
    width = 1024;
    height = 768;
  }
};

class GPDisplayBase;

class GPDisplay
{
public:
  virtual void init ( const GPDisplayParams &params = GPDisplayParams() );

  // fill out the caps
  static void getCaps ( GPCaps *caps );

protected:
  friend GPCore;

  GPDisplay( );
  virtual ~GPDisplay();

private:
  GPCore  &core;
  GPDisplayBase	*base;
};

#endif //_GPLIB_H_


// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
