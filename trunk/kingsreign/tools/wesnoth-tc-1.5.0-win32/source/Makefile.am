##
## Automake input file for /
## Process this file with automake to produce Makefile.in
##

ACLOCAL_AMFLAGS = -I m4 --install
SUBDIRS = src m4

EXTRA_DIST = \
	bootstrap \
	rc-magenta.png

dist_pkgdata_DATA = \
	share/tc_ranges.def

distclean-local:
	rm -f src/site_config.h
