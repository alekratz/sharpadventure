using System;
using System.Linq;

namespace sharpadventure.Language
{
	/// <summary>
	/// Reduces context. Duh
	/// </summary>
	public class ContextReducer
	{
		public GameState State { get; private set; }

		public ContextReducer (GameState state)
		{
			State = state;
		}

		/// <summary>
		/// Gets the context of a given line.
		/// </summary>
		/// <returns>A bunch of sentence parts that were inferred</returns>
		/// <param name="contextLine">Context line.</param>
		public SentenceParts GetContext(string contextLine)
		{
			// If it's not a valid command, return it
			if (string.IsNullOrEmpty (contextLine) || contextLine.Split (' ').Length == 0)
				return SentenceParts.None;
			string command, target, directObject;
			// man, I hope this works.

			// get the command and its synonym
			command = LangUtil.GetSynonym (contextLine.Split () [0]);
			// First, strip off the throwaways from the beginning and end
			string[] strippedLineParts = LangUtil.TrimThrowaways (contextLine.Split(' '), 1); // start at index 0, this is the entire command line
			string strippedLine = string.Join (" ", strippedLineParts);
			// get the number of... things
			// TODO : add context based on which command we're using
			// I definitely wrote this with the mentality of switch case, I'll fix it later
			string[] terms = LangUtil.SplitPrepositions (strippedLine);

			if (terms.Length > 2)
			{
				target = "";
				directObject = "";
			} 
			else if (terms.Length == 2)
			{
				target = terms [0];
				directObject = terms [1];
			} 
			else
			{
				target = terms [0];
				directObject = "";
			}

			return new SentenceParts (command, target, directObject);
		}
	}
}

