using System;

namespace sharpadventure.Language
{
	public struct SentenceParts
	{
		public string Command { get; private set; }
		public string Target { get; private set; }
		public string Preposition { get; private set; }
		public string DirectObject { get; private set; }

		public SentenceParts(string command, string target, string preposition, string directObject)
			: this()
		{
			Command = command;
			Target = target;
			Preposition = preposition;
			DirectObject = directObject;
		}

		public static readonly SentenceParts None = new SentenceParts("", "", "", "");
	}
}

