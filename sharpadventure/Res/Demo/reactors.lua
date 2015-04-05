-- reactors.lua
-- Reactors are lua functions that take three arguments: the game state, the owner, and the command line arguments.

-- open_reactor
-- reactor for the open state. Checks the following, exiting if failed:
-- 1. Is owner state locked?
-- 2. Is owner state open?
function open_reactor(state, owner, args)
  -- Check owner state
  if owner.State == "locked" then
    StringUtil.WrapWriteLine("The #({0}) {1} locked.", owner.Name, owner.StateVerb)
  elseif owner.State == "open" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #({0}) {1} already open.", owner.Name, owner.StateVerb)
  elseif owner.State == "closed" then
    StringUtil.WrapWriteLine("You open the #({0})", owner.Name)
    owner.State = "open"
  end
end

function unlock_reactor(state, owner, args)
  if owner.State == "locked" then
    --print("The {0} {1} locked. You need to find the proper key.")
    StringUtil.WrapWriteLine("You manage to unlock the #({0}). Somehow.", owner.Name)
    owner.State = "closed"
  elseif owner.State == "open" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #({0}) {1} already unlocked and wide open.", owner.Name, owner.StateVerb)
  elseif owner.State == "closed" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #({0}) {1} already unlocked.", owner.Name, owner.StateVerb)
  end
end

function close_reactor(state, owner, args)
  if owner.State == "locked" then
    --print("The {0} {1} locked. You need to find the proper key.")
    StringUtil.WrapWriteLine("The #({0}) {1} is already locked and closed.", owner.Name, owner.StateVerb)
  elseif owner.State == "open" then
    StringUtil.WrapWriteLine("You close the #({0}).", owner.Name)
  elseif owner.State == "closed" then
    StringUtil.WrapWriteLine("You feel like a dunce, as the #({0}) {1} already closed.", owner.Name, owner.StateVerb)
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