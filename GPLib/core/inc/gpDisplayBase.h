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

#ifndef _GP_DISPLAY_BASE_H_
#define _GP_DISPLAY_BASE_H_

#include "gpDisplay.h"
#include "gpCapabilities.h"

/* GPDisplay base is the core class that
    each display platform derives from.
    It only does the OS specific parts of
    window setup and GL management.

  *NOTE*
   Your derived class must be a friend to 
   GPdisplay so it can construct it.
*/

class GPDisplayBase 
{
public:
  virtual void init ( const GPDisplayParams &params ) = 0;

  static void getCaps ( GPCaps *caps ){};

protected:
  friend GPDisplay;

  GPDisplay *display;

  GPDisplayBase(){display = NULL;}
  virtual ~GPDisplayBase(){};
};

#endif //_GP_DISPLAY_BASE_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
