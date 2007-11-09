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

#include "gpDisplay.h"
#include "gpLib.h"
#include "gpDisplayBase.h"

#ifdef _WIN32
#include "winAPIDisplay.h"
#else

#endif

void GPDisplay::getCaps ( GPCaps *caps )
{
#ifdef _WIN32
  WinAPIDisplay::getCaps(caps);
#endif
}

GPDisplay::GPDisplay( void ) : core(GPCore::instance())
{
  base = NULL;
// the big long place where we pick the backends

#ifdef _WIN32
  base = new WinAPIDisplay;
#endif //_WIN32

  base->display = this;
}

GPDisplay::~GPDisplay()
{
  if (base)
    delete(base);
}

void GPDisplay::init ( const GPDisplayParams &params )
{
  if (!base)
    return;

  base->init(params);
}

void GPDisplay::setQuitOnClose ( bool quit )
{
  quitOnClose = quit;
}


void GPDisplay::closed ( void )
{
  delete(base);
  base = NULL;

  if (quitOnClose)
    core.getApp()->quit();
}

void GPDisplay::resized ( int x, int y )
{

}

void GPDisplay::moved ( int x, int y )
{

}

void GPDisplay::invalidated ( void )
{
}



// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
