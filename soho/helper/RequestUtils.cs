using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace soho.helper
{
    public static class RequestUtils
    {
        private static IDictionary _urDictionary = new Dictionary<string, string>();
        private static bool _flag = true;

        public static string GetUrl(string method)
        {
            if (_flag)
            {
                lock (_urDictionary)
                {
                    if (_flag)
                    {
                        var inifstream = new FileStream(Directory.GetCurrentDirectory() + "//urlConfig.txt",
                            FileMode.OpenOrCreate);
                        var streamReader = new StreamReader(inifstream);
                        var regex = new Regex("(\\w+)=(.*)");
                        while (!streamReader.EndOfStream)
                        {
                            var urlLine = streamReader.ReadLine();
                            if (urlLine != null)
                            {
                                var match = regex.Match(urlLine);
                                var groupCollection = match.Groups;
                                _urDictionary.Add(groupCollection[1].Value, groupCollection[2].Value);
                            }
                        }
                        _flag = false;
                    }
                }
            }
            return _urDictionary.Contains(method) ? _urDictionary[method].ToString() : "";
        }
    }
}