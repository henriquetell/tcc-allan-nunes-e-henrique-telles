using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;

namespace Framework.UI.Extenders
{
    public static class CustomHiddenForModel
    {
        public static IHtmlContent HiddenForModel(this IHtmlHelper htmlHelper, object model, string htmlFieldPrefix = "")
        {
            using (var writer = new StringWriter())
            {
                HiddenForModelWriter(model, writer, htmlFieldPrefix);
                return new HtmlString(writer.ToString());
            }
        }

        private static void HiddenForModelWriter(object model, StringWriter writer, string htmlFieldPrefix = "")
        {
            foreach (var propertyInfo in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var isPrimitiveType = propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.IsValueType || (propertyInfo.PropertyType == typeof(string));
                if (isPrimitiveType)
                {
                    var builder = new TagBuilder("input");
                    builder.Attributes["type"] = "hidden";
                    builder.Attributes["name"] = htmlFieldPrefix+ propertyInfo.Name;
                    builder.Attributes["value"] = propertyInfo.GetValue(model, null)?.ToString();
                    builder.TagRenderMode = TagRenderMode.SelfClosing;

                    builder.WriteTo(writer, HtmlEncoder.Default);
                }
                else
                {
                    var propModel = propertyInfo.GetValue(model, null);
                    if (propModel != null)
                        HiddenForModelWriter(propModel, writer, $"{htmlFieldPrefix}{propertyInfo.Name}.");
                }
            }
        }
    }
}
