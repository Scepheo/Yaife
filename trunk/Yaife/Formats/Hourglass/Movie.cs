using System;
using System.IO;
using System.Windows.Forms;

namespace Yaife.Formats.Hourglass
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
			get { return "Hourglass"; }
		}

		public string[] Extensions
		{
			get { return new string[] { ".wtf", ".hgm" }; }
		}

		public void ReadFile(string path)
		{
			this.Path = path;

			var stream = new FileStream(path, FileMode.Open);
			this.RealHeader = new Header(stream);
			this.InputLog = InputLogIO.Read(stream);
			stream.Close();
		}

		public void WriteFile(string path)
		{
			var stream = new FileStream(path, FileMode.OpenOrCreate);
			this.RealHeader.Write(stream);
			InputLogIO.Write(stream, InputLog);
			stream.Close();
		}

		public bool IsEmptyFrame(string[] frame)
		{
			if (frame == null)
				return false;

			return String.IsNullOrEmpty(frame[0]);
		}

		public void SetFrameCount(uint count)
		{
			RealHeader.InputFrames = count;
		}
	}
}
