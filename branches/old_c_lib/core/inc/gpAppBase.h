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

#ifndef _GP_APP_BASE_H_
#define _GP_APP_BASE_H_

class GPApp;

/* GPAppBase base is the core class that
    each application (event) platform derives from.
    It only does the OS specific parts of
    events setup, and picks the input classes.
*/

class GPAppBase 
{
public:
  virtual bool update ( void ) = 0;

protected:
  friend GPApp;

  virtual ~GPAppBase(){};
};

#endif //_GP_APP_BASE_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
