namespace Yaife.Formats.BizHawkBK2
{
	public class Frame : IFrame
	{
		private string content;

		public Frame(string line)
		{
			this.content = line;
		}

		public string[] ToStrings()
		{
			return new string[] { content };
		}

		public void Parse(string[] strings)
		{
			this.content = strings[0];
		}
	}
}
