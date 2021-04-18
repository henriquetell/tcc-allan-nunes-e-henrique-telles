using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;


namespace Framework.UI.Extenders
{
    public static class HmtlHelperExtensions
    {
        public static string IsSelected(this IHtmlHelper html, string[] controller = null, string action = null, string cssClass = null, string[] controllerRelatorio = null, string[] actionRelatorio = null)
        {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            var currentAction = (string)html.ViewContext.RouteData.Values[nameof(action)];
            var currentController = (string)html.ViewContext.RouteData.Values[nameof(controller)];

            if (controller == null)
                controller = new string[] { };

            if (string.IsNullOrEmpty(action))
                action = currentAction;

            var eRelatorio = controllerRelatorio != null && actionRelatorio != null && controllerRelatorio.Contains(currentController) && actionRelatorio.Contains(currentAction);

            if (controller.Contains(currentController) && action == currentAction || eRelatorio)
                return cssClass;
            else
                return string.Empty;
        }
    }
}
