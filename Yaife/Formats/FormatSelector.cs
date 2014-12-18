using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Yaife.Formats
{
	public static class FormatSelector
	{
		private static Type[] formats =
		{
			typeof(BizHawkBK2.Movie),
			typeof(Hourglass.Movie),
			typeof(Dolphin.Movie),
			typeof(PCSX.Movie),
			typeof(VisualBoyAdvance.Movie)
		};

		public static string GetFileFilter()
		{
			var mainFilter = new StringBuilder("All known movie files|");
			var filter = new StringBuilder();

			foreach (var t in formats)
			{
				// Fetch and invoke the parameterless constructor
				var constructor = t.GetConstructor(Type.EmptyTypes);
				var format = (constructor.Invoke(new object[0]) as IMovie);

				var ext = String.Join(";*", format.Extensions);

				filter.Append(format.Description);
				filter.Append("|*");
				filter.Append(ext);
				filter.Append("|");

				mainFilter.Append("*" + ext + ";");
			}

			// Remove extra | from individual filter
			filter.Remove(filter.Length - 1, 1);

			// Remove extra ; from main filter
			mainFilter.Remove(mainFilter.Length - 1, 1);

			var allFilter = "All files|*.*";

			return String.Join("|", mainFilter, filter, allFilter);
		}

		public static IMovie Open(string path)
		{
			var extension = Path.GetExtension(path);

			// Guess based on extension
			foreach (var t in formats)
			{
				// Fetch and invoke the parameterless constructor
				var constructor = t.GetConstructor(Type.EmptyTypes);
				var format = (constructor.Invoke(new object[0]) as IMovie);

				if (format.Extensions.Any(ext => String.Equals(extension, ext, StringComparison.OrdinalIgnoreCase)))
				{
					format.ReadFile(path);
					return format;
				}
			}

			var fileName = Path.GetFileName(path);
			throw new Exception("File '" + fileName + "' did not match any known extensions.");
		}
	}
}
