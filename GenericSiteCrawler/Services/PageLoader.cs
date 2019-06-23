using GenericSiteCrawler.Data.DomainModel;
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
    public class PageLoader
    {
        private static readonly Logger logg = LogManager.GetLogger(nameof(PageLoader));

        public delegate void MethodContainerPageSuccess(Page downloadedPage, List<string> newLinks);
        public event MethodContainerPageSuccess OnPageSuccess;

        public delegate void MethodContainerError(string message);
        public event MethodContainerError OnError;

        private string Domain { get; set; }

        public PageLoader(string domain)
        {
            Domain = domain;
        }

        public bool StartLoading(string Url, out List<string> links)
        {
            links = new List<string>() { };
            //OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {Url}");

            string loadUrl = LinkNormalization.NormalizeUrl(Url, Domain);
            var webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
            var filePath = LinkNormalization.GetFilePathFromUrl(Url, Domain);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (filePath.EndsWith(".htm") || filePath.EndsWith(".html"))
            {
                try
                {
                    var html = webClient.DownloadString(loadUrl);
                    links = new LinkExtractor(html, Domain).StartExtract();
                    File.WriteAllText(filePath, LinkReplacer.Replace(html, links, Domain));
                    return true;
                }
                catch (Exception ex)
                {
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {Url} | ERROR [{ex.Message}]");
                }
            }
            else
            {
                try
                {
                    webClient.DownloadFile(loadUrl, filePath);
                }
                catch (Exception ex)
                {
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {Url} | ERROR [{ex.Message}]");
                }
            }
            return false;
        }

        /*
        public void StartLoadingPageAsync(Page page)
        {
            OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {page.Url}");
            string loadUrl = LinkNormalization.NormalizeUrl(page.Url, Domain);

            var webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };

            var filePath = LinkNormalization.GetFilePathFromUrl(page.Url, Domain);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (filePath.EndsWith(".htm") || filePath.EndsWith(".html"))
            {
                try
                {
                    var html = webClient.DownloadString(loadUrl);
                    var links = new LinkExtractor(html, Domain).StartExtract();
                    File.WriteAllText(filePath, LinkReplacer.Replace(html, links, Domain));
                    OnPageSuccess(page, links);
                }
                catch(Exception ex)
                {
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {page.Url} | ERROR [{ex.Message}]");
                }
            }
            else
            {
                try
                {
                    webClient.DownloadFile(loadUrl, filePath);
                }
                catch(Exception ex)
                {
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {page.Url} | ERROR [{ex.Message}]");
                }
            }
        }
        */

        private void PageDownloader_OnPageError(string link, string message)
        {
            OnError($"{message} | URL=[{link}]");
        }

    }
}