//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/fs.hpp - Filesystem routines
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

#ifndef WESNOTH_TC_FS_HPP_INCLUDED__
#define WESNOTH_TC_FS_HPP_INCLUDED__

#include <string>

/**
 * Checks whether the specified regular file exists and can be accessed.
 * @param path  Checked path.
 * @param read  Check if the effective uid can read.
 * @param write Check if the effective uid can write.
 * @param exec  Check if the effective uid can execute.
 * @return      false if 'path' does not exist, it is not a regular file, or
 *              doesn't satisfy all selected checks.
 */
bool check_regular_file_permissions(const std::string& path, bool read=false, bool write=false, bool exec=false);

/**
 * Checks whether the specified directory exists and can be accessed.
 * @param path  Checked path.
 * @param read  Check if the effective uid can read.
 * @param write Check if the effective uid can write.
 * @return      false if 'path' does not exist, it is not a directory, or
 *              doesn't satisfy all selected checks.
 */
bool check_dir_permissions(const std::string& path, bool read=false, bool write=false, bool search=false);

/**
 * Returns the path to the current working directory.
 */
std::string get_cwd();

/**
 * Tries to determine the program's executable image file directory.
 * If it fails, it returns the current directory path instead.
 */
std::string get_exe_dir();

/**
 * Tries to locate @a file in the specified @a prefix using standard Unix paths.
 * If it succeeds, it returns the first path to @a file that resolves to a readable
 * object. If it fails, it will return an empty string.
 */
std::string find_file_in_prefix(const std::string& prefix, const std::string& file);

#endif // ! WESNOTH_TC_FS_HPP_INCLUDED__

// kate: indent-mode normal; encoding utf-8; space-indent off; indent-width 4;
