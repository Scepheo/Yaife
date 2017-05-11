using System;
using System.ComponentModel;
using System.Globalization;
using Yaife.Utilities;

namespace Yaife.Editors
{
    public class HexByteConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var bytes = value as byte[];

            return bytes != null && destinationType == typeof(string)
                ? bytes.ToHexString()
                : base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            return str != null ? HexString.Parse(str) : base.ConvertFrom(context, culture, value);
        }
    }
}
