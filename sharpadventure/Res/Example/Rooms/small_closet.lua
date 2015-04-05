name = "a small closet"
shortname = "small_closet"
description = "A small, cramped closet for storing various wares, most of which are not of interest to you. " ..
			  "Various cleaning supplies are scattered about the $floor, and the $shelves are stocked with, well, more cleaning supplies."
exits = { "small_room" }

fixtures = {
	{
		name = "bucket",
		description = "A boring #bucket. That alliteration was way more interesting than this bucket is."
	},

	{
		name = "floor",
		description = function(state) 
			room = state.CurrentRoom
			desc = "The floor is ironically dusty and basically disgusting."
			if room:GetFixture("bucket") ~= nil then desc = desc .. " There is a #bucket on the floor." end
			desc = desc .. " There is nothing else terribly interesting in this closet, unless you really want some urinal cakes."
			return desc end,
		stuck = true
	}
}