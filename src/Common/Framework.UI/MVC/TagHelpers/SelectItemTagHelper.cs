using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.UI.MVC.TagHelpers
{

    [HtmlTargetElement("select", Attributes = SelectItensName)]
    public class SelectItemTagHelper : TagHelper
    {
        private const string SelectItensName = "asp-select-itens";

        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName(SelectItensName)]
        public IEnumerable<SelectListItem> SelectItens { get; set; }
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        private ICollection<string> CurrentValues;

        public SelectItemTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Init(TagHelperContext context)
        {
            var realModelType = For.ModelExplorer.ModelType;
            var allowMultiple = typeof(string) != realModelType && typeof(IEnumerable).IsAssignableFrom(realModelType);

            CurrentValues = _generator.GetCurrentValues(ViewContext, For.ModelExplorer, For.Name, allowMultiple);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var agrupado = SelectItens.GroupBy(i => i.Group?.Name).OrderBy(i => i.Key);

            foreach (var grupo in agrupado)
            {
                if (grupo.Key != null)
                {
                    var groupDescription = grupo.Key.ToString();
                    output.PostContent.AppendHtml($"<optgroup label=\"{groupDescription}\">");
                }

                foreach (var item in grupo)
                {
                    object formDataEntry;
                    context.Items.TryGetValue(typeof(SelectTagHelper), out formDataEntry);

                    var selectedValues = CurrentValues ?? Enumerable.Empty<string>();

                    var selected = "";
                    if (selectedValues != null && selectedValues.Count() > 0 && selectedValues.Contains(item.Value.Trim()))
                        selected = " selected = \"selected\"";

                    output.PostContent.AppendHtml($"<option value=\"{item.Value}\"{selected}>{item.Text}</option>");
                }

                if (grupo.Key != null)
                {
                    output.PostContent.AppendHtml("</optgroup>");
                }
            }

        }

    }
}
