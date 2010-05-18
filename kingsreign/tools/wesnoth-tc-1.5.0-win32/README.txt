Wesnoth Team Colorizer README
-----------------------------

Copyright (C) 2008, 2009, 2010 by Ignacio R. Morelle <shadowm@wesnoth.org>.
Portions Copyright (C) 2003-2008 by the Battle for Wesnoth Project
<http://www.wesnoth.org/>.


This program is intended to help BfW artists to test their team-colored sprites
without having to either hack their unit files and start a 9-player multiplayer
game, or just assuming that their TC will look good in-game in any of the
available mainline color ranges (red, blue, etc.).

It can also help test team flag graphics, and possibly anything else that can
be recolored with the game's algorithm.

To build and install this software, read the INSTALL file.


1. Usage

You must specify a space-separated list of files to colorize at the end of the
command line. Each of them will be processed with the same options, which by
default means that every of them will be copied and colorized to the default
player colors in mainline, which are red, blue, green, purple, orange, black,
white, brown and teal (as of Wesnoth 1.5). For instance, using the following:

$ wesnoth-tc gnome-wizard.png gnome-warrior.png

Will produce the following files:

gnome-wizard-TC-magenta-1.png         gnome-warrior-TC-magenta-1.png
gnome-wizard-TC-magenta-2.png         gnome-warrior-TC-magenta-2.png
gnome-wizard-TC-magenta-3.png         gnome-warrior-TC-magenta-3.png
                        ...
gnome-wizard-TC-magenta-9.png         gnome-warrior-TC-magenta-9.png

Note that the default player numbers that would be assigned those colors are
used to name the output files in this case. That's normal, because the author
decided it was more intuitive, both for the artist and their favorite file
manager. :-)

BEWARE: If files with the same name of the output files exist,
        they will be overwritten. It is very unlikely that you have files
        named that way, though, unless they are just old wesnoth-tc output.

You can fine-tune the operations performed by wesnoth-tc, see section 3
for details.



2. Numeric aliases

By default, the following range identifiers are equivalent to the following
numbers, according to Wesnoth's default choice of color per player number:

Numeric   Common name
   1          red
   2         blue
   3         green
   4        purple
   5        orange
   6         black
   7         white
   8         brown
   9         teal

New aliases can be defined, see the inline documentation on the tc_ranges.def
file.



3. Commandline options

    wesnoth-tc [options] input_files

Options:

--key <palette name>
     Specifies a different key palette than 'magenta' to be used for recoloring
     the input files. The specified palette must be declared in the RC
     definition file (see --def).

--rc, --tc <ranges list>
     Specifies a comma-separated list of team-color ranges to be used.
     By default, the team-colorizer will use the default (1,2,3,4,5,6,7,8,9).
     The specified ranges *must* be declared in the RC definition file (see
     --def). Output files have the "-RC-<key>-<range id>" suffix.

--pal <palette list>
     Specifies a comma-separated list of color palettes to replace pixels
     matching the 'key' palette. The target palettes must be declared in the RC
     definition file. If the target or key palettes aren't all of the same size,
     then all of them are truncated by the lowest size. Output files have the
     "-PAL-<key>-<palette id>" suffix.

--def <filename>
     Specifies a RC definition file to be used instead of the default
     './tc_ranges.def'.

--debug
     Increases output verbosity for testing or debugging.

Please note that the palette names and range identifiers are case sensitive.
This means that

$ wesnoth-tc --tc red doomguy.png

Is NOT the same as:

$ wesnoth-tc --tc Red doomguy.png

The default provided palettes and color ranges have lowercase identifiers to
ease your life, so with them, the last command line above would not work at all.



4. Palettes and color ranges

This version of wesnoth-tc comes with all the predefined palettes and color
ranges found in Wesnoth 1.4. Those are:

Palettes:
	magenta     - Regular unit TC palette.
	flag_green  - Team/village flag palette.
	ellipse_red - Regular unit ellipse recoloring palette.
	rc_blue_pal - Blueish recoloring palette, unused in mainline.

Color ranges:
	red         - Player #1
	blue        - Player #2
	green       - Player #3
	purple      - Player #4
	orange      - Player #5
	black       - Player #6
	white       - Player #7
	brown       - Player #8
	teal        - Player #9
	lightred    - Extra color range, intended for special units
	darkred     - Extra color range, intended for special units

New palettes and color ranges can be defined, see the inline documentation on
the tc_ranges.def file.



5. Contacting

Bugs, complaints, wishes and contributions should be sent to Ignacio R. Morelle
<shadowm@wesnoth.org>.

Have fun!
