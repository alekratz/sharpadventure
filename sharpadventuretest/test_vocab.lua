-- test_vocab.lua
-- Tests the vocabulary functionality of the game.

synonyms = {
	LOOK = { "VIEW", "CHECK" },
	GO = { "LEAVE" },
	CLOSE = { "SHUT" },
	TAKE = { "GET", "GRAB" },
	BASH = { "BREAK", "FORCE", "SMASH", "DESTROY" }
}

-- This is a test file, so we don't need to test too many things
throwaway = {
	"a",
	"an",
	"the"
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