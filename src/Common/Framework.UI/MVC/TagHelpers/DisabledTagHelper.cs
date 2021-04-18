using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Framework.UI.MVC.TagHelpers
{
    [HtmlTargetElement(HtmlTargetElementAttribute.ElementCatchAllTarget, Attributes = DisabledAttributeName)]
    public class DisabledTagHelper : TagHelper
    {
        private const string DisabledAttributeName = "asp-disabled";

        [HtmlAttributeName(DisabledAttributeName)]
        public bool IsDisabled { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsDisabled)
                output.Attributes.Add("disabled", "disabled");
        }
    }
}
