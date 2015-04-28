using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace sharpadventure
{
	public static class StringUtil
	{
		/*
		public const string NORM    = "\x1b[0m";

		public const string BLACK   = "\x1b[30m";
		public const string RED     = "\x1b[31m";
		public const string GREEN   = "\x1b[32m";
		public const string YELLOW  = "\x1b[33m";
		public const string BLUE    = "\x1b[34m";
		public const string MAGENTA = "\x1b[35m";
		public const string CYAN    = "\x1b[36m";
		public const string WHITE   = "\x1b[37m";

		public const string BLACK_BOLD   = "\x1b[30;1m";
		public const string RED_BOLD     = "\x1b[31;1m";
		public const string GREEN_BOLD   = "\x1b[32;1m";
		public const string YELLOW_BOLD  = "\x1b[33;1m";
		public const string BLUE_BOLD    = "\x1b[34;1m";
		public const string MAGENTA_BOLD = "\x1b[35;1m";
		public const string CYAN_BOLD    = "\x1b[36;1m";
		public const string WHITE_BOLD   = "\x1b[37;1m";

		public static string Colorize(string input)
		{
			string output = input;
			output = Regex.Replace (output, @"!\((.+)\)|!([^\s]+)", RED + "$1$2" + NORM);
			output = Regex.Replace (output, @"@\((.+)\)|@([^\s]+)", BLUE + "$1$2" + NORM);
			output = Regex.Replace (output, @"#\((.+)\)|#([^\s]+)", GREEN + "$1$2" + NORM);
			output = Regex.Replace (output, @"\$\((.+)\)|\$([^\s]+)", MAGENTA + "$1$2" + NORM);
			return output;
		}
		*/

		public const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;

		public static void WriteColor(string text)
		{
			/*
		 	 * ! is a reference to a command (red)
			 * @ is a reference to a person (blue)
			 * # is a reference to a thing (green)
			 * $ is a reference to a place (magenta)
			 * http://en.wikipedia.org/wiki/ANSI_escape_code#graphics for color reference
			 */
			Dictionary<char, ConsoleColor> colors = new Dictionary<char, ConsoleColor> {
				{ '!', ConsoleColor.Red },
				{ '@', ConsoleColor.Blue },
				{ '#', ConsoleColor.Green },
				{ '$', ConsoleColor.Magenta }
			};
			const char ESCAPE_CHAR = '~';

			string chunk = "";
			bool paren = false;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text [i], 
					 cnext = StringNext(text, i);
				if(c == ESCAPE_CHAR && (colors.ContainsKey (cnext) || cnext == ESCAPE_CHAR)) // backslash escapes
				{
					chunk += cnext;
					i++; // skip past the next character
				}
				else if (colors.ContainsKey (c) && cnext != ' ')
				{
					Console.Write (chunk);
					chunk = "";
					if (StringNext (text, i) == '(')
					{
						i++; // skip past the paren
						paren = true;
					}
					Console.ForegroundColor = colors [c];
				}
				else if (paren && c == ')')
				{
					paren = false;
					Console.ForegroundColor = DEFAULT_COLOR;
				} 
				else if (Console.ForegroundColor != DEFAULT_COLOR)
				{
					Console.Write (c);
					if (c == ' ' && !paren)
						Console.ForegroundColor = DEFAULT_COLOR;
				} else
					chunk += c;
			}
			Console.Write (chunk);
			Console.ForegroundColor = DEFAULT_COLOR;
		}

		public static void WriteLineColor(string text)
		{
			WriteColor (text);
			Console.WriteLine ();
		}

		public static char StringPrev(string text, int index)
		{
			Debug.Assert (!(index < 0));
			return (index == 0 || text.Length == 0) ? '\0' : text[index - 1];
		}

		public static char StringNext(string text, int index)
		{
			Debug.Assert (!(index >= text.Length));
			return (index < text.Length - 1) ? text [index + 1] : '\0';
		}

		public static void WrapWriteLine(String text, params object[] args)
		{
			text = String.Format (text, args);
			//text = StringUtil.Colorize (text);
			String[] words = text.Split(' ');
			StringBuilder buffer = new StringBuilder();

			foreach (String word in words)
			{
				buffer.Append(word);

				if (buffer.Length >= Console.BufferWidth)
				{
					String line = buffer.ToString().Substring(0, buffer.Length - word.Length);
					WriteLineColor(line);
					buffer.Clear();
					buffer.Append(word);
				}

				buffer.Append(" ");

			}

			WriteLineColor(buffer.ToString());
		}
			
	}
}

