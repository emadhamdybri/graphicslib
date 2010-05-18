##
## Automake input file for /src/
## Process this file with automake to produce Makefile.in
##

noinst_HEADERS = \
	color_range.hpp \
	debug.hpp \
	fs.hpp \
	job.hpp \
	parser.hpp \
	rc_png.hpp \
	string_utils.hpp \
	util.hpp

bin_PROGRAMS = wesnoth-tc
wesnoth_tc_SOURCES =                                       \
	color_range.cpp                                        \
	debug.cpp                                              \
	fs.cpp                                                 \
	parser.cpp                                             \
	rc_png.cpp                                             \
	string_utils.cpp                                       \
	util.cpp                                               \
	wesnoth-tc.cpp
wesnoth_tc_LDADD = $(PNG_LIBS)

AM_CFLAGS = $(GD_CFLAGS) -include site_config.h
AM_CXXFLAGS = $(GD_CFLAGS) -include site_config.h
AM_LDFLAGS =

if STATIC
AM_LDFLAGS += -all-static
endif

if EXTRADEBUG
AM_CXXFLAGS += -D_GLIBCXX_DEBUG -D_GLIBCXX_DEBUG_PEDANTIC
endif
