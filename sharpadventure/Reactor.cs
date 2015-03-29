using System;
using System.Linq;
using System.Collections.Generic;
using NLua;

namespace sharpadventure
{
	public class Reactor
	{
		public LuaFunction Function { get; private set; }
		public string Name { get; private set; }
		public string[] Commands { get; private set; }

		public Reactor(LuaFunction function, string name, string[] commands)
		{
			Function = function;
			Name = name;
			Commands = commands;
		}
	}

	public class ReactorList : List<Reactor>
	{
		public ReactorList()
		{}

		public Reactor GetReactorByCommand(string command)
		{
			foreach(Reactor r in this)
			{
				if (r.Commands.Contains (command))
					return r;
			}
			return null;
		}

		public Reactor GetReactorByName(string name)
		{
			foreach(Reactor r in this)
			{
				if (r.Name == name)
					return r;
			}
			return null;
		}
	}
}

