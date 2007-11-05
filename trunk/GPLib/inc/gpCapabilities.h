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

#ifndef _GPCAPS_H_
#define _GPCAPS_H_

class GPCore;
#include <list>

typedef struct 
{
  int height, width, originX, originY;
}GPScreenResolution;

typedef std::list<GPScreenResolution> GPScreenResolutionList;

typedef struct GPScreenDescriptor
{
  GPScreenResolutionList    resoultions;
  bool			    primary;
  void*			    osParam;

  GPScreenDescriptor()
  {
    osParam = NULL;
    primary = false;
  }
}GPScreenDescriptor;

typedef std::list<GPScreenDescriptor> GPScreenDescriptorList;

class GPCaps
{
public:

  // display related caps
  int	  maxDisplays;
  bool	  canFullScreen;
  int	  maxRes[2];

  GPScreenDescriptorList	screens;

protected:
  friend GPCore;

  GPCaps(){};
};

#endif //_GPCAPS_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
