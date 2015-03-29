using System;
using System.Text;
using System.Linq;
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
		private Dictionary<string, List<Fixture>> reactorFixtures;

		public Game ()
		{
			PredefinedCommands = new Dictionary<string, PredefinedCommand> ();

			PredefinedCommands.Add ("HELP", 
				(state, arg) =>
			{
				EpicWriteLine("In every room, you have the following commands available to you:");
				foreach(KeyValuePair<string, PredefinedCommand> kvp in PredefinedCommands)
					EpicWriteLine("  >> !{0}", kvp.Key);
				EpicWriteLine("You may also interact with the world around you. Look for colorized names in the rooms, and do things to them.");
				EpicWriteLine(@"
  Red is a reference to a !command
  Blue is a reference to a @person
  Green is a reference to a #thing
  Magenta is a reference to a $place
				");
				EpicWriteLine("For example, if you found some #doors, you would open them by typing");
				EpicWriteLine(" >> !OPEN #doors");
				EpicWriteLine("Or, if you ran into a locked #chest, you could either try your luck to !pick the lock, or just !bash it open.");
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
					EpicWriteLine(state.CurrentRoom.Description);
					PrintExits();
				}
				else
				{
					// TODO : hash search for the items
					Fixture fix = state.CurrentRoom.GetFixture(string.Join(" ", arg[1]));
					if(fix == null)
						EpicWriteLine("You strain your eyes looking for the {0} in the room, but it doesn't seem to exist.", arg[1]);
					else
						EpicWriteLine(fix.Description);
				}
			});
			PredefinedCommands.Add("GO",
				(state, arg) =>
			{
				if(arg.Length == 1)
				{
					EpicWriteLine("Where would you like to go?");
					PrintExits();
				}
				else
				{
					string target = String.Join(" ", arg, 1, arg.Length - 1).ToLower();
					// otherwise, make the string and check it against all exit names, and find which one the user meant
					List<string> matches = new List<string>();
					List<string> longNames = new List<string>();
					foreach(string shortName in gameState.CurrentRoom.Exits)
					{
						if(gameState.Rooms[shortName].Name.ToLower().Contains(target))
						{
							matches.Add(shortName);
							longNames.Add(gameState.Rooms[shortName].Name);
						}
					}
					if(matches.Count == 0)
						EpicWriteLine("There is no room that can be identified by '$({0}').", target);
					else if(matches.Count == 1)
						GoRoom(matches[0]);
					else
					{
						int choice = ChooseOne(longNames);
						if(choice < 0)
							return;
						GoRoom(matches[choice]);
					}
				}
			});
		}

		public void Run(GameState state)
		{
			gameState = state;
			GoRoom (gameState.CurrentRoom.ShortName);
			EpicWriteLine ("Obviously, if you need help at any time, type '!HELP' and press [RETURN].");

			do
			{
				EpicWriteLine ("What do you do?", gameState.CurrentRoom.Name);
				string line;
				do
				{
					line = GetLine();
				} while (line.Length == 0);

				string[] splitLine = line.Split(' ');
				// go through synonyms that are available
				string commandKeyword = GetBestCommand(splitLine[0].ToUpper());

				if(PredefinedCommands.ContainsKey(commandKeyword))
					PredefinedCommands[commandKeyword](gameState, splitLine);
				else if(reactorFixtures.ContainsKey(commandKeyword))
				{
					if(splitLine.Length == 1)
					{
						EpicWriteLine("What do you wish to !" + commandKeyword + "?");
						continue;
					}

					List<Fixture> fixList = reactorFixtures[commandKeyword];
					// Get the name of the first argument, aka the target
					// TODO : multiword targets
					string targetName = splitLine[1];

					// fixture doesn't exist in the room
					if(gameState.CurrentRoom.GetFixture(targetName) == null)
					{
						EpicWriteLine("You feel like a dunce, realizing you can't find the #(" + targetName + ") in the room.");
						continue;
					}

					Fixture target = null;
					foreach(Fixture fix in fixList)
						if(fix.Name == targetName) target = fix;
					// Fixture doesn't have the given action associated with it
					if(target == null)
					{
						EpicWriteLine("You can't !" + commandKeyword + " the #" + target.Name);
						continue;
					}
					// Make sure that the reactor isn't null
					LuaFunction reactor = target.Reactors[commandKeyword];
					Debug.Assert(reactor != null, "Could not find a reactor associated with " + target.Name + " in room " + gameState.CurrentRoom.ShortName);
					/*
					if(reactor == null)
					{
						EpicWriteLine("You can't !" + commandKeyword + " the #" + target.Name);
						continue;
					}
					*/
					try
					{
						reactor.Call(gameState, target, splitLine);
					}
					catch(NLua.Exceptions.LuaScriptException ex)
					{
						EpicWriteLine("ERROR in room {0}: {1}", gameState.CurrentRoom.ShortName, ex.Message);
					}
				}
				else
					EpicWriteLine("Command not found.");

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

		private int ChooseOne(List<string> items)
		{
			string response;
			int choice;
			do
			{
				EpicWriteLine ("Which of these did you mean? (negative response to cancel)");
				foreach(string item in items)
					EpicWriteLine ("  {0}", item);
				response = GetLine();
				if(gameState.NegativeWords.Contains(response))
				{
					EpicWriteLine("{0} to you, too.", response);
					return -1;
				}
			} while((choice = items.FindIndex(x => x.Contains(response))) == -1);
			return choice;
		}

		private void GoRoom(string shortName)
		{
			gameState.CurrentRoom = gameState.Rooms [shortName];
			reactorFixtures = ConstructRoomCommands(gameState.CurrentRoom);
			EpicWriteLine ("You are in {0}.", gameState.CurrentRoom.Name);
		}

		private void PrintExits()
		{
			int index = 1;
			EpicWriteLine ("Exits:");
			foreach(string shortName in gameState.CurrentRoom.Exits)
				EpicWriteLine ("{0}) $({1})", index++, gameState.Rooms [shortName].Name);
		}

		private static string GetLine()
		{
			Console.Write (StringUtil.Colorize(" !>> "));
			return Console.ReadLine ();
		}

		private static void EpicWriteLine(String text, params object[] args)
		{
			StringUtil.EpicWriteLine (text, args);
		}
	}
}