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
			string command, target, directObject, preposition;
			// man, I hope this works.

			// get the command and its synonym
			command = LangUtil.GetSynonym (contextLine.Split () [0]);
			// First, strip off the throwaways from the beginning and end
			string[] strippedLineParts = LangUtil.TrimThrowaways (contextLine.Split(' '), 1); // start at index 0, this is the entire command line
			string strippedLine = string.Join (" ", strippedLineParts);
			// get the number of... things
			// TODO : add context based on which command we're using
			string[] terms = LangUtil.SplitPrepositions (strippedLine);

			if (terms.Length > 3)
			{
				target = "";
				directObject = "";
				preposition = "";
			}
			else if (terms.Length == 3)
			{
				target = terms [0];
				preposition = terms [1];
				directObject = terms [2];
			} 
			else
			{
				target = terms [0];
				directObject = "";
				preposition = "";
			}

			return new SentenceParts (command, target, preposition, directObject);
		}
	}
}
