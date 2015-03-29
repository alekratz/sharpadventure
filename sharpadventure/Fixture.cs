using System;
using System.Collections.Generic;
using NLua;

namespace sharpadventure
{
	public class Fixture
	{
		private string description = "";

		public string Name { get; set; }
		public string State { get; set; }
		public string StateVerb { get; set; }
		public string Description
		{ 
			get { return (description == "") ? States [State] : description; }
			set { description = value; }
		}
		public Dictionary<string, LuaFunction> Reactors { get; set; }
		public Dictionary<string, string> States { get; set; }

		public Fixture ()
		{
			States = new Dictionary<string, string> ();
			Reactors = new Dictionary<string, LuaFunction> ();
		}

		public override string ToString ()
		{
			return string.Format ("[Fixture: Name={0}, State={1}, StateVerb={2}, Description={3}, Reactors={4}, States={5}]", Name, State, StateVerb, Description, Reactors, States);
		}

		public static Fixture FromLuaTable(LuaTable table, ReactorList reactors)
		{
			Fixture fixture = new Fixture ();
			fixture.Name = table ["name"] as string;
			if (table ["description"] as string != null)
				fixture.Description = table ["description"] as string;
			fixture.StateVerb = (table ["state_verb"] as string != null) ? table ["state_verb"] as string : "is";
			if (table ["state"] as string != null)
				fixture.State = table ["state"] as string;
			if(table["states"] as LuaTable != null)
			{
				foreach (KeyValuePair<object, object> kvp in table["states"] as LuaTable)
				{
					if(kvp.Value as string != null)
						fixture.States.Add (kvp.Key as string, kvp.Value as string);
					else
					{
						Console.WriteLine ("UNIMPLEMENTED: Adding FixtureStates as tables (name: {0}, state: {1})", fixture.Name, kvp.Key as string);
					}
				}
			}
			if(table["reactors"] as LuaTable != null)
			{
				foreach (KeyValuePair<object, object> kvp in table["reactors"] as LuaTable)
				{
					string reactorKeyword = kvp.Key as string;
					LuaFunction reactor = kvp.Value as LuaFunction;
					if (reactor == null)
						Console.WriteLine ("WARNING: unknown reactor found in fixture (name: {0}, keyword: {1})", fixture.Name, reactorKeyword);
					else
						fixture.Reactors.Add (reactorKeyword, reactor);
				}
			}

			return fixture;
		}
	}
}

