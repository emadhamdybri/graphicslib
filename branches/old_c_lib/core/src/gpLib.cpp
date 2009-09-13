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
#include <algorithm>

GPCore::GPCore()
{
  app = NULL;
  gotCaps = false;
  currentDisplay = NULL;
}

GPCore::~GPCore()
{
  if (displays.size())
  {
    for (unsigned int i = 0; i < (unsigned int)displays.size(); i++)
      delete(displays[i]);
  }
}

GPDisplay* GPCore::newDisplay ( void )
{
  // we are out of displays ( usualy if there is a limit, it's 1 )
  if ( (getCapabilities().maxDisplays > 0) && (displays.size() >= (unsigned int)getCapabilities().maxDisplays) )
    return NULL;

  GPDisplay *display = new GPDisplay;
  displays.push_back(display);
  return display;
}

void GPCore::deleteDisplay ( GPDisplay* display )
{
  if (!display)
    return;

  GPDisplayPointerList::iterator itr = std::find(displays.begin(),displays.end(),display);
  if ( itr != displays.end())
    displays.erase(itr);

  delete(display);
}

const GPCaps& GPCore::getCapabilities ( void )
{
  if (gotCaps)
    return caps;
  // run thru each sub systems and have them set the caps they can do
  GPDisplay::getCaps(&caps);
  gotCaps = true;
  return caps;
}

void GPCore::setCurrentContext ( GPDisplay *display )
{
  if(display)
    currentDisplay = display;
}

GPDisplay* GPCore::getCurrentDisplay ( void )
{
  // techincaly we can just make a display here
  // because there always shoudl be one.
  // but the problem is it's not appearnt that we made it
  // and it won't be inited.
  if (!displays.size())
    return NULL;

  if (!currentDisplay)
  {
    // ok so this should NEVER happen, but in case it does
    // we'll go and get the last context we added and make it current
    currentDisplay = displays[displays.size()-1];
  }

  // just in case something wacky with GL happend, go and set this
  // display to be the current one again ( we may want to cache this with the GL pointer
  // for speed later )
  currentDisplay->setCurrent();
  return currentDisplay;
}

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
