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

#include "gpApp.h"
#include "gpLib.h"
#include "gpAppBase.h"
#include "gpAppEvents.h"

#ifdef _WIN32
#include "winAPIApp.h"
#else
#endif

GPApp::GPApp()
{
  base = NULL;
  GPCore::instance().setApp(this);
}

GPApp::~GPApp()
{
  if (base)
    delete(base);
}

void GPApp::init ( void )
{
  if (base)
    delete(base);

#ifdef _WIN32
  base = new WinAPIApp;
#else
  base = NULL;
#endif

  setup();
}

void GPApp::run ( void )
{
  done = false;
  while (!done)
  {
    if (!base)
      done = true;
    else
    {
      // let the platform update itself
      // and call any events
      if (base->update())
	done = true;

      // if we are going to keep going then check for any pump events from other systems
      if (!done)
      {
	  // do other events here (joysitck polls, etc)
      }

      if (!done)    // if we aren't going to quit yet, then call the frame
	doFrame();
    }
  }
}

// kill the main loop
// and fire off the event that we are quiting.
void GPApp::quit ( void )
{
  done = true;
  event(GPAppQuitEvent());
}

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
