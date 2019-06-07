using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GenericSiteCrawler.Services
{
    public class LinkExtractor
    {
        private string PageHtml { get; set; }
        private string DomainUrl { get; set; }

        public LinkExtractor(string pageHtml, string domainUrl)
        {
            PageHtml = pageHtml;
            DomainUrl = domainUrl;
        }

        public List<string> StartExtract()
        {
            var result = new List<string>();
            var mathes = Regex.Matches(PageHtml, $"(href|src)=\"(\\S*)\"");
            if (mathes.Count ==0)
                return result;

            foreach(var m in mathes)
            {
                var url = m.ToString();
                if (url.Contains("comsys"))
                {
                    var a = 2;
                }
                if (!string.IsNullOrEmpty(url))
                {
                    url = url.Trim().Remove(0, url.IndexOf("=\"") + 2).TrimEnd('"');
                    if (!string.IsNullOrEmpty(url))
                    {
                        if (!(url.Contains("http") && !url.Contains(DomainUrl)))
                            if (url != "javascript:void(0)" || url[0] == '/' || url.Contains(DomainUrl))
                                result.Add(url);
                    }
                }
            }
            return result.Distinct().ToList();
        }
    }
}
