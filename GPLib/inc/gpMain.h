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

#ifndef _GPMAIN_H_
#define _GPMAIN_H_

#include "gpLib.h"
#include "gpApp.h"

#ifdef _WIN32
#include <windows.h>
int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
{
  GPApp *app = GPCore::instance().getApp();
  // app->setCommandline(lpCmdLine);

#else
int main ( int argc, char *argv[] )
{
  GPApp *app = GPCore::instance().getApp();
  // app->setCommandline(argc,argv);
#endif
  
  if (!app)
    return -1;
  app->init();
  app->run();
  return 0;
}

#endif //_GPMAIN_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
