using ApplicationCore.DataValue.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Extenders
{
    public static class SelectListItemExtension
    {
        public static List<SelectListItem> Convert(this List<SelectListItemDataValue> value)
        {
            if (value == null || !value.Any())
                return new List<SelectListItem>();

            return value.Select(c => new SelectListItem(c.Text, c.Value)).OrderBy(c => c.Text).ToList();
        }

        public static List<SelectListItem> Convert<TEnum>(this List<SelectListItemDataValue<TEnum>> value) where TEnum : Enum
        {
            if (value == null || !value.Any())
                return new List<SelectListItem>();

            return value.Select(c => new SelectListItem(c.Text, c.Value)
            {
                Group = new SelectListGroup
                {
                    Name = c.Group
                }
            })
            .OrderBy(c => c.Group.Name)
            .ThenBy(c => c.Text)
            .ToList();
        }

        public static List<SelectListItem> Convert(this List<SelectListItemDataValue> value, params int?[] selected) =>
            Convert(value, selected.Where(s => s.HasValue).Cast<int>().ToArray());

        public static List<SelectListItem> Convert(this List<SelectListItemDataValue> value, params int[] selected)
        {
            if (value == null || !value.Any())
                return new List<SelectListItem>();

            return value.Select(c => new SelectListItem(c.Text, c.Value, selected?.Select(y => y.ToString()).Contains(c.Value) ?? false)).OrderBy(c => c.Text).ToList();
        }
    }
}
