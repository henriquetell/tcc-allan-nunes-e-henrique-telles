using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Framework.Extenders
{
    public static class FormatWithExtender
    {

        public static string FormatWith(this string input, string jsonSource,
            IFormatProvider provider = null)
        {
            var jObjectSource = JObject.Parse(jsonSource);

            return FormatWith(input, jObjectSource, provider);
        }

        public static string FormatWith(this string input, object source,
            IFormatProvider provider = null)
        {
            var jsonConfig = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            var jsonSource = JObject.FromObject(source, JsonSerializer.Create(jsonConfig));

            return FormatWith(input, jsonSource, provider);
        }

        private static string FormatWith(this string input, JObject jsonSource,
            IFormatProvider provider = null)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]\:]+)(?<format>:[^}]+)?(?<end>\})+",
              RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            provider = provider ?? CultureInfo.CreateSpecificCulture("pt-BR");

            var rewrittenFormat = r.Replace(input, delegate (Match m)
            {
                var startGroup = m.Groups["start"];
                var propertyGroup = m.Groups["property"];
                var formatGroup = m.Groups["format"];
                var endGroup = m.Groups["end"];

                if (!propertyGroup.Value.Contains(":"))
                {
                    var token = jsonSource.SelectToken(propertyGroup.Value);
                    if (token != null)
                        return string.Format(provider, "{0}", token);
                }
                else
                {
                    var v = propertyGroup.Value.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    var token = jsonSource.SelectToken(v[0]);
                    if (token != null)
                        return string.Format(provider, $"{{0:{v[1]}}}", token);

                }

                return m.Value;
            });

            return rewrittenFormat;
        }
    }
}
