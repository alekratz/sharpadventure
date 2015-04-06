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
	"at",
	"with",  -- maybe get rid of this one? or make a grammatical norm
	-- ex: 'with' must always be used as an adjective, e.g. beef with broccoli, and not an adverb, e.g. open door with key
	"inside", -- consider axing thing one too. if it comes up as an object name, then it may be removed.
	"in"
}

negative = {
	"jk",
	"cancel",
	"nevermind",
	"none",
	"no"
}