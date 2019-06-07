using System.IO;

namespace GenericSiteCrawler.Tools
{
    public static class LinkNormalization
    {
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
            link = link.Replace("https://", "").Replace("http://", "").Replace(domain, "");
            if (string.IsNullOrEmpty(link))
                link = "index.html";
            if (link[0] == '/')
                link = link.Remove(0, 1);
            return (isRelative ? "" : "domain\\") + link.Replace('/', '\\');
        }
    }
}
