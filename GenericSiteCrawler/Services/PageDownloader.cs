using System;
using System.Net;
using System.Text;

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

        public void StartDownload()
        {
            var client = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
            client.DownloadStringCompleted += Client_DownloadStringCompleted;
            var url = new Uri(PageUrl);
            client.DownloadStringAsync(url);
        }

        private void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnPageError(PageUrl, e.Error.Message);
                return;
            }
            if (string.IsNullOrEmpty(e.Result))
            {
                OnPageError(PageUrl, $"Page is empty");
                return;
            }

            OnSuccess(e.Result, PageUrl);
        }
    }
}
