using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaife.Formats.Dolphin
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
			get { return "Dolphin"; }
		}

		public string[] Extensions
		{
			get { return new string[] { ".dtm" }; }
		}

		public void ReadFile(string path)
		{
			this.Path = path;
			var stream = new FileStream(path, FileMode.Open);

			RealHeader = new Header();
			RealHeader.Read(stream);

			var isWii = RealHeader.Controllers.HasFlag(Controller.WiiMote1);
			InputLog = InputLogIO.Read(stream, isWii);

			stream.Close();
		}

		public void WriteFile(string path)
		{
			var stream = new FileStream(path, FileMode.Create);
			RealHeader.Write(stream);

			var isWii = RealHeader.Controllers.HasFlag(Controller.WiiMote1);
			InputLogIO.Write(stream, InputLog, isWii);
			stream.Close();
		}

		public bool IsEmptyFrame(string[] str)
		{
			var frame = new GameCubeFrame();
			frame.Parse(str);
			return frame.IsEmpty();
		}

		public void SetFrameCount(uint count)
		{
			RealHeader.InputCount = count;
		}
	}
}
