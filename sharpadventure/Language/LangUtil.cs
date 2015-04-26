using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace sharpadventure.Language
{
	public static class LangUtil
	{
		public static readonly Vocabulary VocabInstance = Vocabulary.Instance;

		/// <summary>
		/// Removes ther throwaway words from a string array, but only at the beginning and end as they appear.
		/// </summary>
		/// <param name="args">The arguments to trim from.</param>
		/// <param name="startIndex">The optional index to start at. Default is 0.</param>
		/// <returns>The string array without the throwaways at the beginning/end.</returns>
		public static string[] TrimThrowaways(string[] args, int startIndex = 0)
		{
			return VocabInstance.TrimThrowaways (args, startIndex);
		}

		/// <summary>
		/// Selects the best synonym for the given word.
		/// </summary>
		/// <param name="word">The word to check synonyms against. This should be in upper case.</param>
		/// <returns>The synonym found, if there exists one; otherwise, the original word.</returns>
		public static string GetSynonym(string word)
		{
			return VocabInstance.GetSynonym (word);
		}


		public static string[] SplitPrepositions(string sentence)
		{
			// get the first occurrence of a preposition
			// TODO : make this better. deduce stuff. figure out possibilities.
			// TODO : Maybe move this to the contextreducer to find the /best/ option?
			// HACK : lol, it just kinda splits things using the prepositions array

			return sentence.Split (VocabInstance.Prepositions.ToArray(), StringSplitOptions.RemoveEmptyEntries);
		}

		public static string MakeEnglishList(string[] list)
		{
			Debug.Assert (list.Length != 0, "MakeEnglishList list should have length of at least 1.");
			if (list.Length == 1)
				return list [0];
			else if (list.Length == 2)
				return string.Format ("{0} and {1}", list [0], list [1]);
			else
			{
				string result = list[0];
				for (int i = 1; i < list.Length - 1; i++)
					result += ", " + list [i];
				result += ", and " + list.Last ();
				return result;
			}
		}
	}
}

