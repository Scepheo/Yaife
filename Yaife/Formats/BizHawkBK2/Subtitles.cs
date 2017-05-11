using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Yaife.Editors;

namespace Yaife.Formats.BizHawkBK2
{
    public partial class Header
    {
        public static PositionedSubtitle[] ReadSubtitles(ZipArchive archive)
        {
            var entry = archive.Entries.SingleOrDefault(
                    e => e.Name.StartsWith("Subtitles", StringComparison.OrdinalIgnoreCase)
                    );

            if (entry == null)
                return new PositionedSubtitle[0];

            var file = new StreamReader(entry.Open());

            var list = new List<PositionedSubtitle>();

            while (!file.EndOfStream)
            {
                var sub = ParseSubtitle(file.ReadLine());
                if (sub != null)
                    list.Add(sub);
            }

            var result = list.ToArray();
            file.Close();

            return result;
        }

        public static void WriteSubtitles(ZipArchive archive, PositionedSubtitle[] subtitles)
        {
            var archiveEntry = archive.Entries.SingleOrDefault(
                e => e.Name.StartsWith("Subtitles", StringComparison.OrdinalIgnoreCase)
            );

            var entry = archiveEntry ?? archive.CreateEntry("Subtitles.txt");

            var file = new StreamWriter(entry.Open());

            foreach (var sub in subtitles)
                if (sub != null)
                    file.WriteLine(string.Join(" ",
                        "subtitle",
                        sub.Frame,
                        sub.X,
                        sub.Y,
                        sub.Length,
                        sub.Color.ToArgb().ToString("X"),
                        sub.Text
                        ));

            file.Flush();
            file.Close();
        }

        private static PositionedSubtitle ParseSubtitle(string str)
        {
            var sub = new PositionedSubtitle();
            var split = str.Split(' ');

            if (!str.StartsWith("subtitle") || split.Length < 6)
                return null;

            sub.Text = string.Join(" ", split, 6, split.Length - 6);
            sub.Frame = int.Parse(split[1]);
            sub.X = int.Parse(split[2]);
            sub.Y = int.Parse(split[3]);
            sub.Length = int.Parse(split[4]);
            sub.Color = Color.FromArgb(int.Parse(split[5], NumberStyles.HexNumber));

            return sub;
        }
    }
}
