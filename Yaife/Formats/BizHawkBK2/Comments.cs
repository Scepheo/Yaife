using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Yaife.Formats.BizHawkBK2
{
    public partial class Header
    {
        public static string[] ReadComments(ZipArchive archive)
        {
            var entry = archive.Entries.SingleOrDefault(
                    e => e.Name.StartsWith("Comments", StringComparison.OrdinalIgnoreCase)
                    );

            if (entry == null)
                return new string[0];

            var file = new StreamReader(entry.Open());

            var list = new List<string>();

            while (!file.EndOfStream)
            {
                var line = file.ReadLine();

                if (line.StartsWith("comment ", StringComparison.OrdinalIgnoreCase))
                {
                    var comment = line.Substring(line.IndexOf(' '));
                    list.Add(comment);
                }
            }

            var result = list.ToArray();
            file.Close();

            return result;
        }

        public static void WriteComments(ZipArchive archive, string[] comments)
        {
            var archiveEntry = archive.Entries.SingleOrDefault(
                e => e.Name.StartsWith("Comments", StringComparison.OrdinalIgnoreCase)
            );
            
            var entry = archiveEntry ?? archive.CreateEntry("Comments.txt");

            var file = new StreamWriter(entry.Open());

            foreach (var comment in comments)
                if (comment != null)
                    file.WriteLine("comment " + comment);

            file.Flush();
            file.Close();
        }
    }
}
