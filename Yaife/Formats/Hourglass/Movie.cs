using System;
using System.IO;
using System.Windows.Forms;
using Yaife.FormatInterfaces;

namespace Yaife.Formats.Hourglass
{
    public class Movie : IMovie, IFrameCount
    {
        public InputLog InputLog { get; private set; }

        public Header RealHeader;

        public object Header => RealHeader;

        public bool HasMenu => true;

        public ToolStripMenuItem GetMovieMenu(MovieTab tab)
        {
            return new Menu(tab);
        }

        public string Path { get; set; }

        public string Description => "Hourglass";

        public string[] Extensions => new[] { ".wtf", ".hgm" };

        public void ReadFile(string path)
        {
            Path = path;

            var stream = new FileStream(path, FileMode.Open);
            RealHeader = new Header(stream);
            InputLog = InputLogIo.Read(stream);
            stream.Close();
        }

        public void WriteFile(string path)
        {
            var stream = new FileStream(path, FileMode.OpenOrCreate);
            RealHeader.Write(stream);
            InputLogIo.Write(stream, InputLog);
            stream.Close();
        }

        public bool IsEmptyFrame(string[] frame)
        {
            return frame != null && string.IsNullOrEmpty(frame[0]);
        }

        public void SetFrameCount(uint count)
        {
            RealHeader.InputFrames = count;
        }
    }
}
