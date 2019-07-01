using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GenericSiteCrawler.Services
{
    public class LinkExtractor
    {
        private string PageHtml { get; set; }
        private string Domain { get; set; }

        public LinkExtractor(string pageHtml, string domain)
        {
            PageHtml = pageHtml;
            Domain = domain;
        }

        public List<string> StartExtract()
        {
            var result = new List<string>();
            var matches = Regex.Matches(PageHtml, $"(href|src)=\"(\\S*)\"");
            if (matches.Count == 0)
                return result;

            foreach (var m in matches)
            {
                var url = m.ToString();
                url = url.Substring(url.IndexOf('"') + 1, url.LastIndexOf('"') - url.IndexOf('"') - 1).Trim();

                if (MatchLink(url))
                    result.Add(url);
            }
            return result.Distinct().ToList();
        }

        private bool MatchLink(string link)
        {
            if (string.IsNullOrEmpty(link))
                return false;
            if (link == "/")
                return false;

            if (link.Contains("http"))
                link = link.Replace("http://", "").Replace("https://", "");
            if (link.IndexOf(Domain) == 0 || link.IndexOf($"www.{Domain}") == 0)
                return true;

            if (link[0] == '/')
                return true;

            return false;
        }
    }
}
