using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using Yaife.Editors;
using Yaife.FormatInterfaces;

namespace Yaife.Formats.BizHawkBK2
{
    public class Movie : IMovie, ISubtitled
    {
        public InputLog InputLog { get; private set; }
        public Header RealHeader;
        private string _logKey;

        public bool HasMenu => false;
        public ToolStripMenuItem GetMovieMenu(MovieTab tab) { return null; }

        public object Header => RealHeader;

        public string Path { get; set; }

        public string Description => "BizHawk 2.0";

        public string[] Extensions => new[] { ".bk2" };

        public void ReadFile(string path)
        {
            Path = path;

            using (var fileStream = new FileStream(path, FileMode.Open))
            using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                RealHeader = new Header(zipArchive);
                InputLog = InputLogIo.Read(zipArchive, out _logKey);
            }
        }

        public void WriteFile(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Update))
            {
                RealHeader.WriteHeader(zipArchive);
                InputLogIo.Write(zipArchive, InputLog, _logKey);
            }
        }

        public bool IsEmptyFrame(string[] frame)
        {
            return frame == null || frame[0].All(c => c == '.' || c == '|');
        }

        public Subtitle[] GetSubtitles()
        {
            return RealHeader.Subtitles.OfType<Subtitle>().ToArray();
        }
    }
}
