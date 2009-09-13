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

#include "gpTexture.h"

void GPTexture::bind ( void )
{
  if (boundID == GP_INVALID_TEXTURE_ID)
  {
    char* d = getData();
    size_t s = getDataSize();

    glGenTextures(1,(GLuint*)&boundID );
    glBindTexture(GL_TEXTURE_2D,boundID );

    GLenum	eFormat = 0;

    switch(bitsPerPixel)
    {
    case 1:
      eFormat = GL_LUMINANCE;
      break;

    case 2:
      eFormat = GL_LUMINANCE_ALPHA;
      break;

    case 3:
      eFormat = GL_RGB;
      break;

    case 4:
    default:
      eFormat = GL_RGBA;
      break;
    }

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, paramsCallback->getSWrap(paramParam));
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, paramsCallback->getTWrap(paramParam));
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, paramsCallback->getMagFilter(paramParam));
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, paramsCallback->getMinFilter(paramParam));
    glHint(GL_PERSPECTIVE_CORRECTION_HINT, paramsCallback->getPerspeciveHint(paramParam)); 

    gluBuild2DMipmaps(GL_TEXTURE_2D,bitsPerPixel,size.x,size.y,eFormat,GL_UNSIGNED_BYTE,d);

    releaseData(d);
  }
}

void GPTexture::unbind ( void )
{
  if (boundID != GP_INVALID_TEXTURE_ID)
    glDeleteTextures(1,&boundID);
}

GPTexture::GPTexture( GPTexturePatamaters *func , void *param )
{
  boundID = GP_INVALID_TEXTURE_ID;
  bitsPerPixel = 0;

  paramsCallback = func;
  paramParam  = param;
}

GPTexture::~GPTexture()
{
  unbind();
}

class GPDevilTextureProvider : public GPTextureProvider
{
public:
  virtual ~GPDevilTextureProvider();

  virtual GPTexture* getTexture ( const char *name );
  virtual GPTexture* getTexture ( const std::string &name );
};

class GPTextureSystem
{
public:

protected:
  friend GPDisplay;

  GPTextureSystem (GPDisplay *d);
  virtual GPTextureSystem();

  GPTextureID	getID ( const char	*name );
  GPTextureID	getID ( const std::string &name );

  GPTexture* getTexture ( const char	*name );
  GPTexture* getTexture ( const std::string &name );

  GPTextureID addTexture ( const char * name, GPTexture* texture );
  GPTextureID addTexture ( const std::string &name, GPTexture* texture );

  GPTextureID removeTexture ( GPTextureID id );

  void releaseContext ( void );
  void setContext ( void );

  void setProvider ( GPTextureProvider *p );
private:
  GPDisplay *display;
  GPTextureProvider *provier;

  // GPTextureProvider
  // textures that are managed by the provider
  std::map<GPTextureID,GPTexture*>	loadedTextures;
  std::map<std::string,GPTextureID>	loadedTextureNames;

  // textures that are passed in.
  // they are unmanaged other then context rebinds needed
  std::map<GPTextureID,GPTexture*>	customTextures;
  std::map<std::string,GPTextureID>	customTextureNames;
};

#endif //_GPTEXTURE_H_


// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
