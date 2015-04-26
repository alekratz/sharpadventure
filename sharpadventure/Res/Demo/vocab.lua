-- vocab.lua
-- common vocabulary things. Mostly for synonyms.

synonyms = {
	LOOK = { "VIEW", "CHECK" },
	GO = { "LEAVE" },
	CLOSE = { "SHUT" },
	TAKE = { "GET", "GRAB" },
	BASH = { "BREAK", "FORCE", "SMASH", "DESTROY" }
}

-- fixtures really shouldn't start with any of these words in the first place, I think.
throwaway = {
	"a",
	"an",
	"the",
	-- it's a good idea to have prepositions in the throwaways as well
	"at",
	"with",
	"inside",
	"outside",
	"in",
	"out"
	-- for example, "look at the door"
}

preposition = {
	"at",
	"with",
	"inside",
	"outside",
	"in",
	"out"
}

negative = {
	"jk",
	"cancel",
	"nevermind",
	"none",
	"no"
}