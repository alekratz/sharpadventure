require "reactors"

name = "A small room"
shortname = "small_room"
description = "This room is very small. There are some #cabinets on the wall."
exits = { "small_closet", "large_closet" }
start = true

fixtures = {
	{
		name = "wall",
		description = "A happy wall, with some #cabinets on it."
	},

	{
		name = "cabinets",
		state = "locked",
		state_verb = "are",
		states = {
			locked = "A set of sturdy wooden #cabinets. They are locked.",
			closed = "A set of sturdy wooden #cabinets. They are closed, and unlocked.",
			open = "A set of sturdy wooden #cabinets. They are wide open, showing you absolutely nothing. Great use of your time, there."
		},

		reactors = {
			OPEN = open_reactor,
			CLOSE = close_reactor,
			UNLOCK = unlock_reactor,
			BASH = function(state, owner, args) print("These cabinets are not suitable for destroying. Trust me.") end
		}
	}
}