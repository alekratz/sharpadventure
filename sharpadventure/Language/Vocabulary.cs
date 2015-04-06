using System;
using System.Linq;
using System.Collections.Generic;
using NLua;

namespace sharpadventure.Language
{
	public class Vocabulary
	{
		public static Vocabulary Instance { get; private set; }
		public HashSet<string> Throwaways { get; private set; }
		public HashSet<string> NegativeWords { get; private set; }
		public Dictionary<string, HashSet<string>> Synonyms { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="sharpadventure.Vocab"/> class.
		/// </summary>
		/// <param name="scriptPath">Path to the vocab.lua for the game.</param>
		private Vocabulary ()
		{
			Throwaways = new HashSet<string> ();
			NegativeWords = new HashSet<string> ();
			Synonyms = new Dictionary<string, HashSet<string>> ();

		}

		public bool IsNegative(string word)
		{
			return NegativeWords.Contains (word);
		}

		public static void Load(string vocabPath)
		{
			Lua state = new Lua ();
			state.DoFile (vocabPath);
			Instance = new Vocabulary ();
			// Get synonyms
			if (state ["synonyms"] as LuaTable == null)
				Console.WriteLine ("WARNING: Synonyms table not found in vocab.lua. Vocabulary for actions will be restricted.");
			else
			{
				foreach (KeyValuePair<object, object> synPair in state["synonyms"] as LuaTable)
				{
					string command = synPair.Key as string;
					LuaTable synonymsTable = synPair.Value as LuaTable;
					if (synonymsTable == null)
						continue;
					HashSet<string> synonyms = new HashSet<string> ();
					foreach (KeyValuePair<object, object> kvp in synonymsTable)
						synonyms.Add (kvp.Value as string);

					Instance.Synonyms.Add (command, synonyms);
				}
			}

			if (state ["throwaway"] as LuaTable == null)
				Console.WriteLine ("WARNING: Throwaway table not found in vocab.lua.");
			else
			{
				foreach(KeyValuePair<object, object> throwawayPair in state["throwaway"] as LuaTable)
					Instance.Throwaways.Add (throwawayPair.Value as string);
			}

			if (state ["negative"] as LuaTable == null)
				Console.WriteLine ("WARNING: Negative table not found in vocab.lua.");
			else
			{
				foreach(KeyValuePair<object, object> negPair in state["negative"] as LuaTable)
					Instance.NegativeWords.Add (negPair.Value as string);
			}
		}

		/// <summary>
		/// Removes ther throwaway words from a string array, but only at the beginning and end as they appear.
		/// </summary>
		/// <param name="args">The arguments to trim from.</param>
		/// <param name="startIndex">The optional index to start at. Default is 0.</param>
		/// <returns>The string array without the throwaways at the beginning/end.</returns>
		public string[] TrimThrowaways(string[] args, int startIndex = 0)
		{
			// trim beginning
			int start = startIndex; // start/end indices
			for (; start < args.Length && Throwaways.Contains (args [start]); start++)
				;
			int end = args.Length - 1;
			for (; Throwaways.Contains (args [end]); end++)
				;
			// trim the array
			return args.SelectMany ((str, x) => (x < start || x > end) ? new string[0] : new string[] { str }).ToArray();
		}

		/// <summary>
		/// Selects the best synonym for the given word.
		/// </summary>
		/// <param name="word">The word to check synonyms against. This should be in upper case.</param>
		/// <returns>The synonym found, if there exists one; otherwise, the original word.</returns>
		public string GetSynonym(string word)
		{
			// go through each entry in the vocabulary, and check if it's a part of that
			foreach(KeyValuePair<string, HashSet<string>> kvp in Synonyms)
			{
				var command = kvp.Key;
				if (command == word)
					return word;
				var syns = kvp.Value;
				if (syns.Contains (word))
					return command;
			}
			return word;
		}
	}
}

