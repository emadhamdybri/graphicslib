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

// devApp.h : Defines the entry point for the application.
//

#ifndef _DEV_APP
#define _DEV_APP

#include "gpLib.h"
#include "gpApp.h"

class MyApp : public GPApp
{
public:
  MyApp();

  virtual void setup ( void );
  virtual void handleEvent ( void );
  virtual void doFrame ( void );
};

#endif //_DEV_APP

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8