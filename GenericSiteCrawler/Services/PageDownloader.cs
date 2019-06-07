using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Services
{
    public class PageDownloader
    {
        private static readonly Logger logg = LogManager.GetLogger(nameof(PageDownloader));

        private string PageUrl { get; set; }

        public PageDownloader(string url)
        {
            PageUrl = url;
        }

        public void Download()
        {
            var client = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
            client.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                    ThrowError($"{e.Error.Message} by downloading [{PageUrl}]");
                if (string.IsNullOrEmpty(e.Result))
                    ThrowError($"Result is empty by downloading [{PageUrl}]");


            };
            var url = new Uri(PageUrl);
            client.DownloadStringAsync(url);
        }

        private void ThrowError(string message)
        {
            logg.Error(message);
            throw new Exception(message);
        }
    }
}
