//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/string_utils.cpp - Character string algorithms
//
// Copyright (C) 2008 by Ignacio R. Morelle <shadowm@wesnoth.org>
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

#include "string_utils.hpp"

namespace utils {

std::vector< std::string > split(std::string const &val, char c)
{
	std::string sval = val;
	remove_surrounding_quotes(sval);
	
	std::vector< std::string > res;

	std::string::const_iterator i1 = sval.begin();
	std::string::const_iterator i2 = sval.begin();

	while (i2 != sval.end()) {
		if (*i2 == c) {
			std::string new_val(i1, i2);
			if (!new_val.empty())
				res.push_back(new_val);
			++i2;
			i1 = i2;
		} else {
			++i2;
		}
	}

	std::string new_val(i1, i2);
	if (!new_val.empty())
		res.push_back(new_val);

	return res;
}

void remove_surrounding_quotes(std::string& str)
{
	std::string::size_type p;

	do {
		p = str.find('\"');
		if (p != str.npos)
			str.erase(p, 1);
	} while (p != str.npos);
}

} // end utils namespace

// kate: indent-mode normal; encoding utf-8; space-indent off; indent-width 4;
