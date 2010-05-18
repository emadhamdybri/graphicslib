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

#include "debug.hpp"
#include "parser.hpp"
#include "color_range.hpp"
#include <vector>
#include <string>
#include <fstream>
#include <iostream>
#include <sstream>
#include <iosfwd>
#include <cassert>

namespace {
	// TC-def language conventions
	const std::string cmd_alias = "alias";
	const std::string cmd_rc_pal = "rc_pal";
	const std::string cmd_rc_range = "rc_range";

	const char comment_line = '#';
	const char list_separator = ',';
	const char newline = '\n';

	const std::string cmd_comment = "#";
} // end anonymous namespace

bool parser::register_rc_range(const std::string& map_entry_name, const std::string& rc_array)
{
	if(debug_mode()) {
		std::cerr << "registering RC range '" << map_entry_name << "' of color array: " << rc_array << '\n';
	}
	std::vector<uint32_t> cvt_range = string2rgb(rc_array);

	color_range rc_range(cvt_range);
	return rc_range_tab_.insert( std::make_pair(map_entry_name, rc_range) ).second;
}

bool parser::register_rc_pal(const std::string& map_entry_name, const std::string& rc_array)
{
	if(debug_mode()) {
		std::cerr << "registering RC palette '" << map_entry_name << "' of color array: " << rc_array << '\n';
	}
	std::vector<uint32_t> cvt_pal = string2rgb(rc_array);

	return rc_pal_tab_.insert( std::make_pair(map_entry_name, cvt_pal) ).second;
}

std::vector< std::string > parser::get_parameters(COMMAND cmd, const std::string& line)
{
	const std::string* cmd_prefix;
	std::vector< std::string > result;

	switch ( cmd )
	{
		case RECOLOR_RANGE:
			{ cmd_prefix = &cmd_rc_range; break; }
		case RECOLOR_PALETTE:
			{ cmd_prefix = &cmd_rc_pal; break; }
		case ALIAS:
			{ cmd_prefix = &cmd_alias; break; }
		default:
			{ return result; }
	}

	const int param_start = 1+(cmd_prefix != NULL ? cmd_prefix->length() : -1);

	std::istringstream istr(line.substr(param_start));
	char c;
	std::ostringstream * ostr = new std::ostringstream();

	assert(ostr != NULL);

	// Split parameter list now
	while (!istr.eof()) {
		c = istr.get();
		if ((istr.eof() == false) && (c != newline)) {
			if (c != ' ') {
				(*ostr) << c;
			}
			else {
				std::string new_parameter = ostr->str();
				if (!new_parameter.empty()) {
					result.push_back(ostr->str());
				}
				delete ostr;
				ostr = new std::ostringstream();
				assert(ostr != NULL);
				continue;
			}
		} else {
			std::string new_parameter = ostr->str();
			if (!new_parameter.empty()) {
				result.push_back(ostr->str());
			}
			break;
		}
	}

	if (ostr != NULL) {
		delete ostr;
	}

	return result;
}

void parser::commit(COMMAND cmd, const std::string& line)
{
	std::vector< std::string > cmd_parameters = get_parameters(cmd, line);
	switch ( cmd )
	{
		case RECOLOR_RANGE:
			if (!this->register_rc_range(cmd_parameters[0], cmd_parameters[1])) {
				std::cerr << "failed registering RC range '" << cmd_parameters[0] << "'\n";
			}
			break;
		case RECOLOR_PALETTE:
			if (!this->register_rc_pal(cmd_parameters[0], cmd_parameters[1])) {
				std::cerr << "failed registering RC palette '" << cmd_parameters[0] << "'\n";
			}
			break;
		case ALIAS:
			if (!symbol_aliases_.insert(std::make_pair(cmd_parameters[0], cmd_parameters[1])).second) {
				std::cerr << "failed inserting alias '" << cmd_parameters[0] << "' to symbol '" << cmd_parameters[1] << "'!\n";
			}
			break;
		case COMMENT: case NONE:
			return;
		default:
			std::cerr << "parser error: '" << line << "'\n";
			return;
	}
}

void parser::ctor_internal()
{
	char c;
	std::istream& input = *this->in_;

	std::string current_line;
	COMMAND current_command = NONE;

	//int line = 0, col = 1;
	
	while (!input.eof()) {
		c = input.get();
		if (input.eof()) {
			break;
		}
		else if (c == newline) {
			if (current_command != COMMENT) {
				this->commit(current_command, current_line);
			}
			current_line.clear();
			current_command = NONE;
			continue;
		} else if (current_command != COMMENT) {
			current_line += c;

			if (current_line == cmd_alias)
				current_command = ALIAS;
			else if (current_line == cmd_rc_pal)
				current_command = RECOLOR_PALETTE;
			else if (current_line == cmd_rc_range)
				current_command = RECOLOR_RANGE;
			else if (current_line == cmd_comment) {
				current_command = COMMENT;
			}
		}
	}
}

parser::parser(const std::string& file_path):
	in_( new std::ifstream(file_path.c_str()) )
{
	if(in_->is_open())
		this->ctor_internal();
	else {
		delete in_;
		in_ = NULL;
	}
}

parser::~parser(void) {
	if(in_ != NULL) delete in_;
}

bool parser::is_pal(const std::string& sym)
{
	return (rc_pal_tab_.find(sym) != rc_pal_tab_.end());
}

bool parser::is_range(const std::string& sym)
{
	return (rc_range_tab_.find(sym) != rc_range_tab_.end());
}

std::string parser::resolve_symbol(const std::string& sym)
{
	std::map< std::string, std::string >::const_iterator alentry = symbol_aliases_.find(sym);
	// if it cannot be found in alias list we shall assume it exists...
	return ((alentry == symbol_aliases_.end()) ? sym : (*alentry).second);
}

std::vector<uint32_t> parser::get_palette(const std::string& sym)
{
	const std::string asym = this->resolve_symbol( sym );
	std::map< std::string, std::vector<uint32_t> >::const_iterator aentry = rc_pal_tab_.find(asym);

	if (aentry == rc_pal_tab_.end()) {
		if (this->is_range(asym)) {
			throw symbol_type_mismatch();
		}
		else {
			throw symbol_not_found();
		}

		return std::vector<uint32_t>();
	}

	return (*aentry).second;
}

color_range parser::get_color_range(const std::string& sym)
{
	const std::string asym = this->resolve_symbol( sym );
	std::map< std::string, color_range >::const_iterator aentry = rc_range_tab_.find(asym);

	if (aentry == rc_range_tab_.end()) {
		if (this->is_pal(asym)) {
			throw symbol_type_mismatch();
		}
		else {
			throw symbol_not_found();
		}

		return color_range(0x00000000);
	}

	return (*aentry).second;
}

bool parser::have_pal(const std::string& sym)
{
	const std::string asym = this->resolve_symbol( sym );
	return rc_pal_tab_.find(asym) != rc_pal_tab_.end();
}

bool parser::have_range(const std::string& sym)
{
	const std::string asym = this->resolve_symbol( sym );
	return rc_range_tab_.find(asym) != rc_range_tab_.end();
}

// kate: indent-mode normal; encoding utf-8; space-indent off; indent-width 4;
