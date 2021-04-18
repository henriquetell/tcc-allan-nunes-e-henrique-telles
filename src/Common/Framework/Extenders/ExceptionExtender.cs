using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Framework.Exceptions;

namespace Framework.Extenders
{
    public static class ExceptionExtender
    {
        public static string GetMessages(this Exception ex, string separador = null, bool includeStackTrace = false)
        {
            separador = separador ?? Environment.NewLine;

            var erros = new List<string>();

            while (ex != null)
            {
                if (ex is AggregateException)
                {
                    foreach (var i in ((AggregateException)ex).InnerExceptions)
                    {
                        erros.Add(i.GetMessages(separador));
                    }
                }
                else if (ex is MensagemException mensagemException)
                {
                    var m = string.Join(", ", mensagemException.Erros.Select(e => e.ToString()).ToArray());

                    erros.Add($"{ex.GetType().Name}: {m}");
                }
                else if (ex is WebException)
                {
                    erros.Add($"{ex.GetType().Name}: {ex.Message}");

                    var webException = (WebException)ex;
                    if (webException.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = webException.Response as HttpWebResponse;
                        if (response != null)
                        {
                            erros.Add($"Status: {WebExceptionStatus.ProtocolError}");
                            erros.Add($"Status Code: {(int)response.StatusCode}");
                            erros.Add($"Status Description: {response.StatusDescription}");

                            try
                            {
                                using (var stream = response.GetResponseStream())
                                {
                                    using (var reader = new StreamReader(stream))
                                    {
                                        erros.Add("Response: ");
                                        erros.Add(reader.ReadToEnd());
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                erros.Add("Não foi possível capturar a response da WebException");
                            }
                        }
                    }

                }
                else
                {
                    erros.Add($"{ex.GetType().Name}: {ex.Message}");
                }

                if (includeStackTrace && ex.StackTrace != null)
                {
                    erros.Add("StackTrace: ");
                    erros.Add(ex.StackTrace);
                }

                ex = ex.InnerException;
            }

            return string.Join(separador, erros.ToArray());
        }

        public static string GetMessages(this MensagemException ex, string separador = null)
        {
            separador = separador ?? Environment.NewLine;

            var erros = ex.Erros
                .Select(e => e.Value)
                .ToArray();

            return string.Join(separador, erros);
        }
    }
}