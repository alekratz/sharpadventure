-- Reactors are lua functions that take three arguments: the game state, the owner, and the command line arguments.

-- open_reactor
-- reactor for the open state. Checks the following, exiting if failed:
-- 1. Is owner state locked?
-- 2. Is owner state open?
function open_reactor(state, owner, args)
  -- Check owner state
  if owner.State == "locked" then
    print("The " .. owner.Name .. " " .. owner.StateVerb .. " locked.")
  elseif owner.State == "open" then
    print("You feel like a dunce, as the " .. owner.Name .. " " .. owner.StateVerb .. " already open.")
  elseif owner.State == "closed" then
    print("You open the " .. owner.Name)
    owner.State = "open"
  end
end

function unlock_reactor(state, owner, args)
  if owner.State == "locked" then
    --print("The " .. owner.Name .. " " .. owner.StateVerb .. " locked. You need to find the proper key.")
    print("You manage to unlock the " .. owner.Name .. ". Somehow.")
    owner.State = "closed"
  elseif owner.State == "open" then
    print("You feel like a dunce, as the " .. owner.Name .. " " .. owner.StateVerb .. " already unlocked and wide open.")
  elseif owner.State == "closed" then
    print("You feel like a dunce, as the " .. owner.Name .. " " .. owner.StateVerb .. " already unlocked.")
  end
end

function close_reactor(state, owner, args)
  print("You attempt to close the " .. owner.Name .. " to no avail.")
end