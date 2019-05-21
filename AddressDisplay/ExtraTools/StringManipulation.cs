using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AddressDisplay.ExtraTools {
    public static class StringManipulation {

        public static string RegexExtract (string start, string end, string source) {
            Regex regex = new Regex(start + "(.*?)" + end);
            Match match = regex.Match(source);
            return match.Groups[1].ToString();
        }

        public static string RemoveHttps(string url) {
            return RegexExtract("https://", "/", url);
        }

    }
}
