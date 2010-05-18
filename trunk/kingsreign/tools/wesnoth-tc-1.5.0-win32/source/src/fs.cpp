//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/fs.cpp - Filesystem routines
//
// Copyright (C) 2008, 2009 by Ignacio R. Morelle <shadowm2006@gmail.com>
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

#include "fs.hpp"
#include "debug.hpp"
extern "C" {
	#include <libgen.h>
	#include <sys/stat.h>
	#include <sys/types.h>
	#include <unistd.h>
}
#include <boost/assign.hpp>
#include <cerrno>
#include <cstring>
#include <iostream>
#include <vector>

namespace
{
	/**
	 * Actual filesystem object access test.
	 * @param s     stat structure.
	 * @param read  Test read permissions.
	 * @param write Test write permssions.
	 * @param exec  Test exec permissions.
	 * @return      Whether all specified tests passed or not.
	 */
	bool test_fs_object_mode(struct stat& s, bool read, bool write, bool exec)
	{
		if(read == false && write == false && exec == false) {
			return true;
		}

#ifndef _WIN32
		const uid_t euid = geteuid();
		const gid_t egid = getegid();
		const bool r = (s.st_mode & S_IRUSR && euid == s.st_uid) || (s.st_mode & S_IRGRP && egid == s.st_gid) || (s.st_mode & S_IROTH);
		const bool w = (s.st_mode & S_IWUSR && euid == s.st_uid) || (s.st_mode & S_IWGRP && egid == s.st_gid) || (s.st_mode & S_IWOTH);
		const bool x = (s.st_mode & S_IXUSR && euid == s.st_uid) || (s.st_mode & S_IXGRP && egid == s.st_gid) || (s.st_mode & S_IXOTH);

		return (read ? r : true) && (write ? w : true) && (exec ? x : true);
#else
		// FIXME: maybe do actual permission checking on Win32...
		return (&s != NULL); // silence unused param warning
#endif
	}

	bool xstat(const std::string& path, struct stat& s)
	{
		if(stat(path.c_str(), &s) != 0) {
			if(debug_mode()) {
				int e = errno;
				std::cerr << "stat() of " << path << " failed (" << strerror(e) << ")\n";
			}
			return false;
		}
		return true;
	}
}

bool check_regular_file_permissions(const std::string& path, bool read, bool write, bool exec)
{
	struct stat s;
	if(!xstat(path, s)) {
		return false;
	}
	return s.st_mode & S_IFREG && test_fs_object_mode(s, read, write, exec);
}

bool check_dir_permissions(const std::string& path, bool read, bool write, bool search)
{
	struct stat s;
	if(!xstat(path, s)) {
		return false;
	}
	return s.st_mode & S_IFDIR && test_fs_object_mode(s, read, write, search);
}

std::string get_cwd()
{
	char buf[1024];
	const char* const res = getcwd(buf,sizeof(buf));
	if(res != NULL) {
		std::string str(res);

#ifdef _WIN32
		std::replace(str.begin(),str.end(),'\\','/');
#endif

		return str;
	} else {
		return "";
	}
}

std::string get_exe_dir()
{
#ifndef _WIN32
	char buf[1024];
	size_t path_size = readlink("/proc/self/exe", buf, 1024);
	if(path_size == static_cast<size_t>(-1))
		return get_cwd();
	buf[path_size] = 0;
	return std::string(dirname(buf));
#else
	return get_cwd();
#endif
}

namespace {
	std::vector<std::string> finder_locs = boost::assign::list_of
		("/share/wesnoth-tc/")
		("/etc/wesnoth-tc/")
		("/share/")
		("/etc/")
		("/bin/")
		("/")
	;
}

std::string find_file_in_prefix(const std::string& prefix, const std::string& file)
{
	std::string path = "";
	if(debug_mode()) {
		std::cerr << "searching for " << file << " on " << prefix << " within " << finder_locs.size() << " subdirectories\n";
	}
	for(size_t k = 0; k < finder_locs.size(); ++k) {
		path = prefix + finder_locs[k] + file;
		if(check_regular_file_permissions(path, true)) {
			break;
		}
		path.clear();
	}
	return path;
}

// kate: indent-mode normal; encoding utf-8; space-indent off; indent-width 4;
