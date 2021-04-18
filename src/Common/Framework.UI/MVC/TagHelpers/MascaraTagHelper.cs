using Framework.Extenders;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Framework.UI.MVC.TagHelpers
{
    [HtmlTargetElement("input", Attributes = MaskAttributeName)]
    [HtmlTargetElement("p", Attributes = MaskAttributeName)]
    [HtmlTargetElement("span", Attributes = MaskAttributeName)]
    [HtmlTargetElement("h1", Attributes = MaskAttributeName)]
    [HtmlTargetElement("h2", Attributes = MaskAttributeName)]
    [HtmlTargetElement("h3", Attributes = MaskAttributeName)]
    [HtmlTargetElement("h4", Attributes = MaskAttributeName)]
    [HtmlTargetElement("h5", Attributes = MaskAttributeName)]
    [HtmlTargetElement("h6", Attributes = MaskAttributeName)]
    public class MascaraTagHelper : TagHelper
    {
        private const string MaskAttributeName = "asp-mascara";

        [HtmlAttributeName(MaskAttributeName)]
        public EMascara NomeMascara { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-mascara", NomeMascara.GetDescription());
        }

    }
}
