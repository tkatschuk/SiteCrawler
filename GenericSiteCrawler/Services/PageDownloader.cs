using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Services
{
    public class PageDownloader
    {
        public delegate void MethodContainerSuccess(string html, string url);
        public event MethodContainerSuccess OnSuccess;

        public delegate void MethodContainerPageError(string link, string message);
        public event MethodContainerPageError OnPageError;

        private string PageUrl { get; set; }

        public PageDownloader(string pageUrl)
        {
            PageUrl = pageUrl;
        }

        public async Task StartDownload()
        {
            var client = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
            string html = await client.DownloadStringTaskAsync(PageUrl);
            OnSuccess(html, PageUrl);
        }
    }
}
