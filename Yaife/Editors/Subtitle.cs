using System;
using System.Drawing;
using System.Text;

namespace Yaife.Editors
{
    public class Subtitle
    {
        public int Frame { get; set; }
        public int Length { get; set; }
        public Color Color { get; set; }
        public string Text { get; set; }

        private string ToSubRip(int num, float fps)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(num.ToString());
            stringBuilder.AppendLine($"{SubRipTime(Frame / fps)} --> {SubRipTime((Frame + Length) / fps)}");
            stringBuilder.AppendLine(Text);
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }

        private static string SubRipTime(float timeInSeconds)
        {
            var hours        = (int)Math.Floor(timeInSeconds / 3600);
            var minutes      = (int)(Math.Floor(timeInSeconds / 60) % 60);
            var seconds      = (int)(Math.Floor(timeInSeconds) % 60);
            var milliSeconds = (int)(Math.Floor(timeInSeconds * 1000) % 1000);

            return $"{hours:D2}:{minutes:D2}:{seconds:D2},{milliSeconds:D3}";
        }

        public static string ToSubRip(Subtitle[] subs, float fps)
        {
            var stringBuilder = new StringBuilder();
            
            for (var i = 0; i < subs.Length; i++)
            {
                stringBuilder.Append(subs[i].ToSubRip(i + 1, fps));
            }

            return stringBuilder.ToString();
        }

        private string ToSubStationAlpha(float fps)
        {
            //Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
            return "Dialogue: "
                + string.Join(",",
                    0,
                    SubStationAlphaTime(Frame / fps),
                    SubStationAlphaTime((Frame + Length) / fps),
                    "default",
                    "",
                    "0000",
                    "0000",
                    "0000",
                    "",
                    Text);
        }

        private static string SubStationAlphaTime(float timeInSeconds)
        {
            var hours = (int)Math.Floor(timeInSeconds / 3600);
            var minutes = (int)(Math.Floor(timeInSeconds / 60) % 60);
            var seconds = (int)(Math.Floor(timeInSeconds) % 60);
            var centiSeconds = (int)(Math.Floor(timeInSeconds * 100) % 100);

            return $"{hours:D1}:{minutes:D2}:{seconds:D2}.{centiSeconds:D2}";
        }

        public static string ToSubStationAlpha(Subtitle[] subtitles, float fps)
        {
            var stringBuilder = new StringBuilder();

            // TODO: It'd probably be best to ask the user for dimensions, especially when using positioned subtitles.
            stringBuilder.AppendLine("[Script Info]");
            stringBuilder.AppendLine("Title: Yaife Exported Subtitles");
            stringBuilder.AppendLine("Script Type: v4.00+");
            stringBuilder.AppendLine("PlayResY: 640");
            stringBuilder.AppendLine("PlayResX: 480");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("[V4+ Styles]");
            stringBuilder.AppendLine("Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, BackColour, Bold, Italic, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding");
            stringBuilder.AppendLine("Style: default,Arial,28,11861244,11861244,-2147483640,-1,0,1,1,2,2,30,30,30,0,0");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("[Events]");
            stringBuilder.AppendLine("Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text");

            foreach (var subtitle in subtitles)
            {
                stringBuilder.AppendLine(subtitle.ToSubStationAlpha(fps));
            }

            return stringBuilder.ToString();
        }
    }
}