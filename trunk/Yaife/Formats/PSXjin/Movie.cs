using System.IO;
using System.Windows.Forms;

namespace Yaife.Formats.PSXjin
{
	public class Movie : IMovie, IFrameCount
	{
		public InputLog InputLog { get; private set; }

		public Header RealHeader;
		public object Header
		{
			get { return RealHeader; }
		}

		public bool HasMenu { get { return true; } }
		public ToolStripMenuItem GetMovieMenu(MovieTab tab)
		{
			return new Menu(tab);
		}

		public string Path { get; set; }

		public string Description
		{
			get { return "PSXjin"; }
		}

		public string[] Extensions
		{
			get { return new string[] { ".pjm" }; }
		}

		public void ReadFile(string path)
		{
			this.Path = path;

			var file = new FileStream(path, FileMode.Open);
			this.RealHeader = new Header();
			RealHeader.Read(file);

			file.Seek(RealHeader.ControllerDataOffset, SeekOrigin.Begin);
			InputLog = InputLogIO.Read(file, RealHeader.ControllerTypePort1, RealHeader.ControllerTypePort2, RealHeader.TextFormat);

			file.Close();
		}

		public void WriteFile(string path)
		{
			var file = new FileStream(path, FileMode.Create);

			RealHeader.Write(file);
			InputLogIO.Write(file, InputLog, RealHeader.TextFormat);

			file.Close();
		}

		public bool IsEmptyFrame(string[] strings)
		{
			var frame = new Frame();
			frame.Parse(strings);
			return frame.IsEmpty();
		}

		public void SetFrameCount(uint count)
		{
			RealHeader.FrameCount = count;
		}
	}
}
