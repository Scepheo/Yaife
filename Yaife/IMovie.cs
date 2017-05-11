using System.Windows.Forms;

namespace Yaife
{
    public interface IMovie
    {
        // Fields
        InputLog InputLog { get; }
        object Header { get; }

        // Control
        bool HasMenu { get; }
        ToolStripMenuItem GetMovieMenu(MovieTab tab);

        // Meta data
        string Path { get; set; }
        string Description { get; }
        string[] Extensions { get; }

        // File IO
        void ReadFile(string path);
        void WriteFile(string path);

        // All-format functionality
        bool IsEmptyFrame(string[] frame);
    }
}
