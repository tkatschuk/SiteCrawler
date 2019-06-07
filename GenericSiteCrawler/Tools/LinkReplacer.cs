using System.Collections.Generic;

namespace GenericSiteCrawler.Tools
{
    public static class LinkReplacer
    {
        public static string Replace(string html, List<string> links, string domain)
        {
            foreach(var link in links)
            {
                html = html.Replace(link, LinkNormalization.GetFilePathFromUrl(link, domain, true));
            }
            return html;
        }
    }
}
