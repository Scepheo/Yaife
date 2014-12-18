using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
			this.SyncSettings = ReadSyncSettings(archive);
			this.Subtitles = ReadSubtitles(archive);
			this.Comments = ReadComments(archive);
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

				if (String.IsNullOrWhiteSpace(line))
					continue;

				var key = line.Substring(0, line.IndexOf(' '));
				var value = line.Substring(line.IndexOf(' ') + 1);

				switch (key)
				{
					case HeaderKeys.EMULATIONVERSION:
						this.EmulatorVersion = value;
						break;
					case HeaderKeys.MOVIEVERSION:
						this.MovieVersion = value;
						break;
					case HeaderKeys.PLATFORM:
						this.Platform = value;
						break;
					case HeaderKeys.GAMENAME:
						this.GameName = value;
						break;
					case HeaderKeys.AUTHOR:
						this.Author = value;
						break;
					case HeaderKeys.RERECORDS:
						this.Rerecords = ulong.Parse(value);
						break;
					case HeaderKeys.STARTSFROMSAVESTATE:
						this.StartsFromSavestate = (value == "1");
						break;
					case HeaderKeys.SAVESTATEBINARYBASE64BLOB:
						this.BinarySavestate = Encoding.ASCII.GetBytes(value);
						break;
					case HeaderKeys.FOURSCORE:
						this.FourScore = (value == "1");
						break;
					case HeaderKeys.SHA1:
						this.Hash = value;
						break;
					case HeaderKeys.FIRMWARESHA1:
						this.FirmwareHash = value;
						break;
					case HeaderKeys.PAL:
						this.Pal = (value == "1");
						break;
					case HeaderKeys.BOARDNAME:
						this.BoardName = value;
						break;
					case HeaderKeys.SYNCSETTINGS:
						this.SyncSettings = Deserialize(value);
						break;
					case HeaderKeys.LOOPOFFSET:
						this.LoopOffset = int.Parse(value);
						break;
					case HeaderKeys.CORE:
						this.Core = value;
						break;
					case HeaderKeys.VIDEOPLUGIN:
						this.VideoPlugin = value;
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
			var entry = archive.Entries.SingleOrDefault(
					e => e.Name.StartsWith("Header", StringComparison.OrdinalIgnoreCase)
					);

			if (entry == null)
				entry = archive.CreateEntry("Header.txt");

			var file = new StreamWriter(entry.Open());

			#region Native types
			file.WriteLine(HeaderKeys.RERECORDS + " " + Rerecords.ToString());
			file.WriteLine(HeaderKeys.STARTSFROMSAVESTATE + " " + StartsFromSavestate.ToString());
			file.WriteLine(HeaderKeys.FOURSCORE + " " + FourScore.ToString());
			file.WriteLine(HeaderKeys.PAL + " " + Pal.ToString());
			#endregion

			#region Strings
			if (!String.IsNullOrEmpty(EmulatorVersion))
				file.WriteLine(HeaderKeys.EMULATIONVERSION + " " + EmulatorVersion);

			if (!String.IsNullOrEmpty(MovieVersion))
				file.WriteLine(HeaderKeys.MOVIEVERSION + " " + MovieVersion);

			if (!String.IsNullOrEmpty(Platform))
				file.WriteLine(HeaderKeys.PLATFORM + " " + Platform);

			if (!String.IsNullOrEmpty(GameName))
				file.WriteLine(HeaderKeys.GAMENAME + " " + GameName);

			if (!String.IsNullOrEmpty(Author))
				file.WriteLine(HeaderKeys.AUTHOR + " " + Author);
			
			if (!String.IsNullOrEmpty(Hash))
				file.WriteLine(HeaderKeys.SHA1 + " " + Hash);

			if (!String.IsNullOrEmpty(FirmwareHash))
				file.WriteLine(HeaderKeys.FIRMWARESHA1 + " " + FirmwareHash);
			
			if (!String.IsNullOrEmpty(BoardName))
				file.WriteLine(HeaderKeys.BOARDNAME + " " + BoardName);

			if (!String.IsNullOrEmpty(Core))
				file.WriteLine(HeaderKeys.CORE + " " + Core);

			if (!String.IsNullOrEmpty(VideoPlugin))
				file.WriteLine(HeaderKeys.VIDEOPLUGIN + " " + VideoPlugin);
			#endregion
			
			#region Other nullables
			if (BinarySavestate != null)
				file.WriteLine(HeaderKeys.SAVESTATEBINARYBASE64BLOB + " " + Encoding.ASCII.GetString(BinarySavestate));

			if (LoopOffset.HasValue)
				file.WriteLine(HeaderKeys.LOOPOFFSET + " " + LoopOffset.ToString());

			if (SyncSettings != null)
				file.WriteLine(HeaderKeys.SYNCSETTINGS + " " + JsonConvert.SerializeObject(SyncSettings));
			#endregion

			file.Flush();
			file.Close();
		}
	}

	public static class HeaderKeys
	{
		public const string EMULATIONVERSION          = "emuVersion";
		public const string MOVIEVERSION              = "MovieVersion";
		public const string PLATFORM                  = "Platform";
		public const string GAMENAME                  = "GameName";
		public const string AUTHOR                    = "Author";
		public const string RERECORDS                 = "rerecordCount";
		public const string STARTSFROMSAVESTATE       = "StartsFromSavestate";
		public const string SAVESTATEBINARYBASE64BLOB = "SavestateBinaryBase64Blob";
		public const string FOURSCORE                 = "FourScore";
		public const string SHA1                      = "SHA1";
		public const string FIRMWARESHA1              = "FirmwareSHA1";
		public const string PAL                       = "PAL";
		public const string BOARDNAME                 = "BoardName";
		public const string SYNCSETTINGS              = "SyncSettings";
		public const string LOOPOFFSET                = "LoopOffset";
		public const string CORE                      = "Core";
		public const string VIDEOPLUGIN               = "VideoPlugin";
	}
}