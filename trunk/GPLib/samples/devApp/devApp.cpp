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

// devApp.cpp : Defines the entry point for the application.
//

// we will use the GP main, so all we have to do is create an app and it will automaticly run.
// if we don't want to have the lib use a main, then we don't include this file, and go and run the app manualy
#include "gpMain.h"
#include "devApp.h"

//Just contstructing the app gets the ball rolling with gpMain
MyApp app;

MyApp::MyApp() : GPApp()
{
}

// first method called wti
void MyApp::setup ( void )
{
  display = GPCore::instance().newDisplay();
  display->setQuitOnClose(true);
  display->init();
}

void MyApp::doFrame ( void )
{
}



// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8