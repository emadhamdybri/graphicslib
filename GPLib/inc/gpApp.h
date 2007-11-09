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

#ifndef _GPAPP_H_
#define _GPAPP_H_

class GPAppBase;

typedef enum
{
  eNullEvent = 0,
  eAppQuitEvent,
  eLastEvent
}EventTYpes;

class GPApp
{
public:
  GPApp();
  ~GPApp();

  void init ( void );
  void run ( void );
  void quit ( void );

  virtual void setup ( void ) = 0;
  virtual void handleEvent ( void ) = 0;
  virtual void doFrame ( void ) = 0;

  GPAppBase *base;
protected:

private:
  bool done;
};

#endif //_GPAPP_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
