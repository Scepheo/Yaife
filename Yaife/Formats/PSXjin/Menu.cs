using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaife.Formats.PSXjin
{
	public class Menu : ToolStripMenuItem
	{
		private MovieTab tab;
		private Movie movie;

		public Menu(MovieTab tab)
		{
			this.Text = "PSXjin";
			this.tab = tab;
			this.movie = tab.Movie as Movie;

			var exportToPXM = new ToolStripMenuItem("Export to PXM");
			exportToPXM.Click += (s, e) => ExportToPXM();
			exportToPXM.ToolTipText = "Save the movie as a PXM file, for PCSX-rr.";
			DropDownItems.Add(exportToPXM);
		}

		private void ExportToPXM()
		{
			var dialog = new SaveFileDialog();
			dialog.Filter = "PCSX-rr|*.pxm|All Files|*.*";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var fileName = dialog.FileName;

				var pxm = ConvertToPXM(movie);
				pxm.WriteFile(fileName);
			}
		}

		private PCSX.Movie ConvertToPXM(Movie movie)
		{
			var pxm = new PCSX.Movie();

			// Copy header values
			pxm.RealHeader = new PCSX.Header();
			var movieType = movie.RealHeader.GetType();

			foreach (var member in pxm.RealHeader.GetType().GetMembers())
			{
				if (member.MemberType == MemberTypes.Field)
				{
					var value = movieType.GetField(member.Name).GetValue(movie.RealHeader);
					((FieldInfo)member).SetValue(pxm.RealHeader, value);
				}

				if (member.MemberType == MemberTypes.Property)
				{
					var value = movieType.GetProperty(member.Name).GetValue(movie.RealHeader);
					((PropertyInfo)member).SetValue(pxm.RealHeader, value);
				}
			}

			pxm.RealHeader.Signature = new byte[] { 0x50, 0x58, 0x4D, 0x20 };
			pxm.RealHeader.MovieVersion = 2;
			pxm.RealHeader.EmulatorVersion = 12;

			// Our current input log should be perfectly parseable
			pxm.InputLog = movie.InputLog;

			return pxm;
		}
	}
}
