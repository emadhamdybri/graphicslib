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
#include <set>
#include <vector>

class GPFileSystemProvider;

class GPFile
{
public:
  GPFileSystemProvider *provider;

  virtual bool open ( bool write = false, bool text = true, bool apend = false ) = 0;

  // reading 
  virtual bool getData ( char *data ) = 0;
  virtual std::string getLine ( bool leaveNativeEndings = false ) = 0;
  virtual std::vector<std::string> getFileLines ( bool leaveNativeEndings = false ) = 0;
  virtual std::string getText ( bool leaveNativeEndings = false ) = 0;

  virtual bool read ( void * data, size_t size, unsigned int count = 1 ) = 0;

  // writing
  virtual bool putLine ( const std::string &data, bool nativeLineEnding = true ) = 0;
  virtual bool putLine ( const char *data, bool nativeLineEnding = true ) = 0;
  virtual bool petFileLines ( const std::vector<std::string> &data,  bool nativeLineEnding = true ) = 0;
  virtual bool putText ( const std::string &data, bool nativeLineEnding = true ) = 0;
  virtual bool putText ( const char *data, bool nativeLineEnding = true ) = 0;

  virtual bool write ( void *data, size_t size, unsigned int count = 1) = 0;

  // info
  virtual std::string name ( void ) = 0;
  virtual size_t size ( void ) = 0;
  virtual bool valid ( void ) = 0;
  virtual bool opened ( void ) = 0;
  virtual bool writeable ( void ) = 0;

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

  virtual void init ( const char* rootPath  ) = 0;

  virtual GPFile* getFile ( const char* path, FileStatus &status ) = 0;
  virtual GPFile* getFile ( const std::string &path, FileStatus &status ) {return getFile(path.c_str(),status);}
  virtual GPFile* createFile ( const char* path, FileStatus &status ) = 0;
  virtual GPFile* createFile ( const std::string &path, FileStatus &status ) {return createFile(path.c_str(),status);}

  virtual bool closeFile ( GPFile* file ) = 0;

  virtual std::set<std::string> getFileList ( const char* path, bool recursive, const char* filter ) = 0;
  virtual std::set<std::string> getDirList ( const char* path, bool recursive ) = 0;

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

  GPFile* getFile ( const char* path, const char* fileSystem = NULL );
  GPFile* getFile ( const std::string &path, const std::string &fileSystem = std::string("") ) {return getFile(path.c_str(),fileSystem.c_str());}
  GPFile* createFile ( const char* path,  const char* fileSystem = NULL );
  GPFile* createFile ( const std::string &path, const std::string &fileSystem = std::string("") ) {return createFile(path.c_str(),fileSystem.c_str());}

  bool closeFile ( GPFile* file );

  void addFileSystemProvider ( GPFileSystemProvider *provider );
  std::vector<std::string> getFileSystemProviderList ( void );

  std::set<std::string> getFileList ( const char* path, bool recursive = false, const char* filter = NULL, const char* provider = NULL );
  std::set<std::string> getFileList ( const std::string &path, bool recursive = false, const std::string &filter = std::string(), const std::string &provider = std::string() ) {return getFileList(path.c_str(),recursive,filter.c_str(),provider.c_str());}

  std::set<std::string> getDirList ( const char* path, bool recursive = false, const char* provider = NULL );
  std::set<std::string> getDirList ( const std::string &path, bool recursive = false, const std::string &provider = std::string() ) {return getDirList(path.c_str(),recursive,provider.c_str());}

  void setRootPath ( const char* path );
  void setRootPath ( const std::string &path ) {setRootPath(path.c_str());}
  const char* getRootPath ( void );

  // common path utils
  const char* getAppDir ( void );
  const char* getUserDir ( void );
  const char* getTempDir ( void );

  const char* convertPathFromNative ( const char* path );
  const std::string convertPathFromNative ( const std::string &path ) { return std::string(convertPathFromNative(path.c_str()));}
  const char* convertPathToNative ( const char* path );
  const std::string convertPathToNative ( const std::string &path ) { return std::string(convertPathToNative(path.c_str()));}

protected:
  GPFileSystem();
  virtual ~GPFileSystem();

  GPFileSystemProvider *findProvider ( const char* name );

  // always a native path
  const char* getFullPath ( const char *file );
  const std::string getFullPath ( const std::string &file ){ return std::string(getFullPath(std::string(file)));}

private:
  typedef std::vector<GPFileSystemProvider*>	FileSystemProviderList;
 
  std::string rootPath;
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
