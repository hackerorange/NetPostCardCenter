using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hacker.Inko.Net.Utils
{
    public static class JsonHelper
    {
        public static string ToJsonString(this Dictionary<string, string> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }

        public static string ToJsonString<T>(this List<T> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }
    }
}