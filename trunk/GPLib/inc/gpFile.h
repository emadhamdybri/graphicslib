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

#ifndef _GPFILE_H_
#define _GPFILE_H_

#include <string>
#include <map>
#include <vector>

class GPFileSystemProvider;

class GPFile
{
public:
  GPFileSystemProvider *provider;

  bool valid ( void );
  bool opened ( void );
  bool writeable ( void );

  const char* name ( void );

protected:
  friend GPFileSystemProvider;
  virtual ~GPFile(){};
};

class GPFileSystemProvider
{
public:
  virtual ~GPFileSystemProvider(){};

  typedef enum
  {
    eFileLoaded = 0,
    eFileNotFound,
    ePathNotHandled
  }FileStatus;

  virtual GPFile* getFile ( const char* path, FileStatus &status ) = 0;
  virtual GPFile* getFile ( const std::string &path, FileStatus &status ) {return getFile(path.c_str(),status);}
  virtual GPFile* createFile ( const char* path, FileStatus &status ) = 0;
  virtual GPFile* createFile ( const std::string &path, FileStatus &status ) {return createFile(path.c_str(),status);}

  virtual bool closeFile ( GPFile* file ) = 0;

  virtual std::string getName ( void ) = 0;

  virtual void closeAllFiles ( void ) = 0;
};

class GPFileSystem
{
public:
  static GPFileSystem& instance ( void )
  {
    static GPFileSystem filesystem;
    return filesystem;
  }

  virtual GPFile* getFile ( const char* path, const char* fileSystem = NULL );
  virtual GPFile* getFile ( const std::string &path, const std::string &fileSystem = std::string("") ) {return getFile(path.c_str(),fileSystem.c_str());}
  virtual GPFile* createFile ( const char* path,  const char* fileSystem = NULL );
  virtual GPFile* createFile ( const std::string &path, const std::string &fileSystem = std::string("")  ) {return createFile(path.c_str(),fileSystem.c_str());}

  virtual bool closeFile ( GPFile* file );

  void addFileSystemProvider ( GPFileSystemProvider *provider );
  std::vector<std::string> getFileSystemProviderList ( void );

  std::vector<std::string> getFileList ( const char* path, bool recursive = false, const char* filter = NULL );
  std::vector<std::string> getFileList ( const std::string &path, bool recursive = false, const char* filter = NULL );

  std::vector<std::string> getDirList ( const char* path, bool recursive = false );
  std::vector<std::string> getDirList ( const std::string &path, bool recursive = false );

protected:
  GPFileSystem();
  virtual ~GPFileSystem();

  GPFileSystemProvider *findProvider ( const char* name );

  typedef std::vector<GPFileSystemProvider*>	FileSystemProviderList;

  FileSystemProviderList    fileSystemList;

};


#endif //_GPFILE_H_

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8
