using System;
using System.Collections.Generic;
using NLua;

namespace sharpadventure
{
	public class Fixture
	{
		private string description = "";
		private string inlineDescription = "";
		private LuaFunction descriptionFunction = null;

		public string Name { get; set; }
		public string State { get; set; }
		public string StateVerb { get; set; }
		public string Exit { get; set; }
		/// <summary>
		/// The pronoun used for this fixture, i.e. "it" or "them"
		/// </summary>
		/// <value>The pronoun.</value>
		public string Pronoun { get; set; }
		/// <summary>
		/// Text that is displayed when you (attempt) to take this fixture from the room.
		/// </summary>
		/// <value>The take text.</value>
		public string TakeText { get; set; }
		public bool Stuck { get; set; }
		/// <summary>
		/// Whether or not this fixture has been seen.
		/// </summary>
		/// <value><c>true</c> if seen; otherwise, <c>false</c>.</value>
		public bool Seen { get; set; }
		/// <summary>
		/// The optional description used as a convenience value for printing room descriptions.
		/// </summary>
		/// <value>The inline description.</value>
		public string InlineDescription 
		{ 
			get { return (Seen) ? inlineDescription : ""; } 
			set { inlineDescription = value; } 
		}
		/// <summary>
		/// The description that is used when directly looking at the fixture.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{ 
			get { return description; }
			set {  description = value; }
		}
		/// <summary>
		/// Extra, script-specific values.
		/// </summary>
		/// <value>The extra values specified by the script in the form of a LuaTable These are not used by the engine.</value>
		public LuaTable Extra { get; set; }
		public Dictionary<string, LuaFunction> Reactors { get; set; }
		public Dictionary<string, string> States { get; set; }

		public Fixture ()
		{
			States = new Dictionary<string, string> ();
			Reactors = new Dictionary<string, LuaFunction> ();
		}

		public string GetDescription(GameState state)
		{
			if (descriptionFunction != null)
				return descriptionFunction.Call (state) [0] as string;
			else
				return (description == "") ? States [State] : description; 
		}

		public override string ToString ()
		{
			//string.Format ("[Fixture: Name={0}, State={1}, StateVerb={2}, Description={3}, Reactors={4}, States={5}]", Name, State, StateVerb, Description, Reactors, States);
			return (Seen) ? string.Format ("{0} {1}", InlineDescription, Name) : Name;
		}

		public static Fixture FromLuaTable(LuaTable table)
		{
			Fixture fixture = new Fixture ();
			fixture.Name = table ["name"] as string; // required
			// description may be a function
			if (table ["description"] as LuaFunction != null)
				fixture.descriptionFunction = table ["description"] as LuaFunction;
			else
				fixture.description = (table ["description"] as string != null) ? table ["description"] as string : "";

			fixture.InlineDescription = (table["inline"] as string != null) ? table ["inline"] as string : "";
			fixture.StateVerb = (table ["state_verb"] as string != null) 	? table ["state_verb"] as string : "is";
			fixture.Exit = (table ["exit"] as string != null) 				? table ["exit"] as string : "";
			fixture.Pronoun = (table ["pronoun"] as string != null)		 	? table ["pronoun"] as string : "it";
			fixture.TakeText = (table ["taketext"] as string != null)		? table ["taketext"] as string : "";
			fixture.Extra = table ["extra"] as LuaTable;
			fixture.Stuck = (table ["stuck"] != null) 						? (bool)table ["stuck"] : false; // by default, a fixture is not "stuck", e.g., it can be removed from the room and put in player inventory.
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

