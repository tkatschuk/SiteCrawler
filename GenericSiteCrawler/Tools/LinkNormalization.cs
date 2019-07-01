using System;
using System.IO;

namespace GenericSiteCrawler.Tools
{
    public static class LinkNormalization
    {
        public static string GetDomain(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("URL is empty");

            string[] replace = new string[] { "www.", "http:", "https:", "//" };
            foreach (var str in replace)
                if (url.Contains(str))
                    url = url.Replace(str, "");
            if (url.Contains("/"))
                url = url.Substring(0, url.IndexOf('/'));

            if (string.IsNullOrEmpty(url) || url.Contains(".") == false)
                throw new Exception("URL contains no domain");
            return url;
        }

        public static string NormalizeUrl(string link, string domain)
        {
            if (link.Contains("http://") || link.Contains("https://"))
                return link;

            if (!link.Contains(domain))
            {
                if (link[0] != '/')
                    link = "/" + link;
                link = domain + link;
            }

            return "http://" + link;
        }

        public static string GetFilePathFromUrl(string link, string domain, bool isRelative = false)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;

            link = link.Replace("https://", "").Replace("http://", "").Replace(domain, "").Replace('?', '_').Replace('#', '_');
            if (string.IsNullOrEmpty(link))
                link = "index.htm";

            if (link.Contains("\""))
                link = link.Remove(link.IndexOf('"'), link.Length - link.IndexOf('"'));

            if (link[0] == '/')
                link = link.Remove(0, 1);

            link = Path.Combine(rootPath, "domain", link);

            if (!Path.HasExtension(link))
                link = link + ".htm";

            return link;
        }
    }
}
