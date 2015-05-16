using NUnit.Framework;
using System;

using sharpadventure.Language;

namespace sharpadventuretest
{
	[TestFixture ()]
	public class LangUtilTest
	{
		[Test ()]
		public void TestMakeEnglishList()
		{
			string[][] lists = new string[][] {
				new string[] { "foo" },
				new string[] { "foo", "bar" },
				new string[] { "foo", "bar", "baz" },
				new string[] { "ham", "eggs", "milk", "cheese" },
				new string[] { "shave and a haircut", "two bits" },
			};

			Assert.AreEqual (LangUtil.MakeEnglishList(lists [0]), "foo");
			Assert.AreEqual (LangUtil.MakeEnglishList(lists [1]), "foo and bar");
			Assert.AreEqual (LangUtil.MakeEnglishList(lists [2]), "foo, bar, and baz");
			Assert.AreEqual (LangUtil.MakeEnglishList(lists [3]), "ham, eggs, milk, and cheese");
			Assert.AreEqual (LangUtil.MakeEnglishList (lists [4]), "shave and a haircut and two bits");
		}

		[Test ()]
		public void TestSplitPrepositions()
		{
			// can't do this one until we add a better way of loading the vocabulary
			//Assert.AreEqual()
		}
	}
}

