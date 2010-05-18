//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/wesnoth-tc.cpp - Program entry point
//
// Copyright (C) 2003-2008 by the Battle for Wesnoth Project <www.wesnoth.org>
// Copyright (C) 2008, 2009, 2010 by Ignacio R. Morelle <shadowm2006@gmail.com>
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

#include <boost/foreach.hpp>
#include <boost/algorithm/string.hpp>
#include <list>
#include <vector>
#include <utility>
#include <string>
#include <iostream>
#include <cstdio>
#include <cstdlib>

#include "debug.hpp"
#include "fs.hpp"
#include "job.hpp"
#include "parser.hpp"
#include "rc_png.hpp"
#include "string_utils.hpp"
#include "util.hpp"

#define foreach BOOST_FOREACH

namespace {
	std::list< std::string > cmdline_args;

	std::list< std::string > input_files;

	std::vector< std::string > use_ranges;
	std::vector< std::string > use_palettes;

	std::string tcdef_file = "tc_ranges.def";
	std::string keypal_name = "magenta";

	const std::string default_rc_list = "1,2,3,4,5,6,7,8,9";

	char * argv0;

	const char * g_c_helptxt =
			"Usage:\n"
			"    %s [options] input_files\n"
			"\n"
			"Options:\n"
			"\n"
			"--key <palette name>\n"
			"     Specifies a different key palette than 'magenta' to be used for recoloring\n"
			"     the input files. The specified palette must be declared in the RC definition\n"
			"     file (see --def).\n"
			"\n"
			"--rc, --tc <ranges list>\n"
			"     Specifies a comma-separated list of team-color ranges to be used.\n"
			"     By default, the team-colorizer will use the default (\"1,2,3,4,5,6,7,8,9\").\n"
			"     The specified ranges must be declared in the RC definition file (see --def).\n"
			"     Output files have the \"-RC-<key>-<range id>\" suffix.\n"
			"\n"
			"--pal <palette list>\n"
			"     Specifies a comma-separated list of color palettes to replace pixels\n"
			"     matching the 'key' palette. The target palettes must be declared in the RC\n"
			"     definition file. If the target or key palettes aren't all of the same size,\n"
			"     then all of them are truncated by the lowest size. Output files have the\n"
			"     \"-PAL-<key>-<palette id>\" suffix.\n"
			"\n"
			"--def <filename>\n"
			"     Specifies a RC definition file to be used instead of the default\n"
			"     './tc_ranges.def'.\n"
			"\n"
			"--debug\n"
			"     Increases output verbosity for testing or debugging.\n"
			"\n"
			"Any bugs, complaints and similar should be sent to <shadowm@wesnoth.org>\n"
			"\n"
			"This program is free software; you can redistribute it and/or modify it under\n"
			"the terms of the GNU General Public License as published by the Free Software\n"
			"Foundation; either version 2 of the License, or (at your option) any later\n"
			"version.\n"
			"\n"
			"This program is distributed in the hope that it will be useful, but WITHOUT\n"
			"ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS\n"
			"FOR A PARTICULAR PURPOSE. See the GNU General Public License on the COPYING\n"
			"file for details.\n"
			"\n"
			;

	bool fix_file_path(std::string& file)
	{
		std::string path = file;
		if(!check_regular_file_permissions(path, true)) {
			path = find_file_in_prefix(get_cwd(), file);
			if(path.empty()) {
				path = find_file_in_prefix(get_exe_dir(), file);
				if(path.empty()) {
					path = find_file_in_prefix(WESNOTH_TC_PREFIX, file);
					if(path.empty()) {
						const char* home_path = getenv("HOME");
						if(home_path) {
							path = find_file_in_prefix(home_path, file);
						}
					}
				}
			}
		}
		if(path.empty()) {
			std::cerr << "Could not guess path to the RC definition file.\n";
			return false;
		}

		file = path;

		return true;
	}

	void setup()
	{
		std::string pal_list, rc_list;
		std::list<std::string>::const_iterator i;
		for (i = cmdline_args.begin(); i != cmdline_args.end(); ++i)
		{
			if ( ((*i) == "--help" ) || ((*i) == "-h") ) {
				printf(g_c_helptxt, (argv0 != NULL ? argv0 : "wesnoth-tc"));
				exit(0);
			}
			else if ( ((*i) == "--version") || ((*i) == "-v") ) {
				exit(0);
			}
			else if ( (*i) == "--def" ) {
				++i;
				tcdef_file = (*i);
			}
			else if ( (*i) == "--debug") {
				set_debug_mode(true);
			}
			else if ( (*i) == "--tc" || (*i) == "--rc" ) {
				++i;
				rc_list = (*i);
				boost::trim(rc_list);
			}
			else if ( (*i) == "--key" ) {
				++i;
				keypal_name = (*i);
				boost::trim(keypal_name);
			}
			else if ( (*i) == "--pal" ) {
				++i;
				pal_list = (*i);
				boost::trim(pal_list);
			}
			else {
				input_files.push_back(*i);
			}
		}

		if(pal_list.empty() && rc_list.empty()) {
			rc_list = default_rc_list;
		}

		if(!rc_list.empty()) {
			boost::split(use_ranges,   rc_list,  boost::is_any_of(","), boost::token_compress_on);
			foreach(std::string& s, use_ranges) { boost::trim(s); }
		}

		if(!pal_list.empty()) {
			boost::split(use_palettes, pal_list, boost::is_any_of(","), boost::token_compress_on);
			foreach(std::string& s, use_palettes) { boost::trim(s); }
		}

		if (input_files.size() == 0) {
			std::cerr << "You must pass a list of input files; see '" << (argv0?argv0:"wesnoth-tc")<< " --help' for details\n";
			exit(1);
		}
	}
}


int main( int argc, char ** argv )
{
	std::cout << "Wesnoth graphics Team-Colorizer program\n"
	          << "Version " << VERSION << '\n'
#ifdef WORDS_BIGENDIAN
			  << "big-endian\n"
#endif
	          << "Copyright (c) 2008, 2009, 2010 by Ignacio R. Morelle <shadowm@wesnoth.org>\n\n";

	argv0 = argv[0];
	int k;
	for (k = 1; k < argc; ++k) {
		if (argv[k]) {
			cmdline_args.push_back(argv[k]);
		}
	}

	setup();

	if(!fix_file_path(tcdef_file)) {
		std::cerr << "Could not find RC definition file '" << tcdef_file << "'.\n";
		return 1;
	}

	parser psr (tcdef_file);

	//
	// Begin sanity checks
	//

	if(psr.good() != true) {
		std::cerr << "Could not load RC definition file '" << tcdef_file << "'.\n";
		return 1;
	}
	else {
		std::cout << "Using RC definition file: " << tcdef_file << '\n';
	}

	if(use_ranges.empty() && use_palettes.empty()) {
		std::cerr << "You did not specify any ranges or palettes to work with!\n";
		return 1;
	}

	if(!psr.have_pal(keypal_name)) {
		std::cerr << "The key palette '" << keypal_name << "' does not seem to exist.\n";
		return 1;
	}

	if(!use_ranges.empty()) {
		if(debug_mode()) {
			std::cout << "Using color ranges:\n";
		}
		foreach(const std::string& range_id, use_ranges) {
			if(!psr.have_range(range_id)) {
				std::cerr << "The target color range '" << range_id << "' does not seem to exist.\n";
				return 1;
			}
			else if(debug_mode()) {
				std::cout << " * " << range_id << '\n';
			}
		}
	}

	if(!use_palettes.empty()) {
		if(debug_mode()) {
			std::cout << "Using target palettes:\n";
		}
		foreach(const std::string& palette_id, use_palettes) {
			if(!psr.have_pal(palette_id)) {
				std::cerr << "The target color palette '" << palette_id << "' does not seem to exist.\n";
				return 1;
			}
			else if(debug_mode()) {
				std::cout << " * " << palette_id << '\n';
			}
		}
	}

	//
	// End sanity checks
	//

	std::vector<uint32_t> key = psr.get_palette(keypal_name);
	std::list<rc_job>  rc_jobs;
	std::list<pal_job> pal_jobs;

	rc_job  new_rc_job;
	pal_job new_pal_job;

	if(debug_mode()) {
		std::cout << "Generating job queue...";
	}

	foreach(const std::string& file, input_files)
	{
		std::string output_file_stem = file;
		const size_t len = output_file_stem.length();
		if(len > 4 && output_file_stem.substr(len-4) == ".png") {
			output_file_stem.resize(len-4);
		}
	
		foreach(const std::string& range_id, use_ranges)
		{
			rc_jobs.push_back(rc_job());
			rc_job& job = rc_jobs.back();

			std::string output_file = output_file_stem;
			output_file += "-RC-";
			output_file += keypal_name;
			output_file += "-";
			output_file += range_id;
			output_file += ".png";

			job.input_file = file;
			job.output_file = output_file;
			job.rc_range = psr.get_color_range(range_id);
		}

		foreach(const std::string& palette_id, use_palettes)
		{
			pal_jobs.push_back(pal_job());
			pal_job& job = pal_jobs.back();

			std::string output_file = output_file_stem;
			output_file += "-PAL-";
			output_file += keypal_name;
			output_file += "-";
			output_file += palette_id;
			output_file += ".png";

			job.input_file = file;
			job.output_file = output_file;
			job.target_palette = psr.get_palette(palette_id);
		}
	}

	if(debug_mode()) {
		std::cout << " done\n";
		std::cout << "Processing jobs...\n";
	}

	foreach(const rc_job& job, rc_jobs) {
		if(!recolor_image(job.input_file, job.output_file, job.rc_range, key)) {
			std::cerr << "Failed! :(\n";
			return 1;
		}
	}

	foreach(const pal_job& job, pal_jobs) {
		if(!switch_image_palette(job.input_file, job.output_file, job.target_palette, key)) {
			std::cerr << "Failed! :(\n";
			return 1;
		}
	}

	if(debug_mode()) {
		std::cout << "Success.\n";
	}
	
	return 0;
}

// kate: indent-mode normal; encoding utf-8; space-indent off; indent-width 4;
