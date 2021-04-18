using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;

namespace Framework.UI.MVC.TagHelpers
{
    public class EnvironmentTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private const string CdnName = "asp-environment";

        protected IHostingEnvironment HostingEnvironment { get; }

        protected IMemoryCache Cache { get; }

        [HtmlAttributeName(CdnName)]
        public bool UsarCdn { get; set; }

        public EnvironmentTagHelper(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

        }
    }
}
