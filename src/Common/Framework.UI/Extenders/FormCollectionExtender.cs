using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Framework.UI.Extenders
{
    public static class FormCollectionExtender
    {
        public static string GetJsonForm(this IFormCollection form)
        {
            if (form == null || form.Count <= 0)
                return null;

            var itens = form.Where(c => c.Key != "__RequestVerificationToken")
                .ToDictionary<KeyValuePair<string, StringValues>, string, string>(item => item.Key, item => item.Value);

            return JsonConvert.SerializeObject(itens);
        }
    }
}
