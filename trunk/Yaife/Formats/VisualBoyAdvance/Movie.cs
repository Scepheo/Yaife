using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Yaife.Formats.VisualBoyAdvance
{
	public class Movie : IMovie, IFrameCount
	{
		public InputLog InputLog { get; private set; }

		public Header RealHeader;
		public object Header
		{
			get { return RealHeader; }
		}

		public bool HasMenu { get { return false; } }
		public ToolStripMenuItem GetMovieMenu(MovieTab tab) { return null; }

		public string Path { get; set; }

		public string Description
		{
			get { return "VisualBoyAdvance"; }
		}

		public string[] Extensions
		{
			get { return new string[] { ".vbm" }; }
		}

		public void ReadFile(string path)
		{
			this.Path = path;

			var file = new FileStream(path, FileMode.Open);
			this.RealHeader = new Header();
			RealHeader.Read(file);
			InputLog = InputLogIO.Read(file, RealHeader.ConnectedControllers);

			file.Close();
		}

		public void WriteFile(string path)
		{
			var file = new FileStream(path, FileMode.Create);

			RealHeader.Write(file);
			InputLogIO.Write(file, InputLog);

			file.Close();
		}

		public bool IsEmptyFrame(string[] strings)
		{
			var frame = new Frame();
			frame.Parse(strings);

			return frame.Controllers.All(b => b == GameboyButtons.Empty);
		}

		public void SetFrameCount(uint count)
		{
			RealHeader.FrameCount = count;
		}
	}
}
