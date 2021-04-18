using System;
using Microsoft.Extensions.Caching.Memory;
using Framework.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Framework.UI.MVC.TagHelpers
{
    [HtmlTargetElement("img", Attributes = CdnName)]
    [HtmlTargetElement("script", Attributes = CdnName)]
    [HtmlTargetElement("link", Attributes = CdnName)]
    [HtmlTargetElement("iframe", Attributes = CdnName)]
    public class CdnTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private const string CdnName = "asp-cdn";

        private IFileVersionProvider _fileVersionProvider;

        protected IHostingEnvironment HostingEnvironment { get; }

        protected IMemoryCache Cache { get; }

        [HtmlAttributeName(CdnName)]
        public bool UsarCdn { get; set; }

        private readonly FrameworkConfig _config;

        public CdnTagHelper(IOptions<FrameworkConfig> config, IHostingEnvironment hostingEnvironment, IMemoryCache cache, IFileVersionProvider fileVersionProvider)
        {
            HostingEnvironment = hostingEnvironment;
            Cache = cache;
            _config = config.Value;
            _fileVersionProvider = fileVersionProvider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!UsarCdn || !_config.Cdn.UsarCdn || string.IsNullOrWhiteSpace(_config.Cdn.UrlEndPointSite))
                return;

            var sourceAttr = context.AllAttributes[GetPathAttributeName(output)];
            var path = sourceAttr.Value.ToString();

            if (path.StartsWith("~"))
                path = path.Substring(1);

            EnsureFileVersionProvider();
            //var pathVersion = _fileVersionProvider.AddFileVersionToPath(path);

            //output.Attributes.SetAttribute(
            //    sourceAttr.Name,
            //    ConvertToCdnUrl(pathVersion));
        }

        private string ConvertToCdnUrl(string path)
        {
            Uri uri;
            if (!Uri.TryCreate(new Uri(_config.Cdn.UrlEndPointSite), path, out uri))
                throw new Exception($"Invalid path to CDN \"{path}\"");

            return uri.ToString();
        }

        private static string GetPathAttributeName(TagHelperOutput output)
        {
            switch (output.TagName.ToLower())
            {
                case "link":
                    return "href";
                case "img":
                case "script":
                case "iframe":
                    return "src";
                default:
                    throw new NotSupportedException($"The tag {output.TagName} not supported.");
            }
        }

        private void EnsureFileVersionProvider()
        {
            if (_fileVersionProvider == null)
            {
                //_fileVersionProvider = new FileVersionProvider(
                //    HostingEnvironment.WebRootFileProvider,
                //    Cache,
                //    ViewContext.HttpContext.Request.PathBase);
            }
        }
    }
}
