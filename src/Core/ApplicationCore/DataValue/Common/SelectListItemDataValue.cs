using Framework.Extenders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DataValue.Common
{
    public class SelectListItemDataValue
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }

    public class SelectListItemDataValue<TEnum> : SelectListItemDataValue where TEnum : Enum
    {
        public TEnum Enum { set { Group = value.GetDescription(); } }
    }
}
