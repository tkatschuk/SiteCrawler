using GenericSiteCrawler.Models;
using GenericSiteCrawler.Services.Interface;
using GenericSiteCrawler.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;

namespace GenericSiteCrawler.Services
{
    internal class MainService : IMainService
    {
        public event MainServiceMethodContainerError OnError;
        public event MainServiceMethodContainerCrawling OnMainServiceProgressChanged;
        public event MainServiceMethodContainerCompleted OnMainServiceProgressCompleted;

        private string Domain { get; set; }

        private List<string> downloadedPages = new List<string>() { };
        private List<string> pagesToDownloading = new List<string>() { };
        private List<string> downloadingPages = new List<string>() { };
        private List<string> pagesWithError = new List<string>() { };

        private bool process = true;

        public void StartCrawling(string domain)
        {
            this.Domain = domain;

            pagesToDownloading.Add(domain);

            var processTimer = new System.Timers.Timer(1000);
            processTimer.Elapsed += (Object s, ElapsedEventArgs e) =>
            {
                if (downloadedPages.Count > 0 &&
                    downloadingPages.Count == 0 &&
                    pagesToDownloading.Count == 0)
                {
                    process = false;
                    processTimer.Enabled = false;
                }
            };
            processTimer.Enabled = true;

            while (process)
            {
                if (downloadingPages.Count < 10)
                {
                    if (pagesToDownloading.Count > 0) // if any url exist to downloading
                    {
                        var page = pagesToDownloading.First();
                        pagesToDownloading.Remove(page);
                        downloadingPages.Add(page);
                        new Thread(StartDownloadingPage).Start(page);
                        UpdateProgress();
                    }
                }
            }
            OnMainServiceProgressCompleted();
        }

        private async void StartDownloadingPage(object _pageUrl)
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
                    var html = await webClient.DownloadStringTaskAsync(loadUrl);
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
                    await webClient.DownloadFileTaskAsync(loadUrl, filePath);
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
            UpdateProgress();
        }

        private void SetPageDownloaded(string pageUrl)
        {
            downloadingPages.Remove(pageUrl);
            downloadedPages.Add(pageUrl);
            UpdateProgress();
        }

        private void MainService_OnError(string message)
        {
            OnError(message);
        }

        private void UpdateProgress()
        {
            OnMainServiceProgressChanged(
                new CrawlingProgress(
                    pagesToDownloading.Count,
                    downloadingPages.Count,
                    downloadedPages.Count,
                    pagesWithError.Count));
        }
    }
}
