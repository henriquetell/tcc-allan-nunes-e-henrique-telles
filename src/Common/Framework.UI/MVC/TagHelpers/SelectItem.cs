using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Framework.UI.MVC.TagHelpers
{

    [HtmlTargetElement("select", Attributes = SelectItensName)]
    public class SelectItem : TagHelper
    {
        private const string SelectItensName = "asp-select-itens";

        [HtmlAttributeName(SelectItensName)]
        public IEnumerable<SelectListItem> SelectItens { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var agrupado = SelectItens.GroupBy(i => i?.Group?.Name).OrderBy(i => i?.Key);

            foreach (var grupo in agrupado)
            {
                if (grupo.Key != null)
                    output.PostContent.AppendHtml($"<optgroup label=\"{grupo.Key}\">");

                foreach (var item in grupo)
                {
                    context.Items.TryGetValue(typeof(SelectTagHelper), out var formDataEntry);

                    var selectedValues = (formDataEntry as CurrentValues)?.Values ?? Enumerable.Empty<string>();

                    var selected = "";
                    if (selectedValues != null && selectedValues.Any() && selectedValues.Contains(item.Value.Trim()))
                        selected = " selected = \"selected\"";

                    output.PostContent.AppendHtml($"<option value=\"{item.Value}\"{selected}>{item.Text}</option>");
                }

                if (grupo.Key != null)
                    output.PostContent.AppendHtml("</optgroup>");
            }

        }

    }
}
