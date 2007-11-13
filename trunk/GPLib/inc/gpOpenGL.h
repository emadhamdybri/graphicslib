// GPLib
// Copyright (c) 2007 - 2008 Jeff Myers
//
// This package is free software;  you can redistribute it and/or
// modify it under the terms of the license found in the file
// named COPYING that should have accompanied this file.
//
// THIS PACKAGE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//
#ifndef _GL_OPENGL_H_
#define _GL_OPENGL_H_

#ifdef _WIN32 // this file only has windows stuff
  #include <windows.h>
  #include <GL/gl.h>
  #include <GL/glu.h>
#else
  #ifdef __APPLE__
    #include <Carbon/Carbon.h>
    #include <AGL/agl.h>
    #include <AGL/gl.h>
    #include <AGL/glu.h>
  #else	// linux
    #include <GL/gl.h>
    #include <GL/glu.h>
  #endif
#endif // _WIN32

#include <map>
#include <vector>

#define GL_INVALID_ID 0xffffffff

void glSetColor ( float c[3], float alpha = 1.0f );
void glTranslatefv ( float v[3] );

typedef enum
{
  eCenter,
  eLowerLeft,
  eLowerRight,
  eUpperLeft,
  eUpperRight,
  eCenterLeft,
  eCenterRight,
  eCenterTop,
  eCenterBottom
}eAlignment;

void glQuad ( float x, float y, eAlignment align, float scale = 1.0f );
void glLineRing ( float radius, float width = 1 );

class GLColor
{
public:
  GLColor( const float *v );
  GLColor( float c[3], float a );
  GLColor( float r = 1, float g = 1, float b = 1, float a = 1 );

  void setGL ( void );

  bool operator == ( const GLColor & c ) const;
  bool operator != ( const GLColor & c ) const {return !(*this == c);}

  GLColor& operator = ( const GLColor & c);

  void set( const float *v );
  void set( float r = 1, float g = 1, float b = 1, float a = 1 );

  const float *getV ( void ) const { return color;}
  const float R( void ){return color[0];}
  const float G( void ){return color[1];}
  const float B( void ){return color[2];}
  const float A( void ){return color[3];}
protected:
  float color[4];
};

class GLContextInfo
{
public:
  // buffer sizes
  int	frame;
  int	depth;
  int	stencel;
  int	alpha;
  int	aux;
  int	mode;

  // state info
  bool	depthTests;
  bool	ccwWindings;
  bool	flatShaded;

  typedef enum
  {
    noCull = 0,
    backFace,
    frontFace
  }CullingMode;
  CullingMode cullingMode;

  GLContextInfo()
  {
    frame = 0;
    depth = 0;
    stencel = 0;
    alpha = 0;
    aux = 0;
    mode = 0;

    depthTests = true;
    cullingMode = backFace;
    ccwWindings = true;
    flatShaded = false;
  }
};

class GLViewportInfo
{
public:
  int	x;
  int	y;
  float	fov;
  float	aspect;
  float	farZ;
  float	nearZ;

  GLViewportInfo()
  {
    x = -1;
    y = -1;
    fov = -1;
    aspect = 0;
    farZ = 0;
    nearZ = 0;
  }
};

#endif //_GL_OPENGL_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
