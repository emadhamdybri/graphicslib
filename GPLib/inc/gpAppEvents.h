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

#ifndef _GPAPP_EVENTS_H_
#define _GPAPP_EVENTS_H_

// temp defines for pointers we'll need
// every event falls into this structure
typedef enum
{
  eNullEvent = 0,
  eAppQuitEvent,
  eLastEvent
}GPAppEventType;

class GPAppEvent
{
public:
  GPAppEventType event;
  virtual ~GPAppEvent(){};
};

class GPAppQuitEvent : public GPAppEvent
{
public:
  GPAppQuitEvent()
  {
    event = eAppQuitEvent;
  }
  virtual ~GPAppQuitEvent(){};
};


#endif //_GPAPP_EVENTS_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
