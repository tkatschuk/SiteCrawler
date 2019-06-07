using GenericSiteCrawler.Tools;
using System.Collections.Generic;
using System.IO;

namespace GenericSiteCrawler.Services
{
    public class MainService
    {
        public delegate void MethodContainerPageSuccess(List<string> newLinks);
        public event MethodContainerPageSuccess OnPageSuccess;

        public delegate void MethodContainerError(string message);
        public event MethodContainerError OnError;

        private string SiteDomain { get; set; }

        public MainService(string domain)
        {
            SiteDomain = domain;
        }

        public void Start(string url)
        {
            var pageDownloader = new PageDownloader(LinkNormalization.NormalizeUrl(url, SiteDomain));
            pageDownloader.OnSuccess += PageDownloader_OnSuccess;
            pageDownloader.OnPageError += PageDownloader_OnPageError;
            pageDownloader.StartDownload();
        }

        private void PageDownloader_OnSuccess(string html, string url)
        {
            var filePath = LinkNormalization.GetFilePathFromUrl(url, SiteDomain);
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (!Path.HasExtension(filePath))
                filePath = filePath + ".htm";

            var links = new LinkExtractor(html, html).StartExtract();
            new FileSaver().Save(filePath,
                LinkReplacer.Replace(html, links, SiteDomain));

            OnPageSuccess(links);
        }

        private void PageDownloader_OnPageError(string link, string message)
        {
            OnError($"{message} | URL=[{link}]");
        }

    }
}