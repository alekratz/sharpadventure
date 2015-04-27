using NUnit.Framework;
using System;

using sharpadventure;

namespace sharpadventuretest
{
	[TestFixture ()]
	public class StringUtilTest
	{
		[Test ()]
		public void TestStringPrev ()
		{
			string testString = "The quick brown fox jumped over the lazy dogs";
			Assert.AreEqual (StringUtil.StringPrev (testString, 0), '\0');
			for (int i = 1; i < testString.Length; i++)
				Assert.AreEqual (StringUtil.StringPrev (testString, i), testString [i - 1]);

			// Maybe more tests? idk I've never done this before
		}

		[Test ()]
		public void TestStringNext()
		{
			string testString = "The quick brown fox jumped over the lazy dogs";
			for (int i = 0; i < testString.Length - 1; i++)
				Assert.AreEqual (StringUtil.StringNext (testString, i), testString [i + 1]);
			Assert.AreEqual (StringUtil.StringNext (testString, testString.Length - 1), '\0');
		}

		// that is all I guess
	}
}

