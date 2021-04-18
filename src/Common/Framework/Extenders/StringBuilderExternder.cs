using System.Text;

namespace Framework.Extenders
{
    public static class StringBuilderExternder
    {
        public static void AppendLineFormat(this StringBuilder sb, string text, params object[] values)
        {
            if (sb == null)
                return;
            if (string.IsNullOrWhiteSpace(text))
                return;

            sb.AppendFormat(text, values);
            sb.AppendLine();
        }
    }
}