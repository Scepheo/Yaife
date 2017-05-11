using System.IO;
using System.Windows.Forms;
using Yaife.FormatInterfaces;

namespace Yaife.Formats.Dolphin
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

        public string Description => "Dolphin";

        public string[] Extensions => new[] { ".dtm" };

        public void ReadFile(string path)
        {
            Path = path;
            var stream = new FileStream(path, FileMode.Open);

            RealHeader = new Header();
            RealHeader.Read(stream);

            var isWii = RealHeader.Controllers.HasFlag(Controller.WiiMote1);
            InputLog = InputLogIo.Read(stream, isWii);

            stream.Close();
        }

        public void WriteFile(string path)
        {
            var stream = new FileStream(path, FileMode.Create);
            RealHeader.Write(stream);

            var isWii = RealHeader.Controllers.HasFlag(Controller.WiiMote1);
            InputLogIo.Write(stream, InputLog, isWii);
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
