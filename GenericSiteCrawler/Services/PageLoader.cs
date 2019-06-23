using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Tools;
using NLog;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Services
{
    public class PageLoader
    {
        private static readonly Logger logg = LogManager.GetLogger(nameof(PageLoader));

        public delegate Task MethodContainerPageSuccess(Page downloadedPage, List<string> newLinks);
        public event MethodContainerPageSuccess OnPageSuccess;

        public delegate void MethodContainerError(string message);
        public event MethodContainerError OnError;

        private string Domain { get; set; }

        public PageLoader(string domain)
        {
            Domain = domain;
        }

        public async Task StartLoadingPageAsync(Page page)
        {
            string loadUrl = LinkNormalization.NormalizeUrl(page.Url, Domain);

            var webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
            string html = await webClient.DownloadStringTaskAsync(loadUrl);

            var links = new LinkExtractor(html, Domain).StartExtract();

            var filePath = LinkNormalization.GetFilePathFromUrl(page.Url, Domain);
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            if (!Path.HasExtension(filePath))
                filePath = filePath + ".htm";
            new FileSaver().Save(filePath, LinkReplacer.Replace(html, links, Domain));

            await OnPageSuccess(page, links);
        }

        private void PageDownloader_OnPageError(string link, string message)
        {
            OnError($"{message} | URL=[{link}]");
        }

    }
}