using System.IO;
using System.Windows.Forms;
using Yaife.FormatInterfaces;

namespace Yaife.Formats.PSXjin
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

        public string Description => "PSXjin";

        public string[] Extensions => new[] { ".pjm" };

        public void ReadFile(string path)
        {
            Path = path;

            var file = new FileStream(path, FileMode.Open);
            RealHeader = new Header();
            RealHeader.Read(file);

            file.Seek(RealHeader.ControllerDataOffset, SeekOrigin.Begin);
            InputLog = InputLogIo.Read(file, RealHeader.ControllerTypePort1, RealHeader.ControllerTypePort2, RealHeader.TextFormat);

            file.Close();
        }

        public void WriteFile(string path)
        {
            var file = new FileStream(path, FileMode.Create);

            RealHeader.Write(file);
            InputLogIo.Write(file, InputLog, RealHeader.TextFormat);

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
