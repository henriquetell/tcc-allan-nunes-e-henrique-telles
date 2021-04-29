using ApplicationCore.Enuns;
using Framework.Extenders;
using Microsoft.AspNetCore.Html;

namespace Admin.Extenders
{
    public static class HtmlExtender
    {
        public static IHtmlContent CriarBadgeAvaliacaoNps(this int valor)
        {
            if (valor >= 75)
                return new HtmlString($"<span class=\"badge badge-primary\">Excelente: <b>{valor}</b></span>");

            if (valor >= 50 && valor < 75)
                return new HtmlString($"<span class=\"badge badge-info\">Muito bom: <b>{valor}</b></span>");

            if (valor >= 0 && valor < 50)
                return new HtmlString($"<span class=\"badge badge-warning\">Razoável: <b>{valor}</b></span>");

            return new HtmlString($"<span class=\"badge badge-danger\">Ruim: <b>{valor}</b></span>");
        }

        public static IHtmlContent CriarAlertAvaliacaoNps(this int valor)
        {
            if (valor >= 75)
                return new HtmlString($"<div class=\"alert alert-success text-center\">NPS Excelente. <b>{valor}</b></div>");

            if (valor >= 50 && valor < 75)
                return new HtmlString($"<div class=\"alert alert-info text-center\">NPS Muito bom. <b>{valor}</b></div>");

            if (valor >= 0 && valor < 50)
                return new HtmlString($"<div class=\"alert alert-warning text-center\">NPS Razoável. <b>{valor}</b></div>");

            return new HtmlString($"<div class=\"alert alert-danger text-center\">NPS Ruim. <b>{valor}</b></div>");
        }

        public static IHtmlContent CriarBadge(this EStatus status)
        {
            return status switch
            {
                EStatus.Ativo => new HtmlString($"<span class=\"badge badge-primary\">{status.GetDescription()}</span>"),
                EStatus.Inativo => new HtmlString($"<span class=\"badge badge-danger\">{status.GetDescription()}</span>"),
                _ => new HtmlString(""),
            };
        }

        public static IHtmlContent CriarBadge(this int value) => new HtmlString($"<span class=\"badge badge-primary\">{value}</span>");

        public static IHtmlContent CriarBadge(this int? value)
        {
            return value == null ? new HtmlString(string.Empty) : new HtmlString($"<span class=\"badge badge-primary\">{value}</span>");
        }

        public static IHtmlContent CriarBadge(this byte value) => new HtmlString($"<span class=\"badge badge-primary\">{value}</span>");

        public static IHtmlContent CriarBadge(this byte? value)
        {
            if (value == null)
                return new HtmlString(string.Empty);

            return new HtmlString($"<span class=\"badge badge-primary\">{value}</span>");
        }

        public static IHtmlContent LabelOperacao(this int id) => new HtmlString(id > 0 ? "Editar" : "Cadastrar");

        public static IHtmlContent LabelOperacao(this int? id) => new HtmlString(id.GetValueOrDefault() > 0 ? "Editar" : "Cadastrar");

        public static HtmlString CriarBadgeSla(this int? item)
        {
            if (!item.HasValue)
                return new HtmlString("");

            if (item <= 24)
                return new HtmlString($"<span class=\"badge badge-primary\">{item}h</span>");

            return item <= 48
                ? new HtmlString($"<span class=\"badge badge-warning\">{item}h</span>")
                : new HtmlString($"<span class=\"badge badge-danger\">{item}h</span>");
        }
    }
}
