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

#ifndef _GPLIB_H_
#define _GPLIB_H_

#include "gpDisplay.h"
#include "gpCapabilities.h"
#include "gpApp.h"

// utility functions to help windows
// apps link in the libs they need
#ifdef _WIN32
#pragma comment(lib, "comctl32.lib")
#pragma comment(lib, "GPCore.lib")
#endif

class GPCore
{
public:
  static GPCore& instance ( void )
  {
    static GPCore core;
    return core;
  }

  GPDisplay* newDisplay ( void );

  // retuns a GPCaps filled out with the current capabilities
  const GPCaps& getCapabilities ( void );

  void setApp ( GPApp *_app ){app = _app;}
  GPApp* getApp ( void ) { return app; }

protected:
  GPCore();
  ~GPCore();

private:
  GPCaps  caps;
  GPApp	  *app;
};

#endif //_GPLIB_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
