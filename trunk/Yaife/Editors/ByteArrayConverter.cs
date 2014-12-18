using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaife
{
	public class HexByteConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(byte[]))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is byte[] && destinationType == typeof(string))
			{
				var bytes = value as byte[];

				var sb = new StringBuilder(bytes.Length * 2);

				foreach (var b in bytes)
					sb.AppendFormat("{0:X2} ", b);

				return sb.ToString();
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{
				var str = value as string;
				str = str.Replace(" ", "");
				var bytes = new byte[str.Length / 2];

				for (int i = 0; i < bytes.Length; i++)
				{
					bytes[i] = byte.Parse(str.Substring(i * 2, 2), NumberStyles.HexNumber);
				}

				return bytes;
			}

			return base.ConvertFrom(context, culture, value);
		}
	}
}
