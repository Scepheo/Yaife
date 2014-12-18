using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace Yaife.Formats.BizHawkBK2
{
	public class Movie : IMovie, ISubtitled
	{
		public InputLog InputLog { get; private set; }
		public Header RealHeader;
		private string logKey;

		public bool HasMenu { get { return false; } }
		public ToolStripMenuItem GetMovieMenu(MovieTab tab) { return null; }

		public object Header
		{
			get { return RealHeader; }
		}

		public string Path { get; set; }

		public string Description
		{
			get { return "BizHawk 2.0"; }
		}

		public string[] Extensions
		{
			get { return new string[] { ".bk2" }; }
		}

		public void ReadFile(string path)
		{
			Path = path;

			using (var fileStream = new FileStream(path, FileMode.Open))
			using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
			{
				this.RealHeader = new Header(zipArchive);
				this.InputLog = InputLogIO.Read(zipArchive, out logKey);
			}
		}

		public void WriteFile(string path)
		{
			using (var fileStream = new FileStream(path, FileMode.Create))
			using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Update))
			{
				this.RealHeader.WriteHeader(zipArchive);
				InputLogIO.Write(zipArchive, InputLog, logKey);
			}
		}

		public bool IsEmptyFrame(string[] frame)
		{
			if (frame == null)
				return true;

			return frame[0].All(c => c == '.' || c == '|');
		}

		public Subtitle[] GetSubtitles()
		{
			return RealHeader.Subtitles;
		}
	}
}
