using Microsoft.Extensions.Localization;

namespace Framework.Extenders
{
    public static class LocalizedStringExtender
    {
        public static LocalizedString Format(this LocalizedString ls, params object[] args)
        {
            return new LocalizedString(ls.Name,
                string.Format(ls.Value, args));
        }
    }
}
