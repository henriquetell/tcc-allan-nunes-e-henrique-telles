using Microsoft.AspNetCore.Http;
using System;
using System.Text.Encodings.Web;


namespace Framework.UI.Extenders
{
    public static class HttpRequestExtender
    {
        public static bool IsPjax(this HttpRequest request) => request.Headers.ContainsKey("X-PJAX");


        public static IHeaderDictionary SetExcelContent(this IHeaderDictionary header, string filename)
        {
            header.Add("Content-Disposition", $"attachment; filename={UrlEncoder.Default.Encode(filename)}-{DateTime.Now.Ticks}.xlsx");
            return header;
        }

        public static bool IsAjaxRequest(this HttpRequest request) => request?.Headers?["X-Requested-With"].ToString() == "XMLHttpRequest";
    }
}
