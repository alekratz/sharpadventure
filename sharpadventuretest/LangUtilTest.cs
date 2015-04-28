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
				new string[] { "ham", "eggs", "milk", "cheese" }
			};

			Assert.AreEqual (LangUtil.MakeEnglishList(lists [0]), "foo");
			Assert.AreEqual (LangUtil.MakeEnglishList(lists [1]), "foo and bar");
			Assert.AreEqual (LangUtil.MakeEnglishList(lists [2]), "foo, bar, and baz");
			Assert.AreEqual (LangUtil.MakeEnglishList(lists [3]), "ham, eggs, milk, and cheese");
		}
	}
}

