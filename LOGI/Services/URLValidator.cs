using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LOGI.Services
{
    public class URLValidator
    {
        public static bool IsValidURLPart(string url)
        {
            return Regex.IsMatch(url, "^[\\w-]+$");
        }

        public static string CleanURL(string url)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            url = regex.Replace(url, @" ");

            return url.Trim().Replace(" ", "-");
        }
    }
}