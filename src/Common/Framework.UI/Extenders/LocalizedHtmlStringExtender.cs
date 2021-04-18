using Microsoft.AspNetCore.Mvc.Localization;

namespace Framework.UI.Extenders
{
    public static class LocalizedHtmlStringExtender
    {
        public static string GetValueOrDefault(this LocalizedHtmlString ls, string defaultValue, params object[] args)
        {
            if (ls.IsResourceNotFound)
                return defaultValue;

            if (string.IsNullOrWhiteSpace(ls.Value))
                return defaultValue;

            return string.Format(ls.Value, args);
        }
    }
}
