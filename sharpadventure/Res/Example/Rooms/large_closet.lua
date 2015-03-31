require "reactors"

name = "a large closet"
shortname = "large_closet"
description = "A large, roomy closet. There is nothing terribly exciting in here. A #mop lays against the outer frame of the door."
exits = { "small_room" }

fixtures = {
	{
		name = "mop",
		description = "An average #mop. The faded letters on the side say 'Nacho Average Mop™'.",
		reactors = {
			TAKE = take_reactor_template("You pull the elastic waistband on your pants and drop the mop in there. " .. 
										 "You realize that this is not a Monkey Island game, and you put it in your inventory instead.")
		}
	}
}