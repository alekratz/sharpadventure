-- reactors.lua
-- Reactors are lua functions that take three arguments: the game state, the owner, and the command line arguments.

-- open_reactor
-- reactor for the open state. Checks the following, exiting if failed:
-- 1. Is owner state locked?
-- 2. Is owner state open?
function open_reactor(state, owner, args)
  -- Check owner state
  if owner.State == "locked" then
    StringUtil.WrapWriteLine("The #(" .. owner.Name .. ") " .. owner.StateVerb .. " locked.")
  elseif owner.State == "open" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #(" .. owner.Name .. ") " .. owner.StateVerb .. " already open.")
  elseif owner.State == "closed" then
    StringUtil.WrapWriteLine("You open the #(" .. owner.Name .. ").")
    owner.State = "open"
  end
end

function open_exit_reactor_template(exit)
  return function(state, owner, args)
	  assert(exit ~= nil) -- make sure that the exit is defined
	  room = state.CurrentRoom
	  if owner.State == "locked" then
	    StringUtil.WrapWriteLine("The #(" .. owner.Name .. ") " .. owner.StateVerb .. " locked.")
	  elseif owner.State == "open" then
	    StringUtil.WrapWriteLine("You feel like a dunce, as the #(" .. owner.Name .. ") " .. owner.StateVerb .. " already open.")
	  elseif owner.State == "closed" then
		if not room.Exits:Contains(exit) then
			-- TODO : get exit long name
			StringUtil.WrapWriteLine("You open the #(" .. owner.Name .. "), to reveal a new exit: $(" .. exit .. ")")
			room.Exits:Add(exit)
		else
	    	StringUtil.WrapWriteLine("You open the #(" .. owner.Name .. ").")
	    end
	    owner.State = "open"
	  end
  end
end

function unlock_reactor(state, owner, args)
  if owner.State == "locked" then
    --print("The " .. owner.Name .. " " .. owner.StateVerb .. " locked. You need to find the proper key.")
    StringUtil.WrapWriteLine("You manage to unlock the #(" .. owner.Name .. "). Somehow.")
    owner.State = "closed"
  elseif owner.State == "open" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #(" .. owner.Name .. ") " .. owner.StateVerb .. " already unlocked and wide open.")
  elseif owner.State == "closed" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #(" .. owner.Name .. ") " .. owner.StateVerb .. " already unlocked.")
  end
end

function close_reactor(state, owner, args)
  if owner.State == "locked" then
    --print("The " .. owner.Name .. " " .. owner.StateVerb .. " locked. You need to find the proper key.")
    StringUtil.WrapWriteLine("The #(" .. owner.Name .. ") " .. owner.StateVerb .. " is already locked and closed.")
  elseif owner.State == "open" then
    StringUtil.WrapWriteLine("You close the #(" .. owner.Name .. ").")
  elseif owner.State == "closed" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #(" .. owner.Name .. ") " .. owner.StateVerb .. " already closed.")
  end
end

-- This is not a reactor, it is a reactor template. You pass in arguments, and it will create a reactor
-- whose behavior is based on those arguments.
function take_reactor_template(take_string)
  return function(state, owner, args)
  	-- TODO : take the thing out of the got dang room
  	StringUtil.WrapWriteLine(take_string)
  end
end

function bash_reactor_template(bash_string)
  return function(state, owner, args)
  	StringUtil.WrapWriteLine(bash_string)
  end
end