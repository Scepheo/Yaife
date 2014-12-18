using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yaife.Formats.BizHawkBK2
{
	public partial class Header
	{
		public static JObject ReadSyncSettings(ZipArchive archive)
		{
			var entry = archive.Entries.SingleOrDefault(
					e => e.Name.StartsWith("SyncSettings", StringComparison.OrdinalIgnoreCase)
					);

			if (entry == null)
				return new JObject();

			var file = new StreamReader(entry.Open());

			JObject result = Deserialize(file.ReadToEnd());
			file.Close();

			return result;
		}

		public static void WriteSyncSettings(ZipArchive archive, JObject syncSettings)
		{
			var entry = archive.Entries.SingleOrDefault(
					e => e.Name.StartsWith("SyncSettings", StringComparison.OrdinalIgnoreCase)
					);

			if (entry == null)
				entry = archive.CreateEntry("SyncSettings.json");

			var file = new StreamWriter(entry.Open());

			file.WriteLine(Serialize(syncSettings));

			file.Flush();
			file.Close();
		}

		public static JObject Deserialize(string str)
		{
			var obj = JObject.Parse(str);
			return (JObject)obj["o"];
		}

		public static string Serialize(JObject obj)
		{
			var result = new JObject();
			result["o"] = (JToken)obj;
			return result.ToString(Formatting.None);
		}
	}
}
