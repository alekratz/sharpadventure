using NUnit.Framework;
using System;

using sharpadventure.Language;

namespace sharpadventuretest
{
	[TestFixture ()]
	public class VocabularyTest
	{
		[Test ()]
		public void TestLoad()
		{
			Vocabulary.LoadInstance ("test_vocab.lua");
		}
	}
}

