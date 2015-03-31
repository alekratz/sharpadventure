using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NLua;

namespace sharpadventure
{
	public class GameState
	{
		public Vocabulary Vocab { get; private set; }
		public Dictionary<string, Room> Rooms { get; set; }
		public Room CurrentRoom { get; set; }
		public List<Fixture> Inventory { get; set; }
		public bool Running { get; set; }

		public GameState(string resourceDirectory)
		{
			Running = true;
			Vocab = new Vocabulary ();
			Rooms = new Dictionary<string, Room> ();
			CurrentRoom = null;
			Inventory = new List<Fixture> ();
			LoadGame (resourceDirectory);
		}

		private void LoadGame(string resourceDirectory)
		{
			//string reactorsPath = Path.Combine (resourceDirectory, "reactors.lua");
			string vocabPath = Path.Combine (resourceDirectory, "vocab.lua");
			string roomsPath = Path.Combine (resourceDirectory, "Rooms");

			// make sure that there exist reactors.lua and rooms/ as well
			if(!File.Exists(vocabPath))
				throw new Exception("Specified directory requires vocab.lua file (not found)");
			//if(!File.Exists(reactorsPath))
			//	throw new Exception("Specified directory requires reactors.lua file (not found)");
			if(!Directory.Exists(roomsPath))
				throw new Exception ("Specified directory requires rooms/ subdirectory (not found)");

			// Load all of the vocabularies
			Vocab.Load (vocabPath);

			// Load all of the rooms
			foreach(string roomPath in Directory.EnumerateFiles(roomsPath))
			{
				//Room rm = new Room(roomPath);
				Room rm = Room.Load (resourceDirectory, roomPath);
				Rooms.Add (rm.ShortName, rm);
			}

			// find the first room that is labelled as a starting room
			foreach(KeyValuePair<string, Room> kvp in Rooms)
			{
				if (kvp.Value.Start)
				{
					CurrentRoom = kvp.Value;
					break;
				}
			}

			if (CurrentRoom == null)
				throw new Exception ("No rooms are marked as starting rooms.");
		}

		public static void Main (string[] args)
		{
			if(args.Length == 0)
			{
				Console.WriteLine ("Usage: sharpadventure resource-directory [ resource-directory . . . ]");
				Environment.Exit (0);
			}
			Console.ForegroundColor = StringUtil.DEFAULT_COLOR;

			string resourceDirectory = args [0];

			if(!Directory.Exists(resourceDirectory))
			{
				Console.Error.WriteLine ("Could not find specified directory");
				Environment.Exit (1);
			}

			#if DEBUG
			GameState gameState = new GameState(resourceDirectory);
			#else
			GameState gameState = null;
			try
			{
				gameState = new GameState(resourceDirectory);
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine (ex.StackTrace);
				Console.Error.WriteLine ("Error: {0}", ex.Message);
				Environment.Exit (1);
			}
			#endif

			Game game = new Game ();
			game.Run (gameState);

			Console.WriteLine ("Quitter.");
			Console.ResetColor ();
		}
	}
}
