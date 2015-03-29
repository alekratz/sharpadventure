using System;
using System.Collections.Generic;
using System.Diagnostics;
using NLua;

namespace sharpadventure
{
	public delegate void PredefinedCommand(GameState state, string[] args);

	public class Game
	{
		public Dictionary<string, PredefinedCommand> PredefinedCommands { get; private set; }

		private GameState gameState;

		public Game ()
		{
			PredefinedCommands = new Dictionary<string, PredefinedCommand> ();

			PredefinedCommands.Add ("HELP", 
				(state, arg) =>
			{
				Console.WriteLine("Available commands:");
				foreach(KeyValuePair<string, PredefinedCommand> kvp in PredefinedCommands)
					Console.WriteLine(StringUtil.Colorize("  >> !{0}"), kvp.Key);
				Console.WriteLine("You may also interact with the world around you. Look for colorized names in the rooms, and do things to them.");
				Console.WriteLine(StringUtil.Colorize(@"
  Red is a reference to a !command
  Blue is a reference to a @person
  Green is a reference to a #thing
  Magenta is a reference to a $place
				"));
				Console.WriteLine(StringUtil.Colorize("For example, if you found some #doors, you would open them by typing"));
				Console.WriteLine(StringUtil.Colorize(" >> !OPEN #doors"));
			});
			PredefinedCommands.Add("EXIT",
				(state, arg) =>
			{
				state.Running = false;
			});
			PredefinedCommands.Add ("LOOK",
				(state, arg) =>
			{
				if(arg.Length == 1)
				{
					Console.WriteLine(StringUtil.Colorize(state.CurrentRoom.Description));
					PrintExits();
				}
				else
				{
					Fixture fix = state.CurrentRoom.GetFixture(arg[1]);
					if(fix == null)
						Console.WriteLine("You strain your eyes looking for the {0} in the room, but it doesn't seem to exist.", arg[1]);
					else
						Console.WriteLine(StringUtil.Colorize(fix.Description));
				}
			});
			PredefinedCommands.Add("GO",
				(state, arg) =>
			{
				if(arg.Length == 1)
				{
					Console.WriteLine("Where would you like to go?");
					PrintExits();
				}
				else
				{
					string target = String.Join(" ", arg, 1, arg.Length - 1).ToLower();
					// otherwise, make the string and check it against all exit names, and find which one the user meant
					HashSet<string> matches = new HashSet<string>();
					foreach(string shortName in gameState.CurrentRoom.Exits)
					{
						if(gameState.Rooms[shortName].Name.ToLower().Contains(target))
							matches.Add(shortName);
					}
					Console.WriteLine("{0} {1}", matches.Count, (matches.Count == 1) ? "match" : "matches");
				}
			});
		}

		public void Run(GameState state)
		{
			gameState = state;
			Dictionary<string, List<Fixture>> reactorFixtures = ConstructRoomCommands(gameState.CurrentRoom);
			Console.WriteLine ("You find yourself in {0}. What do you do?", gameState.CurrentRoom.Name);
			Console.WriteLine (StringUtil.Colorize("Obviously, if you need help at any time, type '!HELP' and press [RETURN]."));

			do
			{
				string line = GetLine();
				if(line.Length == 0)
					continue;
				string[] splitLine = line.Split(' ');
				// go through synonyms that are available
				string commandKeyword = GetBestCommand(splitLine[0].ToUpper());

				if(PredefinedCommands.ContainsKey(commandKeyword))
					PredefinedCommands[splitLine[0].ToUpper()](gameState, splitLine);
				else if(reactorFixtures.ContainsKey(commandKeyword))
				{
					if(splitLine.Length == 1)
					{
						Console.WriteLine(StringUtil.Colorize("What do you wish to !" + commandKeyword + "?"));
						continue;
					}

					List<Fixture> fixList = reactorFixtures[commandKeyword];
					// Get the name of the first argument, aka the target
					string targetName = splitLine[1];

					// fixture doesn't exist in the room
					if(gameState.CurrentRoom.GetFixture(targetName) == null)
					{
						Console.WriteLine(StringUtil.Colorize("You feel like a dunce, realizing you can't find the #" + targetName + " in the room."));
						continue;
					}

					Fixture target = null;
					foreach(Fixture fix in fixList)
						if(fix.Name == targetName) target = fix;
					// Fixture doesn't have the given action associated with it
					if(target == null)
					{
						Console.WriteLine("You can't !" + commandKeyword + " the #" + target.Name);
						continue;
					}
					// Make sure that the reactor isn't null
					LuaFunction reactor = target.Reactors[commandKeyword];
					Debug.Assert(reactor != null, "Could not find a reactor associated with " + target.Name + " in room " + gameState.CurrentRoom.ShortName);
					/*
					if(reactor == null)
					{
						Console.WriteLine("You can't !" + commandKeyword + " the #" + target.Name);
						continue;
					}
					*/
					try
					{
						reactor.Call(gameState, target, splitLine);
					}
					catch(NLua.Exceptions.LuaScriptException ex)
					{
						Console.WriteLine("ERROR in room {0}: {1}", gameState.CurrentRoom.ShortName, ex.Message);
					}
				}
				else
					Console.WriteLine("Command not found.");

			} while(gameState.Running);
		}

		/// <summary>
		/// Creates a dictionary of lists of fixtures, all indexed by the commands that affect them.
		/// </summary>
		/// <returns>The room commands.</returns>
		/// <param name="room">Room.</param>
		private Dictionary<string, List<Fixture>> ConstructRoomCommands(Room room)
		{
			Dictionary<string, List<Fixture>> commands = new Dictionary<string, List<Fixture>> ();
			// iterate over all fixtures in the room
			foreach(Fixture fixture in room.Fixtures)
			{
				// iterate over all reactors that affect this fixture
				foreach(string command in fixture.Reactors.Keys)
				{
					// if there's not already this command in the list, then create a new list for that command.
					if (!commands.ContainsKey (command))
						commands [command] = new List<Fixture> ();
					commands [command].Add (fixture);
				}
			}
			return commands;
		}

		private string GetBestCommand(string commandKeyword)
		{
			// go through each entry in the vocabulary, and check if it's a part of that
			foreach(KeyValuePair<string, HashSet<string>> kvp in gameState.Synonyms)
			{
				var command = kvp.Key;
				if (command == commandKeyword)
					return commandKeyword;
				var syns = kvp.Value;
				if (syns.Contains (commandKeyword))
					return command;
			}
			return commandKeyword;
		}

		private void PrintExits()
		{
			int index = 1;
			Console.WriteLine ("Exits:");
			foreach(string shortName in gameState.CurrentRoom.Exits)
				Console.WriteLine ("{0}) {1}", index++, gameState.Rooms [shortName].Name);
		}

		static string GetLine()
		{
			Console.Write (StringUtil.Colorize(" !>> "));
			return Console.ReadLine ();
		}
	}
}