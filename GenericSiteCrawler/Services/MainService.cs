using GenericSiteCrawler.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Services
{
    public class MainService
    {
        private static readonly Logger logg = LogManager.GetLogger(nameof(MainService));

        public delegate void MethodContainerPageSuccess(List<string> newLinks);
        public event MethodContainerPageSuccess OnPageSuccess;

        public delegate void MethodContainerError(string message);
        public event MethodContainerError OnError;

        private string Domain { get; set; }
        private WebClient webClient;

        public MainService(string domain)
        {
            webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
            Domain = domain;
        }

        public async Task StartLoadingPageAsync(string url)
        {
            string loadUrl = LinkNormalization.NormalizeUrl(url, Domain);
            logg.Info($"[{url}] -> [{loadUrl}]");

            string html = await webClient.DownloadStringTaskAsync(loadUrl);

            var links = new LinkExtractor(html, Domain).StartExtract();
            foreach (var l in links)
                logg.Info($"[{l}] -> [{LinkNormalization.NormalizeUrl(l, Domain)}]");
            //OnPageSuccess(links);
        }

        /*private void PageDownloader_OnSuccess(string html, string url)
        {
            var filePath = LinkNormalization.GetFilePathFromUrl(url, SiteDomain);
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (!Path.HasExtension(filePath))
                filePath = filePath + ".htm";
            new FileSaver().Save(filePath, LinkReplacer.Replace(html, links, SiteDomain));
        }*/

        private void PageDownloader_OnPageError(string link, string message)
        {
            OnError($"{message} | URL=[{link}]");
        }

    }
}