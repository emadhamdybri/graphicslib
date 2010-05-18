//
// wesnoth-tc - Wesnoth Team-Colorizer
// src/rc_png.cpp - PNG recoloring logic
//
// Copyright (C) 2008, 2009, 2010 by Ignacio R. Morelle <shadowm@wesnoth.org>
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
#include "color_range.hpp"

#include <boost/utility.hpp>
#include <png.h>
#ifndef PNG_READ_SUPPORTED
	#error This program must be compiled with a version of libpng that supports read routines!
#endif
#ifndef PNG_WRITE_SUPPORTED
	#error This program must be compiled with a version of libpng that supports write routines!
#endif

#include <cassert>
#include <cstdio>
#include <iostream>
#include <string>

namespace {
	class pixmap_manager
	{
	public:
		png_bytep* rows;

		pixmap_manager(png_uint_32 height, size_t row_length)
			: rows(), h_(height), s_(row_length)
		{
			rows = new png_bytep[h_];
			for(size_t n = 0; n < h_; ++n) {
				rows[n] = new png_byte[s_];
			}
		}

		~pixmap_manager()
		{
			for(size_t n = 0; n < h_; ++n) {
				delete[] rows[n];
			}
			delete[] rows;
		}

	private:
		png_uint_32 h_;
		size_t s_;
	};

	class png_struct_manager
	{
	public:
		png_structp ctx;
		png_infop info;
		png_infop end_info;

		enum MODE { READ, WRITE };

		png_struct_manager(MODE m)
			: ctx(), info(), end_info(), m_(m)
		{
			if(m_ == READ) {
				ctx = png_create_read_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);
			}
			else {
				ctx = png_create_write_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);
			}

			if(!ctx) {
				throw std::bad_alloc();
			}

			info = png_create_info_struct(ctx);
			end_info = png_create_info_struct(ctx);

			if(!info || !end_info) {
				throw std::bad_alloc();
			}
		}

		~png_struct_manager()
		{
			if(m_ == READ) {
				png_destroy_read_struct(&ctx, &info, &end_info);
			}
			else {
				png_destroy_info_struct(ctx, &end_info);
				png_destroy_write_struct(&ctx, &info);
			}
		}

	private:
		MODE m_;
	};

	template<typename T, typename ReleasePolicy>
	class scoped_resource : boost::noncopyable
	{
		T rsrc_;

	public:
		typedef T				resource_type;
		typedef ReleasePolicy	release_type;

		scoped_resource(resource_type res = resource_type())
			: rsrc_(res)
		{}

		virtual ~scoped_resource() {
			release();
		}

		operator resource_type() const { return rsrc_; }

		resource_type get() const { return rsrc_; }
		resource_type operator->() const { return rsrc_; }

		void assign(const resource_type& o) {
			release();
			rsrc_ = o;
		}

	private:
		void release() { release_type()(rsrc_); }
	};

	struct stdio_fclose_policy {
		void operator()(std::FILE* f) const { if(f) { std::fclose(f); } }
	};

	typedef scoped_resource<FILE*, stdio_fclose_policy> scoped_file;
}

void rc_pixel(pixmap_manager& pixmap, png_uint_32 x, png_uint_32 y, const std::map<uint32_t, uint32_t>& cvt_map)
{
	uint32_t* src_ptr = reinterpret_cast<uint32_t*>(&pixmap.rows[y][x]);
	uint32_t src_color = *src_ptr;

	// libpng has ABGR, we want ARGB

	int a, r, g, b;
	a = src_color >> 24;
	r = src_color & 0x000000FF;
	g = src_color & 0x0000FF00;
	b = src_color & 0x00FF0000;

	uint32_t argb = (r << 16) + g + (b >> 16);

	std::map<uint32_t, uint32_t>::const_iterator cvt_entry = cvt_map.find(argb);
	if(cvt_entry != cvt_map.end()) {
		r = (cvt_entry->second & 0x00FF0000) >> 16;
		g = (cvt_entry->second & 0x0000FF00) >> 8;
		b = (cvt_entry->second & 0x000000FF);
		*src_ptr = (a << 24) + (b << 16) + (g << 8) + r;
		src_color = *src_ptr;
	}
}

bool rc_png(const std::string& input_file, const std::string& output_file, const std::map<uint32_t, uint32_t>& cvt_map)
{
	assert(input_file.length());
	assert(output_file.length());

	// Parts taken from the SDL_image library

	png_struct_manager r( png_struct_manager::READ );
	png_struct_manager w( png_struct_manager::WRITE );

	std::cout << "Recoloring: " << input_file << " -> " << output_file << "...\n";

	scoped_file in(fopen(input_file.c_str(), "rb"));

	if(!in) {
		std::cerr << "rc_png(): Could not open input file " << input_file << '\n';
		return false;
	}

	fseek(in, 0L, SEEK_SET);

	uint8_t signature[8];
	fread(signature, sizeof(uint8_t), 8, in);
	if(png_sig_cmp(reinterpret_cast<png_bytep>(signature), 0, 8)) {
		std::cerr << "rc_png(): Input file " << input_file << " is not a valid PNG file\n";
		return false;
	}

	png_uint_32 width, height;
	int row_length, depth, color_type, interlace_method, compression_method, filter_method;

#ifndef _WIN32
	//
	// FIXME: on Win32 builds we get this due to the pixmap_manager
	//        instantiation later in this code:
	//
	// > warning: variable ‘height’ might be clobbered by ‘longjmp’ or ‘vfor
	//
#if defined(PNG_SETJMP_SUPPORTED) && !defined(PNG_SETJMP_NOT_SUPPORTED)
	if(setjmp(png_jmpbuf(r.ctx))) {
		std::cerr << "rc_png(): libpng call aborted during read\n";
		return false;
	}

	if(setjmp(png_jmpbuf(w.ctx))) {
		std::cerr << "rc_png(): libpng call aborted during write\n";
		return false;
	}
#endif
#endif

	fseek(in, 0L, SEEK_SET);
	png_init_io(r.ctx, in);

	// Read header
	png_read_info(r.ctx, r.info);
	png_get_IHDR(r.ctx, r.info, &width, &height, &depth,
		&color_type, &interlace_method, &compression_method, &filter_method);

	// Tell libpng to strip 16 bit/color files down to 8 bits/color
	png_set_strip_16(r.ctx);
	// Extract multiple pixels with bit depths of 1, 2, and 4 from a single
	// byte into separate bytes (useful for paletted and grayscale images).
	png_set_packing(r.ctx);
	// scale greyscale values to the range 0..255
	if(color_type == PNG_COLOR_TYPE_GRAY) {
		png_set_expand(r.ctx);
	}
	// Handle color-key-transparency images
	if(png_get_valid(r.ctx, r.info, PNG_INFO_tRNS)) {
		png_set_tRNS_to_alpha(r.ctx);
	}
	if(color_type == PNG_COLOR_TYPE_GRAY || color_type == PNG_COLOR_TYPE_GRAY_ALPHA) {
		png_set_gray_to_rgb(r.ctx);
	}
	 if (color_type == PNG_COLOR_TYPE_PALETTE) {
		png_set_palette_to_rgb(r.ctx);
	}

	png_read_update_info(r.ctx, r.info);
	png_get_IHDR(r.ctx, r.info, &width, &height, &depth,
		&color_type, &interlace_method, &compression_method, &filter_method);
	row_length = png_get_rowbytes(r.ctx, r.info);

	pixmap_manager pixmap(height, row_length);

	// Read away now
	png_read_image(r.ctx, pixmap.rows);

	// Get info from the end of the stream
	// TODO: do something useful with date, comments, etc.
	png_read_end(r.ctx, r.end_info);

	for(png_uint_32 y = 0; y < height; ++y)  {
		for(png_uint_32 x = 0; x < width * sizeof(uint32_t); x += sizeof(uint32_t)) {
			rc_pixel(pixmap, x, y, cvt_map);
		}
	}

	scoped_file out(fopen(output_file.c_str(), "wb"));

	if(!out) {
		std::cerr << "rc_png(): Could not open or create output file " << output_file << '\n';
		return false;
	}

	fseek(out, 0L, SEEK_SET);
	png_init_io(w.ctx, out);

	png_set_IHDR(w.ctx, w.info, width, height, depth,
		color_type, interlace_method, compression_method, filter_method);

	// Write now
	png_write_info(w.ctx, w.info);
	png_write_image(w.ctx, pixmap.rows);
	png_write_end(w.ctx, w.end_info);

	return true;
}

bool recolor_image(const std::string& input_filename, const std::string& output_filename, const color_range& target_range, const std::vector<uint32_t>& key_colors)
{
	const std::map< uint32_t, uint32_t >& rc_map = recolor_range(target_range, key_colors);
	return rc_png(input_filename, output_filename, rc_map);
}

bool switch_image_palette(const std::string& input_filename, const std::string& output_filename, const std::vector<uint32_t>& target_colors, const std::vector<uint32_t>& key_colors)
{
	const size_t min_palette_length = std::min(key_colors.size(), target_colors.size());
	std::map< uint32_t, uint32_t > rc_map;
	for(size_t k = 0; k < min_palette_length; k++) {
		rc_map[key_colors[k]] = target_colors[k];
	}
	return rc_png(input_filename, output_filename, rc_map);
}
