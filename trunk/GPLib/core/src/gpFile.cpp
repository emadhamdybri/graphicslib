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

#include "gpFile.h"

class GPDiskFileSystemProvider;

class GPDiskFile : public GPFile
{
public:
  GPFileSystemProvider *provider;

protected:
  friend GPDiskFileSystemProvider;
  GPDiskFile( GPFileSystemProvider* p, const char *path );

  virtual ~GPDiskFile();
};

class GPDiskFileSystemProvider : public GPFileSystemProvider
{
public:
  virtual ~GPDiskFileSystemProvider();

  virtual GPFile* getFile ( const char* path, FileStatus &status );
  virtual GPFile* createFile ( const char* path, FileStatus &status );

  virtual bool closeFile ( GPFile* file );

  virtual std::string getName ( void ){return std::string("DiskFileSystem"};}
  
  virtual void closeAllFiles ( void );

protected:
  typedef std::list<GPDiskFile*> OpenDiskFileMap;
};

//-------------------------GPFileSystem-----------------------//
GPFile* GPFileSystem::getFile ( const char* path, const char* fileSystem )
{
    GPFileSystemProvider::FileStatus status;
    GPFileSystemProvider *provider = findProvider(fileSystem);

    if (provider)
      return provider->getFile(path,status);

    // they didn't specify so try them in order.
    for (int i = 0; i < fileSystemList.size(); i++)
    {
      GPFile *file = fileSystemList[i]->getFile(path,status);
      if (status != GPFileSystemProvider::ePathNotHandled)
	return (status == GPFileSystemProvider::eFileLoaded) ? file : NULL;
    }

    return NULL;
}

GPFile* GPFileSystem::createFile ( const char* path,  const char* fileSystem )
{
  GPFileSystemProvider::FileStatus status;
  GPFileSystemProvider *provider = findProvider(fileSystem);

  if (provider)
    return provider->createFile(path,status);

  // they didn't specify so try them in order.
  for (int i = 0; i < fileSystemList.size(); i++)
  {
    GPFile *file = fileSystemList[i]->createFile(path,status);
    if (status != GPFileSystemProvider::ePathNotHandled)
      return (status == GPFileSystemProvider::eFileLoaded) ? file : NULL;
  }

  return NULL;
}

bool GPFileSystem::closeFile ( GPFile* file )
{
  if (!file || !file->provider)
    return NULL;

  bool open = file->opened();
  file->provider->closeFile(file);

  return open;
}

void GPFileSystem::addFileSystemProvider ( GPFileSystemProvider *provider )
{
  if (provider && provider->getName().size())
    fileSystemList.push_back(provider)
}

std::vector<std::string> GPFileSystem::getFileSystemProviderList ( void )
{
  std::vector<std::string> providers;

  for (int i = 0; i < fileSystemList.size(); i++)
    providers.push_back(fileSystemList[i]->getName());

  return providers;
}

GPFileSystem::GPFileSystem()
{
  //the base disk file system is
  static GPDiskFileSystemProvider diskFileSystem;
  addFileSystemProvider(&diskFileSystem);
}

GPFileSystem::~GPFileSystem()
{
  for (int i = 0; i < fileSystemList.size(); i++)
    fileSystemList[i]->closeAllFiles();

  fileSystemList.clear();
}

GPFileSystemProvider * GPFileSystem::findProvider ( const char* _name )
{
  if (!_name || !strlen(_name))
    return NULL;
  std::string name = _name;

  for (int i = 0; i < fileSystemList.size(); i++)
  {
    if (fileSystemList[i]->getName() == name)
      return fileSystemList[i];
  }
  return NULL;
}


// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8