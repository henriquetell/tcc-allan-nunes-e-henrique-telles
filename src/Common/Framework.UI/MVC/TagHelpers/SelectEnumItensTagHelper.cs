using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Framework.UI.MVC.TagHelpers
{

    [HtmlTargetElement("select", Attributes = SelectEnumItensName)]
    public class SelectEnumItensTagHelper : TagHelper
    {
        private const string SelectEnumItensName = "asp-enum-itens";

        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName(SelectEnumItensName)]
        public Type EnumType { get; set; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        private ICollection<string> CurrentValues;

        public SelectEnumItensTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Init(TagHelperContext context)
        {
            if (For == null) return;

            var realModelType = For.ModelExplorer.ModelType;
            var allowMultiple = typeof(string) != realModelType && typeof(IEnumerable).IsAssignableFrom(realModelType);

            CurrentValues = _generator.GetCurrentValues(ViewContext, For.ModelExplorer, For.Name, allowMultiple);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IEnumerable<DataSelectEnumIten> datasource;
            if (EnumType.IsEnum)
                datasource = DataSourceFromEnum();

            else if (typeof(ValueObjects.IEnumeration).IsAssignableFrom(EnumType))
                datasource = DataSourceFromEnumeration();
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

        private IEnumerable<DataSelectEnumIten> DataSourceFromEnum()
        {
            var selectedValues = CurrentValues ?? Enumerable.Empty<string>();

            var members = EnumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var memberInfo in members)
            {
                var description = memberInfo.GetCustomAttribute<DescriptionAttribute>(false);

                if (description == null) continue;

                var memberValue = Convert.ChangeType(memberInfo.GetValue(null), typeof(int)).ToString();

                var selected = selectedValues != null && selectedValues.Contains(memberValue);

                yield return new DataSelectEnumIten(memberValue, description.Description, selected);
            }
        }

        private IEnumerable<DataSelectEnumIten> DataSourceFromEnumeration()
        {
            var selectedValues = CurrentValues ?? Enumerable.Empty<string>();

            return EnumType
                  .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                  .Where(info => EnumType.IsAssignableFrom(info.FieldType))
                  .Select(info => info.GetValue(null))
                  .Cast<ValueObjects.IEnumeration>()
                  .Select(info => new DataSelectEnumIten(
                      info.Value,
                      info.DisplayName,
                      selectedValues.Contains(info.Value) ||
                      selectedValues.Contains(info.DisplayName)
                     ));
        }

        private struct DataSelectEnumIten
        {
            public readonly string Value;
            public readonly string Display;
            public readonly bool Selected;

            public DataSelectEnumIten(string value, string display, bool selected)
            {
                Value = value;
                Display = display;
                Selected = selected;
            }
        }
    }
}
