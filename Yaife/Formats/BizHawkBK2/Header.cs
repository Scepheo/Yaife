using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yaife.Editors;
using JsonConverter = Yaife.Editors.JsonConverter;

namespace Yaife.Formats.BizHawkBK2
{
    public partial class Header
    {
        [Description("Version of the emulator used to record the movie."),
        Category("Movie File")]
        public string EmulatorVersion { get; set; }

        [Description("Version of the movie format."),
        Category("Movie File")]
        public string MovieVersion { get; set; }

        [Description("The game's platform."),
        Category("Game")]
        public string Platform { get; set; }

        [Description("The game's name."),
        Category("Game")]
        public string GameName { get; set; }

        [Description("Name of the movie's author."),
        Category("Movie File")]
        public string Author { get; set; }

        [Description("The amount of rerecords in the movie."),
        Category("Movie File")]
        public ulong Rerecords { get; set; }

        [Description("Whether the movie starts from a save state (True) or from powerup (False)."),
        Category("Movie File")]
        public bool StartsFromSavestate { get; set; }

        [Description("The savestate to start from."),
        Category("Movie File"),
        TypeConverter(typeof(HexByteConverter))]
        public byte[] BinarySavestate { get; set; }

        [Description("Whether this movie uses the NES Four Score."),
        Category("Movie File")]
        public bool FourScore { get; set; }

        [Description("Hash of the game's ROM."),
        Category("Game")]
        public string Hash { get; set; }

        [Description("Has of the firmware used."),
        Category("Game")]
        public string FirmwareHash { get; set; }

        [Description("Whether the movie is in PAL (True) or NTSC (False)."),
        Category("Game")]
        public bool Pal { get; set; }

        [Description("Name of the board used for switching memory banks."),
        Category("Game")]
        public string BoardName { get; set; }

        [Description("If this is set, playback will jump to this input frame upon reaching the end of the file."),
        Category("Movie File")]
        public int? LoopOffset { get; set; }

        [Description("The emulator core used to record/playback the movie."),
        Category("Configuration")]
        public string Core { get; set; }

        [Description("If applicable (N64), the video plugin used."),
        Category("Configuration")]
        public string VideoPlugin { get; set; }

        [Editor(typeof(CommentEditor), typeof(UITypeEditor)),
        TypeConverter(typeof(CommentConverter)),
        Description("Comments included with the movie file, these aren't used."),
        Category("Movie File")]
        public string[] Comments { get; set; }

        [Editor(typeof(PositionedSubtitleEditor), typeof(UITypeEditor)),
        TypeConverter(typeof(PositionedSubtitleConverter)),
        Description("Any text that will be displayed on screen during playback."),
        Category("Movie File")]
        public PositionedSubtitle[] Subtitles { get; set; }

        [Editor(typeof(JsonEditor), typeof(UITypeEditor)),
        TypeConverter(typeof(JsonConverter)),
        Description("Settings used by the emulator core that are relevant for movie sync."),
        Category("Configuration")]
        public JObject SyncSettings { get; set; }

        public Header(ZipArchive archive)
        {
            ReadHeader(archive);
            SyncSettings = ReadSyncSettings(archive);
            Subtitles = ReadSubtitles(archive);
            Comments = ReadComments(archive);
        }

        public void ReadHeader(ZipArchive archive)
        {
            var entry = archive.Entries.SingleOrDefault(
                    e => e.Name.StartsWith("Header", StringComparison.OrdinalIgnoreCase)
                    );

            if (entry == null)
                return;

            var file = new StreamReader(entry.Open());

            while (!file.EndOfStream)
            {
                var line = file.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var key = line.Substring(0, line.IndexOf(' '));
                var value = line.Substring(line.IndexOf(' ') + 1);

                switch (key)
                {
                    case HeaderKeys.Emulationversion:
                        EmulatorVersion = value;
                        break;
                    case HeaderKeys.Movieversion:
                        MovieVersion = value;
                        break;
                    case HeaderKeys.Platform:
                        Platform = value;
                        break;
                    case HeaderKeys.Gamename:
                        GameName = value;
                        break;
                    case HeaderKeys.Author:
                        Author = value;
                        break;
                    case HeaderKeys.Rerecords:
                        Rerecords = ulong.Parse(value);
                        break;
                    case HeaderKeys.Startsfromsavestate:
                        StartsFromSavestate = value == "1";
                        break;
                    case HeaderKeys.Savestatebinarybase64Blob:
                        BinarySavestate = Encoding.ASCII.GetBytes(value);
                        break;
                    case HeaderKeys.Fourscore:
                        FourScore = value == "1";
                        break;
                    case HeaderKeys.Sha1:
                        Hash = value;
                        break;
                    case HeaderKeys.Firmwaresha1:
                        FirmwareHash = value;
                        break;
                    case HeaderKeys.Pal:
                        Pal = value == "1";
                        break;
                    case HeaderKeys.Boardname:
                        BoardName = value;
                        break;
                    case HeaderKeys.Syncsettings:
                        SyncSettings = Deserialize(value);
                        break;
                    case HeaderKeys.Loopoffset:
                        LoopOffset = int.Parse(value);
                        break;
                    case HeaderKeys.Core:
                        Core = value;
                        break;
                    case HeaderKeys.Videoplugin:
                        VideoPlugin = value;
                        break;
                }
            }

            file.Close();
        }

        public void WriteHeader(ZipArchive archive)
        {
            WriteHeaderData(archive);
            WriteSubtitles(archive, Subtitles);
            WriteSyncSettings(archive, SyncSettings);
            WriteComments(archive, Comments);
        }

        public void WriteHeaderData(ZipArchive archive)
        {
            var existingEntry = archive.Entries.SingleOrDefault(
                e => e.Name.StartsWith("Header", StringComparison.OrdinalIgnoreCase)
            );

            var entry = existingEntry ?? archive.CreateEntry("Header.txt");

            var file = new StreamWriter(entry.Open());

            #region Native types
            file.WriteLine(HeaderKeys.Rerecords + " " + Rerecords);
            file.WriteLine(HeaderKeys.Startsfromsavestate + " " + StartsFromSavestate);
            file.WriteLine(HeaderKeys.Fourscore + " " + FourScore);
            file.WriteLine(HeaderKeys.Pal + " " + Pal);
            #endregion

            #region Strings
            if (!string.IsNullOrEmpty(EmulatorVersion))
                file.WriteLine(HeaderKeys.Emulationversion + " " + EmulatorVersion);

            if (!string.IsNullOrEmpty(MovieVersion))
                file.WriteLine(HeaderKeys.Movieversion + " " + MovieVersion);

            if (!string.IsNullOrEmpty(Platform))
                file.WriteLine(HeaderKeys.Platform + " " + Platform);

            if (!string.IsNullOrEmpty(GameName))
                file.WriteLine(HeaderKeys.Gamename + " " + GameName);

            if (!string.IsNullOrEmpty(Author))
                file.WriteLine(HeaderKeys.Author + " " + Author);
            
            if (!string.IsNullOrEmpty(Hash))
                file.WriteLine(HeaderKeys.Sha1 + " " + Hash);

            if (!string.IsNullOrEmpty(FirmwareHash))
                file.WriteLine(HeaderKeys.Firmwaresha1 + " " + FirmwareHash);
            
            if (!string.IsNullOrEmpty(BoardName))
                file.WriteLine(HeaderKeys.Boardname + " " + BoardName);

            if (!string.IsNullOrEmpty(Core))
                file.WriteLine(HeaderKeys.Core + " " + Core);

            if (!string.IsNullOrEmpty(VideoPlugin))
                file.WriteLine(HeaderKeys.Videoplugin + " " + VideoPlugin);
            #endregion
            
            #region Other nullables
            if (BinarySavestate != null)
                file.WriteLine(HeaderKeys.Savestatebinarybase64Blob + " " + Encoding.ASCII.GetString(BinarySavestate));

            if (LoopOffset.HasValue)
                file.WriteLine(HeaderKeys.Loopoffset + " " + LoopOffset);

            if (SyncSettings != null)
                file.WriteLine(HeaderKeys.Syncsettings + " " + JsonConvert.SerializeObject(SyncSettings));
            #endregion

            file.Flush();
            file.Close();
        }
    }

    public static class HeaderKeys
    {
        public const string Emulationversion          = "emuVersion";
        public const string Movieversion              = "MovieVersion";
        public const string Platform                  = "Platform";
        public const string Gamename                  = "GameName";
        public const string Author                    = "Author";
        public const string Rerecords                 = "rerecordCount";
        public const string Startsfromsavestate       = "StartsFromSavestate";
        public const string Savestatebinarybase64Blob = "SavestateBinaryBase64Blob";
        public const string Fourscore                 = "FourScore";
        public const string Sha1                      = "SHA1";
        public const string Firmwaresha1              = "FirmwareSHA1";
        public const string Pal                       = "PAL";
        public const string Boardname                 = "BoardName";
        public const string Syncsettings              = "SyncSettings";
        public const string Loopoffset                = "LoopOffset";
        public const string Core                      = "Core";
        public const string Videoplugin               = "VideoPlugin";
    }
}