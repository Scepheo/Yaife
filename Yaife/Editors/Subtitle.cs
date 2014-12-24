using System;
using System.Drawing;

namespace Yaife
{
	public class Subtitle
	{
		public int Frame { get; set; }
		public int Length { get; set; }
		public Color Color { get; set; }
		public string Text { get; set; }

		public string ToSubRip(int num, float fps)
		{
			var str = num.ToString() + Environment.NewLine;
			str += subRipTime(Frame / fps) + " --> " + subRipTime((Frame + Length) / fps);
			str += Environment.NewLine;
			str += Text + Environment.NewLine;
			str += Environment.NewLine;
			return str;
		}

		private string subRipTime(float timeInSeconds)
		{
			var hours        = (int)(Math.Floor(timeInSeconds / 3600));
			var minutes      = (int)(Math.Floor(timeInSeconds / 60) % 60);
			var seconds      = (int)(Math.Floor(timeInSeconds) % 60);
			var milliSeconds = (int)(Math.Floor(timeInSeconds * 1000) % 1000);

			return String.Format("{0:D2}:{1:D2}:{2:D2},{3:D3}", hours, minutes, seconds, milliSeconds);
		}

		public static string ToSubRip(Subtitle[] subs, float fps)
		{
			var str = "";

			for (int i = 0; i < subs.Length; i++)
				str += subs[i].ToSubRip(i + 1, fps);

			return str;
		}

		public string ToSubStationAlpha(float fps)
		{
			//Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
			return "Dialogue: "
				+ String.Join(",",
					0,
					subStationAlphaTime(Frame / fps),
					subStationAlphaTime((Frame + Length) / fps),
					"default",
					"",
					"0000",
					"0000",
					"0000",
					"",
					Text)
				+ "\n";
		}

		private string subStationAlphaTime(float timeInSeconds)
		{
			var hours = (int)(Math.Floor(timeInSeconds / 3600));
			var minutes = (int)(Math.Floor(timeInSeconds / 60) % 60);
			var seconds = (int)(Math.Floor(timeInSeconds) % 60);
			var centiSeconds = (int)(Math.Floor(timeInSeconds * 100) % 100);

			return String.Format("{0:D1}:{1:D2}:{2:D2}.{3:D2}", hours, minutes, seconds, centiSeconds);
		}

		public static string ToSubStationAlpha(Subtitle[] subs, float fps)
		{
			var str = "";

			// TODO: It'd probably be best to ask the user for dimensions,
			// especially when using positioned subtitles.
			str += "[Script Info]\n";
			str += "Title: Yaife Exported Subtitles\n";
			str += "Script Type: v4.00+\n";
			str += "PlayResY: 640\n";
			str += "PlayResX: 480\n";
			str += "\n";

			str += "[V4+ Styles]\n";
			str += "Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, BackColour, Bold, Italic, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding\n";
			str += "Style: default,Arial,28,11861244,11861244,-2147483640,-1,0,1,1,2,2,30,30,30,0,0\n";
			str += "\n";

			str += "[Events]\n";
			str += "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";

			for (int i = 0; i < subs.Length; i++)
				str += subs[i].ToSubStationAlpha(fps);

			return str;
		}
	}
}