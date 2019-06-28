using GenericSiteCrawler.Services.Interface;
using GenericSiteCrawler.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace GenericSiteCrawler.Services
{
    internal class MainService : IMainService
    {
        public event GenericCrawlerMethodContainerError OnError;

        private string Domain { get; set; }

        private List<string> downloadedPages = new List<string>() { };
        private List<string> pagesToDownloading = new List<string>() { };
        private List<string> downloadingPages = new List<string>() { };
        private List<string> pagesWithError = new List<string>() { };

        public void StartCrawling(string domain)
        {
            this.Domain = domain;

            pagesToDownloading.Add(domain);

            MainThread();

            /*
            bool process = true;
            while (process)
            {
                Console.Title = $"{pagesToDownloading.Count}   >   {downloadingPages.Count}   >   {downloadedPages.Count} [with errors {pagesWithError.Count}]";

                if (pagesToDownloading.Count == 0 && downloadingPages.Count == 0)
                {
                    process = false;
                    foreach(var link in downloadedPages)
                    {
                        Console.WriteLine($"+ {link}");
                    }
                    foreach (var link in pagesWithError)
                    {
                        Console.WriteLine($"- {link}");
                    }
                }
                Thread.Sleep(500);
            }
            */
        }

        private void MainThread()
        {
            while (true)
            {
                if (downloadingPages.Count < 10)
                {
                    if (pagesToDownloading.Count > 0) // if any url exist to downloading
                    {
                        var page = pagesToDownloading.First();
                        pagesToDownloading.Remove(page);
                        downloadingPages.Add(page);
                        new Thread(StartDownloadingPage).Start(page);
                    }
                }
            }
        }

        private void StartDownloadingPage(object _pageUrl)
        {
            string pageUrl = (string)_pageUrl;

            string loadUrl = LinkNormalization.NormalizeUrl(pageUrl, Domain);
            var filePath = LinkNormalization.GetFilePathFromUrl(pageUrl, Domain);
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };

            if (filePath.EndsWith(".htm") || filePath.EndsWith(".html"))
            {
                try
                {
                    var html = webClient.DownloadString(loadUrl);
                    var links = new LinkExtractor(html, Domain).StartExtract();
                    File.WriteAllText(filePath, LinkReplacer.Replace(html, links, Domain));

                    SetPageDownloaded(pageUrl);

                    foreach (var link in links)
                        if (downloadedPages.Contains(link) == false &&
                            pagesToDownloading.Contains(link) == false &&
                            downloadingPages.Contains(link) == false &&
                            pagesWithError.Contains(link) == false)
                        {
                            pagesToDownloading.Add(link);
                        }
                }
                catch(Exception ex)
                {
                    SetPageError(pageUrl);
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {pageUrl} | ERROR [{ex.Message}]");
                }
            }
            else
            {
                try
                {
                    webClient.DownloadFile(loadUrl, filePath);
                    SetPageDownloaded(pageUrl);
                }
                catch(Exception ex)
                {
                    SetPageError(pageUrl);
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {pageUrl} | ERROR [{ex.Message}]");
                }
            }            
        }

        private void SetPageError(string pageUrl)
        {
            downloadingPages.Remove(pageUrl);
            pagesWithError.Add(pageUrl);
        }

        private void SetPageDownloaded(string pageUrl)
        {
            downloadingPages.Remove(pageUrl);
            downloadedPages.Add(pageUrl);
        }

        private void MainService_OnError(string message)
        {
            OnError(message);
        }
    }
}
