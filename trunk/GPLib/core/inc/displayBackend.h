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

#include "gpDisplay.h"

class WinAPIDisplay : public GPDisplay
{
public:
  virtual void init ( const GPDisplayParams &params = GPDisplayParams() );

protected:
  friend GPCore;

  GPDisplay( GPCore &_core );
  ~GPDisplay();

  GPCore  &core;
};

#endif _WINAPI_DISPLAY_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
