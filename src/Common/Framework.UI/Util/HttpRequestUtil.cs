using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Framework.UI.Util
{
    public static class HttpRequestUtil
    {
        private static readonly Dictionary<string, string> Desktop = new Dictionary<string, string>
        {
            {"Windows 98","Windows 98"},
            {"Windows NT 5.0","Windows 2000"},
            {"Windows NT 5.1","Windows XP"},
            {"Windows NT 5.2","Windows Server 2003"},
            {"Windows NT 6.0","Windows Vista"},
            {"Windows NT 6.1","Windows 7"},
            {"Windows NT 6.2","Windows 8"},
            {"Windows NT 6.3","Windows 8.1"},
            {"Windows NT 10.0","Windows 10"},
            {"CrOS", "Chrome OS"}
        };

        private static readonly List<string> Mobiles = new List<string>
        {
            "Windows", "Android", "Linux", "iPhone",
            "iPad", "Macintosh", "PlayStation", "Opera Mini"
        };

        public static string GetIpAddress(HttpRequest httpRequest)
        {
            var keys = httpRequest.Headers.Keys.Aggregate(Empty, (current, key) => current + key + "#");
            var arrKeys = keys.Split('#');

            var x = 0;
            foreach (string value in httpRequest.Headers.Values)
            {
                if (arrKeys[x].Trim() == "X-Forwarded-For")
                {
                    var arrIP = value.Split(':');
                    if (arrIP[0] != null)
                    {
                        return arrIP[0];
                    }
                }
                x++;
            }
            return "no ip found";
        }

        public static string GetOperatingSystem(string userAgent)
        {
            if (IsNullOrEmpty(userAgent))
                return userAgent;

            if (Desktop.Any(x => userAgent.Contains(x.Key)))
                return Desktop.First(x => userAgent.Contains(x.Key)).Value;

            return Mobiles.Any(userAgent.Contains)
                ? GetOsVersion(userAgent, Mobiles.First(userAgent.Contains))
                : userAgent;
        }

        private static string GetOsVersion(string userAgent, string osName)
        {
            return userAgent.Split(new[] {osName}, StringSplitOptions.None)[1].Split(';', ')').Length != 0
                ? $"{osName}{userAgent.Split(new[] {osName}, StringSplitOptions.None)[1].Split(';', ')')[0]}"
                : osName;
        }
    }
}
