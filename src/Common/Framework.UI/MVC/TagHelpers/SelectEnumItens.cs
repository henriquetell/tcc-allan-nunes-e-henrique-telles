using Framework.ValueObjects;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Framework.UI.MVC.TagHelpers
{

    [HtmlTargetElement("select", Attributes = SelectEnumItensName)]
    public class SelectEnumItens : TagHelper
    {
        private const string SelectEnumItensName = "asp-enum-itens";

        [HtmlAttributeName(SelectEnumItensName)]
        public Type EnumType { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IEnumerable<SelectEnumIten> datasource;
            if (EnumType.IsEnum)
                datasource = DataSourceFromEnum(context);

            else if (typeof(IEnumeration).IsAssignableFrom(EnumType))
                datasource = DataSourceFromAdgEnumeration(context);
            else
                throw new Exception("O Tipo deve ser um enum ou Enumeration");

            foreach (var item in datasource)
            {
                var selected = item.Selected
                    ? " selected = \"selected\""
                    : "";

                output.PostContent.AppendHtml($"<option value=\"{item.Value}\"{selected}>{item.Display}</option>");
            }
        }

        private IEnumerable<SelectEnumIten> DataSourceFromEnum(TagHelperContext context)
        {
            var datasource = new List<KeyValuePair<string, string>>();

            context.Items.TryGetValue(typeof(SelectTagHelper), out var formDataEntry);

            var selectedValues = (formDataEntry as CurrentValues)?.Values ?? Enumerable.Empty<string>(); //ICollection<string>;

            var members = EnumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var memberInfo in members.OrderBy(c => c.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? c.Name))
            {
                var description = memberInfo.GetCustomAttribute<DescriptionAttribute>(false);

                var memberValue = Convert.ChangeType(memberInfo.GetValue(null), typeof(int)).ToString();

                var selected = selectedValues != null && selectedValues.Contains(memberValue);

                yield return new SelectEnumIten(memberValue, description?.Description ?? memberInfo.Name, selected);
            }
        }

        private IEnumerable<SelectEnumIten> DataSourceFromAdgEnumeration(TagHelperContext context)
        {
            context.Items.TryGetValue(typeof(SelectTagHelper), out var formDataEntry);
            var selectedValues = (formDataEntry as CurrentValues)?.Values ?? Enumerable.Empty<string>(); //ICollection<string>;

            return EnumType
                  .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                  .Where(info => EnumType.IsAssignableFrom(info.FieldType))
                  .Select(info => info.GetValue(null))
                  .Cast<IEnumeration>()
                  .Select(info => new SelectEnumIten(
                      info.Value,
                      info.DisplayName,
                      selectedValues.Contains(info.Value) ||
                      selectedValues.Contains(info.DisplayName)
                     ));
        }

        private struct SelectEnumIten
        {
            public readonly string Value;
            public readonly string Display;
            public readonly bool Selected;

            public SelectEnumIten(string value, string display, bool selected)
            {
                Value = value;
                Display = display;
                Selected = selected;
            }
        }
    }
}
