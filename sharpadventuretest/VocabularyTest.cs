using NUnit.Framework;
using System;

using sharpadventure.Language;

namespace sharpadventuretest
{
	[TestFixture ()]
	public class VocabularyTest
	{
		private Vocabulary vocab;

		public VocabularyTest()
		{
			Vocabulary.LoadInstance ("test_vocab.lua");
			vocab = Vocabulary.Instance;
		}

		[Test ()]
		public void TestLoad()
		{
			Assert.True (vocab.Prepositions.Contains ("at"));
			Assert.True (vocab.Prepositions.Contains ("with"));
			Assert.True (vocab.Prepositions.Contains ("inside"));
			Assert.True (vocab.Prepositions.Contains ("outside"));
			Assert.True (vocab.Prepositions.Contains ("in"));
			Assert.True (vocab.Prepositions.Contains ("out"));

			Assert.True (vocab.NegativeWords.Contains ("jk"));
			Assert.True (vocab.NegativeWords.Contains ("cancel"));
			Assert.True (vocab.NegativeWords.Contains ("nevermind"));
			Assert.True (vocab.NegativeWords.Contains ("none"));
			Assert.True (vocab.NegativeWords.Contains ("no"));

			Assert.True (vocab.Throwaways.Contains ("a"));
			Assert.True (vocab.Throwaways.Contains ("an"));
			Assert.True (vocab.Throwaways.Contains ("the"));
		}

		[Test ()]
		public void TestGetSynonym()
		{
			Assert.AreEqual (vocab.GetSynonym ("VIEW"), "LOOK");
			Assert.AreEqual (vocab.GetSynonym ("CHECK"), "LOOK");

			Assert.AreEqual (vocab.GetSynonym ("LEAVE"), "GO");

			Assert.AreEqual (vocab.GetSynonym ("SHUT"), "CLOSE");

			Assert.AreEqual (vocab.GetSynonym ("GET"), "TAKE");
			Assert.AreEqual (vocab.GetSynonym ("GRAB"), "TAKE");

			Assert.AreEqual (vocab.GetSynonym ("BREAK"), "BASH");
			Assert.AreEqual (vocab.GetSynonym ("FORCE"), "BASH");
			Assert.AreEqual (vocab.GetSynonym ("SMASH"), "BASH");
			Assert.AreEqual (vocab.GetSynonym ("DESTROY"), "BASH");
		}

		[Test ()]
		public void TestTrimThrowaways()
		{
			// do this one later
		}
	}
}

