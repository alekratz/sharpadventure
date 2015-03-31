sharpadventure
=
A dumb C# text adventure engine

Where we at
=
Right now, a lot of stuff works! Which is a good thing. Here's a list of the stuff that currently "works":
* Colorized output
* Lua configuration
* Lua vocab
* Rooms, fixtures, etc
* Setting states of fixtures via Lua
* String resolution for room variables
* A lot of built-in sass

However, there are a few things that need to be added. If they're marked with + then Ya Boy is working on it:
* +Player inventory
* Probably some code that isn't used anymore, that we don't need. Things like ReactorList
* Integration of Rant for some sentences at some point (future)
* +Demo game

If you have an improvement, please send a pull request, I'll look at it as soon as possible.

NLua woes
=
Nothing is perfect. NLua on Linux is a headache to work with, but we love it anyway. You may run into a "can't find lua52" library error. If that's the case, download this file and stick it in the executable directory.

http://www.mediafire.com/download/dqz9ahwvi6amyhn/liblua52.so

Copyright
=
GPL v2