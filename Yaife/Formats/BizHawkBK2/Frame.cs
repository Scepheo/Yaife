namespace Yaife.Formats.BizHawkBK2
{
    public class Frame : IFrame
    {
        private string _content;

        public Frame(string line)
        {
            _content = line;
        }

        public object[] ToStrings()
        {
            return new[] { _content };
        }

        public void Parse(string[] strings)
        {
            _content = strings[0];
        }
    }
}
