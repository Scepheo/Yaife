using System.IO;
using System.Windows.Forms;
using Yaife.FormatInterfaces;

namespace Yaife.Formats.PCSX
{
    public class Movie : IMovie, IFrameCount
    {
        public InputLog InputLog { get; set; }

        public Header RealHeader;
        public object Header => RealHeader;

        public bool HasMenu => false;
        public ToolStripMenuItem GetMovieMenu(MovieTab tab) { return null; }

        public string Path { get; set; }

        public string Description => "PCSX-rr";

        public string[] Extensions => new[] { ".pxm" };

        public void ReadFile(string path)
        {
            Path = path;

            var file = new FileStream(path, FileMode.Open);
            RealHeader = new Header();
            RealHeader.Read(file);
            InputLog = InputLogIo.Read(file, RealHeader.ControllerTypePort1, RealHeader.ControllerTypePort2);

            file.Close();
        }

        public void WriteFile(string path)
        {
            var file = new FileStream(path, FileMode.Create);

            RealHeader.Write(file);
            InputLogIo.Write(file, InputLog);

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
