//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/parser.cpp - Config file parser
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

#ifndef PARSER_HPP__
#define PARSER_HPP__

#include "color_range.hpp"
#include <vector>
#include <map>
#include <string>

struct symbol_type_mismatch {};
struct symbol_not_found {};

class parser
{
public:
	enum COMMAND
	{
		ALIAS, RECOLOR_PALETTE, RECOLOR_RANGE, COMMENT, NONE
	};

private:
	std::map< std::string, std::string > symbol_aliases_;
	std::map< std::string, std::vector<uint32_t> > rc_pal_tab_;
	std::map< std::string, color_range > rc_range_tab_;

	std::ifstream* in_;

	void ctor_internal();
	void commit(COMMAND cmd, const std::string& line);

	std::vector< std::string > get_parameters(COMMAND cmd, const std::string& line);

protected:
	bool register_rc_range(const std::string& map_entry_name, const std::string& rc_array);
	bool register_rc_pal(const std::string& map_entry_name, const std::string& rc_array);

	bool is_pal(const std::string& sym);
	bool is_range(const std::string& sym);
public:
	parser(const std::string& file_path);
	~parser();
	
	bool good() const { return in_ != NULL; }

	std::string resolve_symbol(const std::string& sym);

	std::vector<uint32_t> get_palette(const std::string& sym);
	color_range get_color_range(const std::string& sym);

	bool have_pal(const std::string& sym);
	bool have_range(const std::string& sym);
};

#endif

// kate: indent-mode normal; encoding utf-8; space-indent off; indent-width 4;
