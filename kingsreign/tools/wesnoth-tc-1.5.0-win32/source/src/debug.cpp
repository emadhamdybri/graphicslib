//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/debug.hpp - Debug mode setting
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

#include "debug.hpp"

namespace { bool debug_flag = false; }

bool debug_mode()
{
	return debug_flag;
}

void set_debug_mode(bool v)
{
	debug_flag = v;
}

// kate: indent-mode normal; encoding utf-8; space-indent off; indent-width 4;
