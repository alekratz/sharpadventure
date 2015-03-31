using System;

namespace sharpadventure
{
	public class LuaUtil
	{
		private static LuaUtil instance;
		public static LuaUtil Instance
		{
			get
			{ 
				if (instance == null)
					instance = new LuaUtil ();
				return instance; 
			}
		}

		private LuaUtil ()
		{
		}

		public void Printc(string text)
		{
			StringUtil.WrapWriteLine (text);
		}
	}
}

