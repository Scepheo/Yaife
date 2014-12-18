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
			//Format: Layer, Name, Start, End, Style, MarginL, MarginR, MarginV, Effect, Text
			return "Dialoque: "
				+ String.Join(",",
					0,
					"NPT",
					subStationAlphaTime(Frame / fps),
					subStationAlphaTime((Frame + Length) / fps),
					"DefaultVCD",
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

			return String.Format("{0:D1}:{1:D2}:{2:D2}:{3:D2}", hours, minutes, seconds, centiSeconds);
		}

		public static string ToSubStationAlpha(Subtitle[] subs, float fps)
		{
			var str = "";

			str += "[Script Info]\n";
			str += "Title: Yaife Exported Subtitles\n";
			str += "Original Script: Yaife\n";
			str += "Script Type: V4.00\n";
			str += "Collisions: Normal\n";
			str += "PlayResY: 200\n";
			str += "PlayDepth: 0\n";
			str += "Timer: 100,0000\n";
			str += "\n";

			str += "[v4 Styles]\n";
			str += "Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, TertiaryColour, BackColour, Bold, Italic, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding\n";
			str += "Style: DefaultVCD, Arial,28,11861244,11861244,11861244,-2147483640,-1,0,1,1,2,2,30,30,30,0,0\n";
			str += "\n";

			str += "[Events]\n";
			str += "Format: Layer, Name, Start, End, Style, MarginL, MarginR, MarginV, Effect, Text\n";

			for (int i = 0; i < subs.Length; i++)
				str += subs[i].ToSubStationAlpha(fps);

			return str;
		}
	}
}