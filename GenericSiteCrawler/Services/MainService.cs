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

        private string Domain { get; set; }

        private List<string> downloadedPages = new List<string>() { }; //List of successful downloaded pages
        private List<string> pagesToDownloading = new List<string>() { }; //List of waiting to download pages
        private List<string> downloadingPages = new List<string>() { }; //List of now downloading pages
        private List<string> pagesWithError = new List<string>() { }; //List of downloaded pages with errors

        private bool process = true; //Param for downloading progress

        /// <summary>
        /// Start main process of crawling
        /// </summary>
        /// <param name="domain">Main (start) page for crawling</param>
        public void StartCrawling(string domain)
        {
            this.Domain = domain;

            pagesToDownloading.Add(domain);

            var processTimer = new System.Timers.Timer(1000); //Timer for checking progress of crawling
            processTimer.Elapsed += (Object s, ElapsedEventArgs e) =>
            {
                if (downloadedPages.Count > 0 &&
                    downloadingPages.Count == 0 &&
                    pagesToDownloading.Count == 0) //If no pages for crawling
                {
                    process = false; //Stop progress of crawling
                    processTimer.Enabled = false;
                }
            };
            processTimer.Enabled = true;

            while (process) //If process of crawling is enabled
            {
                if (downloadingPages.Count < 10) //Maximal count of thread
                {
                    if (pagesToDownloading.Count > 0) //If any url exist to downloading
                    {
                        var page = pagesToDownloading.First(); //Get first page to downloading in queue
                        pagesToDownloading.Remove(page); //Remove this page from queue of pages, which wait to downloading
                        downloadingPages.Add(page); //Add this page to list for downloading now pages
                        new Thread(StartDownloadingPage).Start(page); //Start new Thread for downloading this page
                        UpdateProgress();
                    }
                }
            }
        }

        /// <summary>
        /// Start downloading page in new thread
        /// </summary>
        /// <param name="_pageUrl">Url of page</param>
        private async void StartDownloadingPage(object _pageUrl)
        {
            string pageUrl = (string)_pageUrl;

            string loadUrl = LinkNormalization.NormalizeUrl(pageUrl, Domain); //Normalize pages url
            var filePath = LinkNormalization.GetFilePathFromUrl(pageUrl, Domain); //Get path of file to save this page
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };

            if (filePath.EndsWith(".htm") || filePath.EndsWith(".html")) //If this page is any HTML-page
            {
                try
                {
                    var html = await webClient.DownloadStringTaskAsync(loadUrl); //Download page (get HTML)
                    var links = new LinkExtractor(html, Domain).StartExtract(); //Get all link in this HTML of page
                    File.WriteAllText(filePath, LinkReplacer.Replace(html, links, Domain)); //Save this page to disk and replace all links

                    SetPageDownloaded(pageUrl);

                    foreach (var link in links) //Check, if link already in our lists of pages
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
            else //If this page is any file (image, css, js, ...)
            {
                try
                {
                    await webClient.DownloadFileTaskAsync(loadUrl, filePath); //Download and save file
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
