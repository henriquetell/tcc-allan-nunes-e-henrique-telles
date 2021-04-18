using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Framework.UI.MVC.TagHelpers
{
    [HtmlTargetElement(HtmlTargetElementAttribute.ElementCatchAllTarget, Attributes = VisibilityAttributeName)]
    public class VisibilityTagHelper : TagHelper
    {
        private const string VisibilityAttributeName = "asp-visibility";

        [HtmlAttributeName(VisibilityAttributeName)]
        public bool IsVisibled { get; set; }


        private const string VisibilityAttributeSuppressOutputName = "asp-suppress";

        [HtmlAttributeName(VisibilityAttributeSuppressOutputName)]
        public bool SuppressOutput { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrWhiteSpace(output.TagName) && output.TagName.ToLower() == "asp")
                output.TagName = null;

            if (!IsVisibled)
            {
                if (SuppressOutput)
                    output.TagName = null;

                output.SuppressOutput();
            }
        }
    }
}
