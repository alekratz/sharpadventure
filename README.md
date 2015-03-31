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

If this doesn't solve it for you, read on. The problem that you're most likely running into is NLua trying to make an external library call to `luanet_pushwstring`. NLua comes with its own distribution of the Lua library, with some .NET utlities and function calls built in to make things run a little smoother between the two frameworks. Basically, the `luanet_pushwstring` is a Windows utility, and `luanet_pushlstring` is a Linux utility. The NuGet packages pull the Windows version of the DLL, which has a compile-time directive to call either `luanet_pushlstring` or `luanet_pushwstring`... However, those are _compile_ time, and not runtime.

*tl;dr*, build LuaNet and install it yourself on Linux if you run into this problem.

Copyright
=
GPL v2
