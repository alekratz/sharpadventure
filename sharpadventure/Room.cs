using System;
using System.Text;
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
		public List<Fixture> Fixtures { get; private set; }

		public Room()
		{
			Name = "";
			ShortName = "";
			Description = "";
			Exits = new List<string> ();
			Fixtures = new List<Fixture> ();
			Start = false;
		}

		public Fixture GetFixture(string fixtureName)
		{
			return Fixtures.FirstOrDefault ((fixture) => (fixture.Name == fixtureName));
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
			foreach (Fixture f in Fixtures)
				str.AppendLine ("\t" + f.ToString());
			return str.ToString();
		}

		public static Room Load(string resourceDirectory, string path, ReactorList reactors)
		{
			Room rm = new Room ();
			Lua state = new Lua ();
			// add the path to the "standard" functions to the package path search
			state["package.path"] += ";" + resourceDirectory + "/?.lua";
			//state.DoString ("print(package.path)");
			//state.DoString ("os.execute('pwd')");
			state.DoFile (path);

			rm.Name = state ["name"] as string;
			rm.ShortName = state ["shortname"] as string;
			rm.Description = state ["description"] as string;
			if (state ["exits"] as LuaTable != null)
			{
				foreach (KeyValuePair<object, object> kvp in (state["exits"] as LuaTable))
				{
					string exit = kvp.Value as string;
					rm.Exits.Add (exit);
				}
			}
			rm.Start = (state ["start"] == null) ? false : (bool)state ["start"];
			if(state["fixtures"] as LuaTable != null)
			{
				foreach (KeyValuePair<object, object> kvp in (state["fixtures"] as LuaTable))
					rm.Fixtures.Add (Fixture.FromLuaTable (kvp.Value as LuaTable, reactors));
			}

			return rm;
		}
	}
}

