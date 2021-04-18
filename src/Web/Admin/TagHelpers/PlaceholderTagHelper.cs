using Admin.Resources;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Resources;

namespace Admin.TagHelpers
{
    [HtmlTargetElement(HtmlTargetElementAttribute.ElementCatchAllTarget, Attributes = PlaceholderAttributeName)]
    public class PlaceholderTagHelper : TagHelper
    {
        private const string PlaceholderAttributeName = "asp-placeholder";
        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof(DisplayModelResource));

        [HtmlAttributeName(PlaceholderAttributeName)]
        public string Placeholder { get; set; }

        public PlaceholderTagHelper()
        { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output.Attributes.Any(c => c.Name == "placeholder"))
                return;

            if (!string.IsNullOrWhiteSpace(Placeholder))
            {
                var placeholder = ResourceManager.GetString(Placeholder);
                if (!string.IsNullOrWhiteSpace(placeholder))
                    output.Attributes.Add(nameof(placeholder), placeholder);
            }
            else
            {
                var name = output.Attributes.FirstOrDefault(c => c.Name == "name")?.Value.ToString();
                if (name != null && name.Contains("."))
                    name = name.Split(".").Last();

                var aspFor = context.AllAttributes.FirstOrDefault(c => c.Name == "asp-for");
                if (aspFor != null)
                {
                    var containerType = (aspFor.Value as Microsoft.AspNetCore.Mvc.ViewFeatures.ModelExpression)?
                        .Metadata?.ContainerType?.Name;

                    if (!string.IsNullOrWhiteSpace(containerType))
                    {
                        var placeholder = ResourceManager.GetString($"{containerType}_{name}");
                        if (!string.IsNullOrWhiteSpace(placeholder))
                        {
                            output.Attributes.Add(nameof(placeholder), placeholder);
                            return;
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var placeholder = ResourceManager.GetString(name);
                    if (!string.IsNullOrWhiteSpace(name))
                        output.Attributes.Add(nameof(placeholder), placeholder);
                }
            }

        }
    }
}
