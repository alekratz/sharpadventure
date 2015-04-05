-- vocab.lua
-- common vocabulary things. Mostly for synonyms.

synonyms = {
	GO = { "LEAVE" },
	CLOSE = { "SHUT" },
	TAKE = { "GET", "GRAB" },
	BASH = { "BREAK", "FORCE", "SMASH", "DESTROY" }
}

throwaway = {
	"a",
	"an",
	"the",
	"at",
	"with"  -- maybe get rid of this one? or make a grammatical norm
	-- ex: 'with' must always be used as an adjective, e.g. beef with broccoli, and not an adverb, e.g. open door with key
}

negative = {
	"jk",
	"cancel",
	"nevermind",
	"none",
	"no"
}