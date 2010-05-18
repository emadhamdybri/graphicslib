//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/rc_png.hpp - PNG recoloring logic (interface)
//
// Copyright (C) 2008, 2009 by Ignacio R. Morelle <shadowm@wesnoth.org>
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

#ifndef RC_PNG_HPP__
#define RC_PNG_HPP__

#include "util.hpp"
#include <string>

bool recolor_image(const std::string& input_filename, const std::string& output_filename, const class color_range& target_range, const std::vector<uint32_t>& key_colors);
bool switch_image_palette(const std::string& input_filename, const std::string& output_filename, const std::vector<uint32_t>& target_colors, const std::vector<uint32_t>& key_colors);

#endif // ! RC_PNG_HPP__
