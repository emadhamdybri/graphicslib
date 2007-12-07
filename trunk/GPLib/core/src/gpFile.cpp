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

#include <list>

#ifdef _WIN32
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <shlobj.h>
#endif

// utils everyone needs.
const char* nativePath ( std::string  &path )
{
#ifdef _WIN32
  for ( unsigned int i = 0; i < (unsigned int)path.size(); i++ )
  {
    if ( path.c_str()[i] == '/' )
      path[i] = '\\';
  }
#endif
  return path.c_str();
}

const char* standardPath ( std::string &path )
{
#ifdef _WIN32
  for ( unsigned int i = 0; i < (unsigned int)path.size(); i++ )
  {
    if ( path.c_str()[i] == '\\' )
      path[i] = '/';
  }
#endif
  return path.c_str();
}

bool writeLineEnding ( FILE *fp, bool native = false )
{
#ifdef _WIN32
  if (native)
    return fprintf(fp,"\r\n") > 0;
#endif
  return fprintf(fp,"\n") > 0;
}

// defines for the disk based file system file system
class GPDiskFileSystemProvider;

class GPDiskFile : public GPFile
{
public:
  GPFileSystemProvider *provider;

  bool open ( bool write = false, bool text = true, bool apend = false ) = false;

  // reading 
  bool getData ( char *data );
  std::string getLine ( bool leaveNativeEndings = false );
  std::vector<std::string> getFileLines ( bool leaveNativeEndings = false );
  std::string getText ( bool leaveNativeEndings = false );

  bool read ( void * data, size_t size, unsigned int count = 1 );

  // writing
  bool putLine ( const std::string &data, bool nativeLineEnding = true );
  bool putLine ( const char *data, bool nativeLineEnding = true );
  bool putFileLines ( const std::vector<std::string> &data,  bool nativeLineEnding = true );
  bool putText ( const std::string &data, bool nativeLineEnding = true );
  bool putText ( const char *data, bool nativeLineEnding = true );

  bool write ( void *data, size_t size, unsigned int count = 1);

  std::string name ( void );
  size_t size ( void );
  bool valid ( void );
  bool opened ( void );
  bool writeable ( void );

protected:
  friend GPDiskFileSystemProvider;
  GPDiskFile( const char *path );

  FILE	*file;
  std::string filePath;
  bool	      writeAccess;

  virtual ~GPDiskFile();
};

class GPDiskFileSystemProvider : public GPFileSystemProvider
{
public:
  virtual ~GPDiskFileSystemProvider();

  virtual void init ( const char* rootPath  );

  virtual GPFile* getFile ( const char* path, FileStatus &status );
  virtual GPFile* createFile ( const char* path, FileStatus &status );

  virtual bool closeFile ( GPFile* file );

  virtual std::string getName ( void ){return std::string(GP_NATIVE_FILE_SYSTEM);}
  
  virtual void closeAllFiles ( void );

  virtual std::set<std::string> getFileList ( const char* path, bool recursive, const char* filter );
  virtual std::set<std::string> getDirList ( const char* path, bool recursive );

protected:
  typedef std::list<GPDiskFile*> OpenDiskFileMap;
};

//-------------------------GPFileSystem-----------------------//
GPFile* GPFileSystem::getFile ( const char* path, const char* fileSystem )
{
    GPFileSystemProvider::FileStatus status;
    GPFileSystemProvider *provider = findProvider(fileSystem);

    if (provider)
      return provider->getFile(getFullPath(path),status);

    // they didn't specify so try them in order.
    for (int i = 0; i < (int)fileSystemList.size(); i++)
    {
      GPFile *file = fileSystemList[i]->getFile(getFullPath(path),status);
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
    return provider->createFile(getFullPath(path),status);

  // they didn't specify so try them in order.
  for (int i = 0; i < (int)fileSystemList.size(); i++)
  {
    GPFile *file = fileSystemList[i]->createFile(getFullPath(path),status);
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

void GPFileSystem::addFileSystemProvider ( GPFileSystemProvider *provider, const char *insertBefore )
{
  if (provider && provider->getName().size())
  {
    if (insertBefore)
    {
      for (int i = 0; i < (int)fileSystemList.size(); i++)
      {
		if ( fileSystemList[i]->getName() == insertBefore )
		{
			fileSystemList.insert(fileSystemList.begin()+i,provider);
			return;
		}
      }
    }
    // ether it can't be found, or we don't care, so just slap it at the end.
    fileSystemList.push_back(provider);
  }
}

std::vector<std::string> GPFileSystem::getFileSystemProviderList ( void )
{
  std::vector<std::string> providers;

  for (int i = 0; i < (int)fileSystemList.size(); i++)
    providers.push_back(fileSystemList[i]->getName());

  return providers;
}

GPFileSystem::GPFileSystem()
{
  //the base disk file system is
  static GPDiskFileSystemProvider diskFileSystem;
  addFileSystemProvider(&diskFileSystem,NULL);
}

GPFileSystem::~GPFileSystem()
{
  for (int i = 0; i < (int)fileSystemList.size(); i++)
    fileSystemList[i]->closeAllFiles();

  fileSystemList.clear();
}

GPFileSystemProvider * GPFileSystem::findProvider ( const char* _name )
{
  if (!_name || !strlen(_name))
    return NULL;
  std::string name = _name;

  for (int i = 0; i < (int)fileSystemList.size(); i++)
  {
    if (fileSystemList[i]->getName() == name)
      return fileSystemList[i];
  }
  return NULL;
}

std::set<std::string> GPFileSystem::getFileList ( const char* path, bool recursive, const char* filter, const char* provider )
{
  GPFileSystemProvider *fileSystem = findProvider(provider);
  if (fileSystem)
    return fileSystem->getFileList(getFullPath(path),recursive,filter);

  std::set<std::string> fileList;
  for (int i = 0; i < (int)fileSystemList.size(); i++)
  {
    std::set<std::string> thisSet = fileSystemList[i]->getFileList(getFullPath(path),recursive,filter);
    fileList.insert(thisSet.begin(),thisSet.end());
  }

  return fileList;
}

std::set<std::string> GPFileSystem::getDirList ( const char* path, bool recursive, const char* provider )
{
  GPFileSystemProvider *fileSystem = findProvider(provider);
  if (fileSystem)
    return fileSystem->getDirList(getFullPath(path),recursive);

  std::set<std::string> dirList;
  for (int i = 0; i < (int)fileSystemList.size(); i++)
  {
    std::set<std::string> thisSet = fileSystemList[i]->getDirList(getFullPath(path),recursive);
    dirList.insert(thisSet.begin(),thisSet.end());
  }

  return dirList;
}

void GPFileSystem::setRootPath ( const char* path )
{
  rootPath = "";
  if (path)
   rootPath = convertPathToNative(path);

  for (int i = 0; i < (int)fileSystemList.size(); i++)
    fileSystemList[i]->init(rootPath.c_str());
}

const char* GPFileSystem::getRootPath ( void )
{
  return convertPathFromNative(rootPath.c_str());
}

// common path utils
const char* GPFileSystem::getAppDir ( void )
{
  std::string appPath;
#ifdef _WIN32
  char temp[MAX_PATH+1] = {0};
  GetModuleFileName(NULL,temp,MAX_PATH);
  if (strlen(temp))
  {
    char *p = strrchr(temp,'\\');
    if (p)
      ++p = NULL;

    appPath = temp;
  }
  else
   appPath = ".\\";
#else
  // get some nix and mac code here to find the applicaitons's path
#endif

  return convertPathFromNative(appPath).c_str();
}

const char* GPFileSystem::getUserDir ( void )
{
  std::string userPath;
#ifdef _WIN32
  char temp[MAX_PATH+1] = {0};
  if (SUCCEEDED(SHGetFolderPath(NULL,CSIDL_APPDATA,NULL,SHGFP_TYPE_CURRENT,temp)))
  {
    userPath = temp;
  }
  else
    userPath = ".\\";
#else
  // get some nix and mac code here to find the home dir or something
#endif

  return convertPathFromNative(userPath).c_str();
}

const char* GPFileSystem::getTempDir ( void )
{
  std::string tempPath;
#ifdef _WIN32
  char temp[MAX_PATH+1] = {0};
  if (GetTempPath(MAX_PATH,temp))
    tempPath = temp;
  else
    tempPath = ".\\";
#else
  // get some nix and mac code here to find the home dir or something
#endif

  return convertPathFromNative(tempPath).c_str();
}

const char* GPFileSystem::convertPathFromNative ( const char* path )
{
  return standardPath(std::string(path));
}

const char* GPFileSystem::convertPathToNative ( const char* path )
{
 return nativePath(std::string(path));
}

const char* GPFileSystem::getFullPath ( const char *file )
{
  static std::string fullPath = rootPath + convertPathToNative(file);
  return fullPath.c_str();
}

//-----------------GPDiskFile---------------//

GPDiskFile:: GPDiskFile( const char *path )
{
  if (path)
    filePath = path;

  file = NULL;
  writeAccess = false;
}

GPDiskFile::~GPDiskFile()
{
  if (file)
    fclose(file);
}

bool GPDiskFile::open ( bool w , bool text, bool append  )
{
  if ( !filePath.size())
    return false;

  writeAccess = w;

  std::string mode;
  if ( writeAccess )
  {
    if ( text )
    {
      if ( append )
	mode = "at";
      else
	mode = "wt";
    }
    else
    {
      if ( append )
	mode = "a";
      else
	mode = "wb";
    }
  }
  else
  {
    if ( text )
      mode = "rt";
    else
      mode = "rb";
  }

  file = fopen(filePath.c_str(),mode.c_str());
  return file != NULL;
}

// reading 
bool GPDiskFile::getData ( char *data )
{
  if (!file || writeAccess )
    return false;

  size_t  s = size();
  return fread(data,s,1,file) == s;
}

std::string GPDiskFile::getLine ( bool leaveNativeEndings )
{
  std::string text;
  if (!file || writeAccess )
    return text;

  bool done = false;
  while (!done)
  {
    char c;
    if (fscanf(file,"%c",&c) == 0)  // end of file damnit
      done = true;
    else if (c != 0)  // just to make sure there are no NULLs
    {
      if ( (c != 13) && (c == 10 && leaveNativeEndings))  // if it's not a LF, or if it's an CR and we are keeping them, then save the character
	text += c;
      else if ( c == 13 )
      {
	if (leaveNativeEndings)
	  text += c;
	done = true;
      }
    }
  }
  
  return text;
}

std::vector<std::string> GPDiskFile::getFileLines ( bool leaveNativeEndings )
{
  std::vector<std::string> lines;
  if (!file || writeAccess )
    return lines;

  std::string line = getLine(leaveNativeEndings);
  while ( line.size() )
  {
    lines.push_back(line);
    line = getLine(leaveNativeEndings);
  }

  return lines;
}

std::string GPDiskFile::getText ( bool leaveNativeEndings )
{
  std::string text;

 if (!file || writeAccess )
    return text;

  if (!file || writeAccess )
    return text;

  std::string line = getLine(leaveNativeEndings);
  while ( line.size() )
  {
    text += line;
    if (!leaveNativeEndings)
      text += "\n";
    line = getLine(leaveNativeEndings);
  }
  return text;
}

bool GPDiskFile::read ( void * data, size_t size, unsigned int count )
{
  if (!file || writeAccess )
    return false;

  size_t r = fread(data,size,count,file);
  return r == size*count;
}

// writing
bool GPDiskFile::putLine ( const std::string &data, bool nativeLineEnding )
{
  if (!file || !writeAccess )
    return false;

  size_t r = fwrite(data.c_str(),data.size(),1,file);
  return writeLineEnding(file,nativeLineEnding) && (r == data.size());
}

bool GPDiskFile::putLine ( const char *data, bool nativeLineEnding )
{
  if (!file || !writeAccess ||!data)
    return false;

  size_t l = strlen(data);
  size_t r = fwrite(data,l,1,file);
  return writeLineEnding(file,nativeLineEnding) && (r == l);
}

bool GPDiskFile::putFileLines ( const std::vector<std::string> &data,  bool nativeLineEnding )
{
  if (!file || !writeAccess )
    return false;

  for ( size_t i = 0; i < data.size(); i++ )
  {  
    if (!putLine(data[i],nativeLineEnding))
      return false;
  }
  return true;
}

bool GPDiskFile::putText ( const std::string &data, bool nativeLineEnding )
{
  return putText(data.c_str(),nativeLineEnding);
}

bool GPDiskFile::putText ( const char *data, bool nativeLineEnding )
{
  if (!file || !writeAccess  ||!data)
    return false;

  size_t l = strlen(data);
  
  for (size_t i = 0; i < l; i++)
  {
    char c = data[i];
    if ( c != '\n')
    {
      if (!fwrite(&c,1,1,file))
	return false;
    }
    else
    {
      if (!writeLineEnding(file,nativeLineEnding))
	return false;
    }
  }

  return true;
}

bool GPDiskFile::write ( void *data, size_t size, unsigned int count)
{
  if (!file || !writeAccess )
    return false;

  return fwrite(data,size,count,file) != size*count;
}

std::string GPDiskFile::name ( void )
{
  return std::string(standardPath(filePath));
}

size_t GPDiskFile::size ( void )
{
  if (!file)
    return 0;

  long  pos = ftell(file);
  fseek(file,0,SEEK_END);
  long s = ftell(file);
  fseek(file,pos,SEEK_SET);
  return (size_t)s;
}

bool GPDiskFile::valid ( void )
{
  if (file)
    return true;

  if (!filePath.size())
    return false;

  FILE *fp = fopen(filePath.c_str(),"rb");
  if (fp)
  {
    fclose(fp);
    return true;
  }
  return false;
}

bool GPDiskFile::opened ( void )
{
  return file != NULL;
}

bool GPDiskFile::writeable ( void )
{
  if (!file || !filePath.size())
    return false;

  return writeAccess;
}

// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8