﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NLua;

namespace sharpadventure
{
	public class Room
	{
		public string Name { get; private set; }
		public string ShortName { get; private set; }
		public string Description { get; private set; }
		public bool Start { get; private set; }
		public List<string> Exits { get; private set; }
		public Dictionary<string, Fixture> Fixtures { get; private set; }

		public Lua State { get; private set; }

		public Room(Lua luaState)
		{
			Name = "";
			ShortName = "";
			Description = "";
			Exits = new List<string> ();
			Fixtures = new Dictionary<string, Fixture> ();
			Start = false;
			State = luaState;
		}

		public Fixture GetFixture(string fixtureName)
		{
			return Fixtures.ContainsKey(fixtureName) ? Fixtures[fixtureName] : null;
		}

		public override string ToString ()
		{
			StringBuilder str = new StringBuilder ();
			str.AppendLine ("Name=" + Name);
			str.AppendLine ("ShortName=" + ShortName);
			str.AppendLine ("Description=" + Description);
			str.AppendLine ("Start=" + Start);
			str.AppendLine ("Exits=");
			foreach (string e in Exits)
				str.AppendLine ("\t" + e);
			str.AppendLine ("Fixtures=");
			foreach (KeyValuePair<string, Fixture> kvp in Fixtures)
				str.AppendLine ("\t" + kvp.Value.ToString());
			return str.ToString();
		}

		/// <summary>
		/// Gets the resolved version of the description for the room. Resolves any variable references.
		/// </summary>
		/// <returns>The resolved description.</returns>
		public string GetResolvedDescription()
		{
			return ResolveString (Description);
		}

		/// <summary>
		/// Takes some text and will fill in variables from the state in the string.
		/// </summary>
		/// <param name="text">Text.</param>
		public string ResolveString(string text)
		{
			string result = "";
			MatchCollection matches = Regex.Matches (text, @"\{\s*([^\d][^\s]*)\s*\}");
			int lastIndex = 0;
			foreach(Match m in matches)
			{
				string matchStr = m.Groups [1].ToString ();
				string varValue = (State [matchStr] != null) ? State [matchStr].ToString() : "";
				result += text.Substring (lastIndex, m.Index - lastIndex) + varValue;
				lastIndex = m.Index + m.Length;
			}
			result += text.Substring (lastIndex);
			return result;
		}

		public static Room Load(string resourceDirectory, string path)
		{
			Lua state = new Lua ();
			Room rm = new Room (state);
			// add the path to the "standard" functions to the package path search
			rm.State["package.path"] += ";" + resourceDirectory + "/?.lua";
			//rm.State ["util"] = LuaUtil.Instance;
			rm.State.LoadCLRPackage ();
			rm.State.DoString ("import('sharpadventure')");

			rm.State.DoFile (path);

			rm.Name = state ["name"] as string;
			rm.ShortName = state ["shortname"] as string;
			rm.Description = state ["description"] as string;
			if (rm.State ["exits"] as LuaTable != null)
			{
				foreach (KeyValuePair<object, object> kvp in (rm.State["exits"] as LuaTable))
				{
					string exit = kvp.Value as string;
					rm.Exits.Add (exit);
				}
			}
			rm.Start = (rm.State ["start"] == null) ? false : (bool)rm.State ["start"];
			LuaTable luaFixtures = rm.State["fixtures"] as LuaTable;
			if(luaFixtures != null)
			{
				foreach (KeyValuePair<object, object> kvp in luaFixtures)
				{
					Fixture fix = Fixture.FromLuaTable (kvp.Value as LuaTable);
					rm.Fixtures.Add (fix.Name, fix);
				}
				rm.State ["fixtures"] = rm.Fixtures;
			}

			return rm;
		}
	}
}

