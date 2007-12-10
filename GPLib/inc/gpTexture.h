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

#ifndef _GPTEXTURE_H_
#define _GPTEXTURE_H_

#include "gpOpenGL.h"
#include "gpCapabilities.h"

class GPDisplay;
class GPTextureSystem;
class GPTextureProvider;

#define GP_INVALID_TEXTURE_ID 0xFFFFFFFF
typedef unsigned int GPTextureID;

// GPTexturePatamaters can be used to override the texture parameters
// used when a texture is bound to GL
// the default is to do bilinear mip mapping and wrap both UV axes.
// if no function is specified with each texture add then the default is used
class GPTexturePatamaters
{
public:
	virtual ~GPTexturePatamaters(){};

	virtual GLenum getSWrap ( void * param ){return GL_REPEAT;}
	virtual GLenum getTWrap ( void * param ){return getSWrap(param);} 
	virtual GLenum getMagFilter ( void * param ){return GL_LINEAR_MIPMAP_LINEAR;}
	virtual GLenum getMinFilter ( void * param ){return getSWrap(param);}  
	virtual GLenum getPerspeciveHint ( void * param ){return GL_NICEST;}  
};

extern GPTexturePatamaters defaultTextureParams;

class GPTexture
{
public:
	GPTextureID	id;
	GPPixelSize	size;
	unsigned char bitsPerPixel;

	virtual size_t	getDataSize ( void ) = 0;
	virtual char* getData ( void ) = 0;
	virtual void releaseData ( char* data ) = 0;

	void bind ( void );
	void unbind ( void );

protected:
	friend GPTextureProvider;
	GPTexture( GPTexturePatamaters *func, void *param );
	virtual ~GPTexture();

	GPTexturePatamaters *paramsCallback;
	void				*paramParam;
	GLuint boundID;
};

class GPTextureProvider
{
public:
	virtual ~GPTextureProvider(){};

	virtual GPTexture* getTexture ( const char *name, GPTexturePatamaters *func = &defaultTextureParams, void *param = NULL ) = 0;
	virtual GPTexture* getTexture ( const std::string &name, GPTexturePatamaters *func = &defaultTextureParams, void *param = NULL ) = 0;
};

class GPTextureSystem
{
public:

protected:
	friend GPDisplay;

	GPTextureSystem (GPDisplay *d);
	virtual GPTextureSystem();

	//Adds new textures to the system.
	//textures loaded by name will come from the texture provider
	//If an existing texture has been loaded with the same parameters, then the old ID will be returned.
	GPTextureID	addTexture ( const char	*name, GPTexturePatamaters *func = &defaultTextureParams, void *param = NULL );
	GPTextureID	addTexture ( const std::string &name, GPTexturePatamaters *func = &defaultTextureParams, void *param = NULL );
	GPTextureID addTexture ( const char * name, GPTexture* texture, GPTexturePatamaters *func = &defaultTextureParams, void *param = NULL );
	GPTextureID addTexture ( const std::string &name, GPTexture* texture, GPTexturePatamaters *func = &defaultTextureParams, void *param = NULL );

	// aliasing
	void setTextureAlias ( GPTextureID id, const char* alias );
	void setTextureAlias ( GPTextureID id, std::string &alias );
	
	GPTextureID getAliasedID ( const char* alias );
	GPTextureID getAliasedID ( std::string &alias );

	// info method to get the actual texture data ( for info )
	const GPTexture* getTexture ( const char *name );
	const GPTexture* getTexture ( const std::string &name );

	GPTextureID removeTexture ( GPTextureID id );

	void releaseContext ( void );
	void setContext ( void );

	void setProvider ( GPTextureProvider *p );

private:
	GPDisplay *display;
	GPTextureProvider *provier;

	std::map<std::string,GPTextureID> textureAliases;

	typedef struct  
	{	
		GPTextureID	id;
		GPTexture*	texture;
		std::string name;
		std::string alias;
		bool		managed;
		GPTexturePatamaters *func;
		void *param;
	}TexureRecord;

	std::map<GPTextureID,TexureRecord>	textures;

	GPTextureID lastTextureID;

	size_t findTexturesOfName ( const std::string &name, std::vector<TexureRecord*> &records );

	std::map<std::string,GPTextureID> textureAliases;
	GPTextureID findTexturesByAlias( const std::string &alias );
};

#endif //_GPTEXTURE_H_