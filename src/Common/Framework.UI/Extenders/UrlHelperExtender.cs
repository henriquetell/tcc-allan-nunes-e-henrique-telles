using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.UI.Extenders
{
    public static class UrlHelperExtender
    {

        //public static string ContentAppFileVersion(this IUrlHelper url, string path, bool fullUriAdress = true)
        //{
        //    if (path.StartsWith("~"))
        //        path = path.Substring(1);
            
        //    path = GetFileVersionProvider(url.ActionContext).AddFileVersionToPath(path);

        //    if (!fullUriAdress)
        //        return path;

        //    var request = url.ActionContext.HttpContext.Request;

        //    return new Uri(
        //        new Uri($"{request.Scheme}://{request.Host}"),
        //        path
        //    ).ToString();
        //}

        //private static FileVersionProvider GetFileVersionProvider(ActionContext actionContext)
        //{
        //    var cache = actionContext.HttpContext.RequestServices.GetService<IMemoryCache>();

        //    var hostingEnvironment = actionContext.HttpContext.RequestServices.GetService<IHostingEnvironment>();

        //    var fileVersionProvider = new FileVersionProvider(
        //        hostingEnvironment.WebRootFileProvider,
        //        cache,
        //        actionContext.HttpContext.Request.PathBase);

        //    return fileVersionProvider;
        //}

    }
}
