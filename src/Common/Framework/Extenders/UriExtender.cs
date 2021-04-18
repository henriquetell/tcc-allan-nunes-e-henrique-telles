using System;
using System.Linq;

namespace Framework.Extenders
{
    public class UriExtender
    {
        public static string Combine(string basePath, params string[] paths)
        {
            var uri = new Uri(basePath);
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/')))).ToString();
        }
    }
}
