require "reactors"

name = "your room"
shortname = "your_room"
description = "There is a { fixtures.door.State } #door on the north end of the room. There are a few obscure #posters { fixtures.posters.InlineDescription }"
			.."on the walls. A{ fixtures.desk.InlineDescription } #desk lines the south wall. There are no windows{ fixtures.vent.InlineDescription }. Your "
			.."#bed is in the southeast corner next to the #(desk)."
start = true
exits = {}

local screwdriver = {
	name = "screwdriver", 
	description = "A flathead screwdriver. Fairly useful to have around, just in case.",
	reactors = {
		USE = function(state, owner, args) 
		print(args)
		end
	}
}

fixtures = {
	{
		name = "door",
		state = "locked",
		exit = "your_room",
		states = {
			locked = "The door to and from your room, which appears to be locked."
		},
		reactors = {
			OPEN = open_exit_reactor,
			CLOSE = close_reactor,
			UNLOCK = unlock_reactor,
			KNOCK = function(state, owner, args) StringUtil.WrapWriteLine("The door does not appear to respond to knocking.") end,
			BASH = function(state, owner, args) StringUtil.WrapWriteLine("Attempting to break down the door is not exactly in your best interests. It feels very solid, and if you managed to breach it, the door would not be the only thing you break.") end
		},
		stuck = true
	},
	{
		name = "posters",
		state_verb = "are",
		description = "Some posters, all of movies which look severely retro. In fact, you have a hard time distinguishing any of the movies, and you wonder if "
					.."any of them are for actual movies.",
		inline = "of some retro movies ",
		taketext = "Upon closer inspection, the posters are not actually posters at all, but are in fact paintings, all painted directly onto the wall. "
					.. "You wonder who was in charge of decorating this room.",
		stuck = true
	},
	{
		name = "desk",
		description = "A boring, empty desk with several #(desk drawers).",
		inline = "n empty",
		stuck = true
	},
	{
		name = "desk drawers",
		state_verb = "are",
		state = "closed",
		states = {
			closed = "Some closed drawers that are part of the #(desk).",
			open = "Some desk drawers that are opened, and may or may not contain items."
		},
		reactors = {
			OPEN = open_reactor,
			CLOSE = close_reactor,
		},
		contains = { 
			screwdriver,
			{ name = "bauble", description = "A small, nondescript bauble." }
		},
		stuck = true
	},
	{
		name = "vent",
		description = "A small air vent above the door in your room. You probably can't reach it.",
		inline = ", but there is a vent higher than you can visibly reach, above the door.",
		stuck = true
	},
	{
		name = "desk chair",
		state = "desk",
		states = {
			desk = 	"A boring, regular desk chair. It is sitting at the #desk and not doing much.",
			door = "A boring, regular desk chair. It is sitting by the #door and right below the #(vent)."
		},
		reactors = {
			MOVE = function(state, owner, args) 
				
			end
		}
	},
	{
		name = "windows",
		state_verb = "are",
		description = "You look through the window. Wait, no you don't. There's no windows in this room.",
		taketext = "You cannot take what does not exist.",
		stuck = true
	}
}