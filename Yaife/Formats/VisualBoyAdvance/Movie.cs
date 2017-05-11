using System.IO;
using System.Linq;
using System.Windows.Forms;
using Yaife.FormatInterfaces;

namespace Yaife.Formats.VisualBoyAdvance
{
    public class Movie : IMovie, IFrameCount
    {
        public InputLog InputLog { get; private set; }

        public Header RealHeader;
        public object Header => RealHeader;

        public bool HasMenu => false;
        public ToolStripMenuItem GetMovieMenu(MovieTab tab) { return null; }

        public string Path { get; set; }

        public string Description => "VisualBoyAdvance";

        public string[] Extensions => new[] { ".vbm" };

        public void ReadFile(string path)
        {
            Path = path;

            var file = new FileStream(path, FileMode.Open);
            RealHeader = new Header();
            RealHeader.Read(file);
            InputLog = InputLogIo.Read(file, RealHeader.ConnectedControllers);

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

            return frame.Controllers.All(b => b == GameboyButtons.Empty);
        }

        public void SetFrameCount(uint count)
        {
            RealHeader.FrameCount = count;
        }
    }
}
