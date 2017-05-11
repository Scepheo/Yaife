using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Yaife.Formats.BizHawkBK2
{
    public static class InputLogIo
    {
        public static InputLog Read(ZipArchive archive, out string logKey)
        {
            logKey = "";

            var entry = archive.Entries.SingleOrDefault(
                    e => e.Name.StartsWith("Input Log", StringComparison.OrdinalIgnoreCase)
                    );

            if (entry == null)
                return new InputLog();


            var log = new List<IFrame>();
            var file = new StreamReader(entry.Open());

            while (!file.EndOfStream)
            {
                var line = file.ReadLine();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line.StartsWith("LogKey:", StringComparison.OrdinalIgnoreCase))
                        logKey = line;
                    else
                        log.Add(new Frame(line));
                }
            }

            return new InputLog(new[] { "Input " }, log);
        }

        public static void Write(ZipArchive archive, InputLog log, string logKey)
        {
            var archiveEntry = archive.Entries.SingleOrDefault(
                e => e.Name.StartsWith("Input Log", StringComparison.OrdinalIgnoreCase)
            );

            var entry = archiveEntry ?? archive.CreateEntry("Input Log.txt");

            var file = new StreamWriter(entry.Open());

            file.WriteLine(logKey);

            foreach (var frame in log.Log)
                file.WriteLine(string.Join("", frame));

            file.Flush();
            file.Close();
        }
    }
}
