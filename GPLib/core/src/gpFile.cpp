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
#include "gpWinAPI.h"

#include <list>
#include <algorithm>

// subs for the file stack methods
#ifdef _WIN32
bool WindowsAddFileStack ( const char *path, const char* fileMask, bool recursive, std::vector<std::string> &list, bool justDirs = false );
#define  _ADDFILESTACK WindowsAddFileStack
#else
bool PosixAddFileStack ( const char *path, const char* fileMask, bool recursive, std::vector<std::string> &list, bool justDirs = false );
#define  _ADDFILESTACK PosixAddFileStack
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

  virtual bool open ( bool write = false, bool text = true, bool apend = false );

  // reading 
  virtual bool getData ( char *data );
  virtual std::string getLine ( bool leaveNativeEndings = false );
  virtual std::vector<std::string> getFileLines ( bool leaveNativeEndings = false );
  virtual std::string getText ( bool leaveNativeEndings = false );

  virtual bool read ( void * data, size_t size, unsigned int count = 1 );

  // writing
  virtual bool putLine ( const std::string &data, bool nativeLineEnding = true );
  virtual bool putLine ( const char *data, bool nativeLineEnding = true );
  virtual bool putFileLines ( const std::vector<std::string> &data,  bool nativeLineEnding = true );
  virtual bool putText ( const std::string &data, bool nativeLineEnding = true );
  virtual bool putText ( const char *data, bool nativeLineEnding = true );

  virtual bool write ( void *data, size_t size, unsigned int count = 1);

  virtual std::string name ( void );
  virtual size_t size ( void );
  virtual bool valid ( void );
  virtual bool opened ( void );
  virtual bool writeable ( void );

protected:
  friend GPDiskFileSystemProvider;
  GPDiskFile( const char *path );

  void close ( void );

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
  OpenDiskFileMap openFiles;

  std::string basePath;
  const char* getFullPath ( const char * file );
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
  nativeFileSystem = new GPDiskFileSystemProvider;
  addFileSystemProvider(nativeFileSystem,NULL);
}

GPFileSystem::~GPFileSystem()
{
  for (int i = 0; i < (int)fileSystemList.size(); i++)
    fileSystemList[i]->closeAllFiles();

  fileSystemList.clear();

  delete(nativeFileSystem);
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

void GPDiskFile::close ( void )
{
  if (file)
    fclose(file);

  file = NULL;
  writeAccess = false;
}

//------------------------GPDiskFileSystemProvider------------------------
GPDiskFileSystemProvider::~GPDiskFileSystemProvider()
{
  closeAllFiles();
}

void GPDiskFileSystemProvider::init ( const char* rootPath  )
{
  if (rootPath)
    basePath = rootPath;
  else
    basePath = "";
}

const char* GPDiskFileSystemProvider::getFullPath ( const char * file )
{
  static std::string path = basePath;
  if (!file)
    return NULL;
  path += file;

  return path.c_str();
}

GPFile* GPDiskFileSystemProvider::getFile ( const char* path, FileStatus &status )
{
  GPDiskFile *file = new GPDiskFile(getFullPath(path));
  file->provider = this;

  if (!file->open(false,false,false))
  {
    file->close();
    delete(file);
    status = GPDiskFileSystemProvider::eFileNotFound;
    return NULL;
  }

  file->close();
  openFiles.push_back(file);
  status = GPDiskFileSystemProvider::eFileLoaded;
  return file;
}

GPFile* GPDiskFileSystemProvider::createFile ( const char* path, FileStatus &status )
{
  GPDiskFile *file = new GPDiskFile(getFullPath(path));
  file->provider = this;
  openFiles.push_back(file);
  status = GPDiskFileSystemProvider::eFileLoaded;
  return file;
}

bool GPDiskFileSystemProvider::closeFile ( GPFile* file )
{
  GPDiskFile *p = (GPDiskFile*)file;
  if (p->provider != this )
    return false;

  bool opened = p->opened();
  p->close();

  OpenDiskFileMap::iterator itr = std::find(openFiles.begin(),openFiles.end(),p);
  if (itr != openFiles.end())
    openFiles.erase(itr);
  delete(p);

  return opened;
}

void GPDiskFileSystemProvider::closeAllFiles ( void )
{
  OpenDiskFileMap::iterator itr = openFiles.begin();
  while (itr != openFiles.end())
  {
    (*itr)->close();
    delete(*itr);
    itr++;
  }
    
  openFiles.clear();
}

std::set<std::string> GPDiskFileSystemProvider::getFileList ( const char* path, bool recursive, const char* filter )
{
  std::set<std::string> files;

  std::vector<std::string> rawList;
  _ADDFILESTACK(getFullPath(path),filter,recursive,rawList,false);
  for ( size_t i = 0; i < rawList.size(); i++ )
    files.insert(rawList[i]);

  return files;
}

std::set<std::string> GPDiskFileSystemProvider::getDirList ( const char* path, bool recursive )
{
  std::set<std::string> dirs;

  std::vector<std::string> rawList;
  _ADDFILESTACK(getFullPath(path),NULL,recursive,rawList,true);
  for ( size_t i = 0; i < rawList.size(); i++ )
    dirs.insert(rawList[i]);

  return dirs;
}

// native file stack stuff

#ifdef _WIN32
#define _DirDelim '\\'
bool WindowsAddFileStack ( const char *path, const char* fileMask, bool recursive, std::vector<std::string> &list, bool justDirs )
{
  struct _finddata_t fileInfo;

  long		hFile;
  std::string searchstr;

  std::string FilePath;

  bool	bDone = false;

  searchstr = path;
  searchstr += "\\";
  if (recursive)
    searchstr += "*.*";
  else if (fileMask)
    searchstr += fileMask;
  else
    searchstr += "*.*";

  std::string extenstionSearch;

  if ( fileMask && strchr(fileMask,'.'))
    extenstionSearch = strchr(fileMask,'.')+1;

  hFile = (long)_findfirst(searchstr.c_str(),&fileInfo);

  if (hFile != -1)
  {
    while (!bDone)
    {
      if ((strlen(fileInfo.name) >0) && (strcmp(fileInfo.name,".") != 0) && 
	(strcmp(fileInfo.name,"..") != 0))
      {
	FilePath = path;
	//if (!(fileInfo.attrib & _A_SUBDIR ))
	FilePath += "\\";
	FilePath += fileInfo.name;

	if (justDirs && (fileInfo.attrib & _A_SUBDIR ))	// we neever do just dirs recrusively
	  list.push_back(FilePath);
	else if (!justDirs)
	{
	  if ( (fileInfo.attrib & _A_SUBDIR ) && recursive)
	    WindowsAddFileStack(FilePath.c_str(),fileMask,recursive,list);
	  else if (!(fileInfo.attrib & _A_SUBDIR) )
	  {
	    if (recursive && fileMask)	// if we are recusive we need to check extension manualy, so we get dirs and stuf
	    {
	      if (strrchr(FilePath.c_str(),'.'))
	      {
		if ( stricmp(strrchr(FilePath.c_str(),'.')+1, extenstionSearch.c_str() ) == 0 )
		  list.push_back(FilePath);
	      }
	    }
	    else
	      list.push_back(FilePath);
	  }
	}
      }
      if (_findnext(hFile,&fileInfo) == -1)
	bDone = true;
    }
  }
  return true;
}
#else
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>
#include <dirent.h>
#include <ctype.h>

#define _DirDelim '/'
static int match_multi (const char **mask, const char **string)
{
  const char *msk;
  const char *str;
  const char *msktop;
  const char *strtop;

  msk = *mask;
  str = *string;

  while ((*msk != '\0') && (*msk == '*'))
    msk++;                      /* get rid of multiple '*'s */

  if (*msk == '\0')				/* '*' was last, auto-match */
    return +1;

  msktop = msk;
  strtop = str;

  while (*msk != '\0')
  {
    if (*msk == '*')
    {
      *mask = msk;
      *string = str;
      return 0;                 /* matched this segment */
    }
    else if (*str == '\0')
      return -1;                /* can't match */
    else
    {
      if ((*msk == '?') || (*msk == *str))
      {
	msk++;
	str++;
	if ((*msk == '\0') && (*str != '\0'))	/* advanced check */
	{										
	  str++;
	  strtop++;
	  str = strtop;
	  msk = msktop;
	}
      }
      else
      {
	str++;
	strtop++;
	str = strtop;
	msk = msktop;
      }
    }
  }

  *mask = msk;
  *string = str;
  return +1;											 /* full match */
}

static int match_mask (const char *mask, const char *string)
{ 
  if (mask == NULL)
    return 0;

  if (string == NULL)
    return 0;

  if ((mask[0] == '*') && (mask[1] == '\0'))
    return 1;									/* instant match */

  while (*mask != '\0')
  {
    if (*mask == '*')
    {
      mask++;
      switch (match_multi (&mask, &string))
      {
      case +1:
	return 1;
      case -1:
	return 0;
      }
    }
    else if (*string == '\0')
      return 0;
    else if ((*mask == '?') || (*mask == *string))
    {
      mask++;
      string++;
    }
    else
      return 0;
  }

  if (*string == '\0')
    return 1;
  else
    return 0;
}

bool PosixAddFileStack ( const char *path, const char* fileMask, bool recursive, std::vector<std::string> &list, bool justDirs )
{
  DIR	*directory;
  dirent	*fileInfo;
  struct stat	statbuf;
  char	searchstr[1024];
  std::string	FilePath;

  strcpy(searchstr, path);
  if (searchstr[strlen(searchstr) - 1] != '/')
    strcat(searchstr, "/");
  directory = opendir(searchstr);
  if (!directory)
    return false;

  // TODO: make it use the filemask
  while ((fileInfo = readdir(directory)))
  {
    if (!((strcmp(fileInfo->d_name, ".") == 0) || (strcmp(fileInfo->d_name, "..") == 0)))
    {
      FilePath = searchstr;
      FilePath += fileInfo->d_name;


      stat(FilePath.c_str(), &statbuf);

      if (justDirs && S_ISDIR(statbuf.st_mode))	// we never do just dirs recrusively
	list.push_back(FilePath);
      else if (!justDirs)
      {
	if (S_ISDIR(statbuf.st_mode) && recursive)
	  PosixAddFileStack(FilePath.c_str(),fileMask,recursive, list);
	else if (match_mask (fileMask, fileInfo->d_name))
	  list.push_back(FilePath);
      }
    }
  }
  closedir(directory);
  return true;
}
#endif 
// Local Variables: ***
// mode:C++ ***
// tab-width: 8 ***
// c-basic-offset: 2 ***
// indent-tabs-mode: t ***
// End: ***
// ex: shiftwidth=2 tabstop=8