using System;
using System.Collections.Generic;
using NLua;

namespace sharpadventure
{
	public class Vocabulary
	{
		public HashSet<string> NegativeWords { get; private set; }
		public Dictionary<string, HashSet<string>> Synonyms { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="sharpadventure.Vocab"/> class.
		/// </summary>
		/// <param name="scriptPath">Path to the vocab.lua for the game.</param>
		public Vocabulary ()
		{
			NegativeWords = new HashSet<string> ();
			Synonyms = new Dictionary<string, HashSet<string>> ();

		}

		public bool IsNegative(string word)
		{
			return NegativeWords.Contains (word);
		}

		public void Load(string vocabPath)
		{
			Lua state = new Lua ();
			state.DoFile (vocabPath);
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

					Synonyms.Add (command, synonyms);
				}
			}

			if (state ["negative"] as LuaTable == null)
				Console.WriteLine ("WARNING: Negative table not found in vocab.lua.");
			else
			{
				foreach(KeyValuePair<object, object> negPair in state["negative"] as LuaTable)
					NegativeWords.Add (negPair.Value as string);
			}
		}
	}
}

