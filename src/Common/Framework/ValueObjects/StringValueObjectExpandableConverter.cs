using System;
using System.ComponentModel;
using System.Globalization;

namespace Framework.ValueObjects
{
    internal class StringValueObjectExpandableConverter<TValueObject> : ExpandableObjectConverter
        where TValueObject : IStringValueObject
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) =>
            value is string
                ? (TValueObject)value
                : default(TValueObject);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
            (value != null && value is TValueObject)
                ? ((TValueObject)value).Value
                : null;
    }

    
}
