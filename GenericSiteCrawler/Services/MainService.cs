using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Services.Interface;
using GenericSiteCrawler.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Services
{
    internal class MainService : IMainService
    {
        private readonly IPageService _pageService;
        private readonly IWebsiteService _websiteService;

        public event GenericCrawlerMethodContainerError OnError;

        private Website webSite = null;

        private PageLoader mainService;
        private string Domain { get; set; }

        public MainService(
            IPageService pageService,
            IWebsiteService websiteService)
        {
            _pageService = pageService;
            _websiteService = websiteService;
        }

        private List<string> downloadedPages = new List<string>() { };
        private List<string> pagesToDownloading = new List<string>() { };
        private List<string> downloadingPages = new List<string>() { };
        private List<string> pagesWithError = new List<string>() { };

        public void StartCrawling(string domain)
        {
            this.Domain = domain;
            //mainService = new PageLoader(domain);
            //mainService.OnError += MainService_OnError;

            pagesToDownloading.Add(domain);

            new Thread(MainThread).Start();
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
        }

        private void MainThread()
        {
            while (true)
            {
                if (downloadingPages.Count < 33)
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
                    foreach (var link in links)
                        if (downloadedPages.Contains(link) == false &&
                            downloadingPages.Contains(link) == false &&
                            pagesToDownloading.Contains(link) == false &&
                            pagesWithError.Contains(link) == false)
                        {
                            pagesToDownloading.Add(link);
                        }
                    downloadingPages.Remove(pageUrl);
                    downloadedPages.Add(pageUrl);
                }
                catch(Exception ex)
                {
                    downloadingPages.Remove(pageUrl);
                    pagesWithError.Add(pageUrl);
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {pageUrl} | ERROR [{ex.Message}]");
                }
            }
            else
            {
                try
                {
                    webClient.DownloadFile(loadUrl, filePath);
                    downloadingPages.Remove(pageUrl);
                    downloadedPages.Add(pageUrl);
                }
                catch(Exception ex)
                {
                    downloadingPages.Remove(pageUrl);
                    pagesWithError.Add(pageUrl);
                    OnError($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {pageUrl} | ERROR [{ex.Message}]");
                }
            }
            
        }


        public async Task StartCrawlingAsync(string domain)
        {
            this.Domain = domain;
            mainService = new PageLoader(domain);
            mainService.OnPageSuccess += MainService_OnPageSuccess;
            mainService.OnError += MainService_OnError;

            webSite = new Website()
            {
                EnterUrl = domain
            };
            _websiteService.CreateWebsite(webSite);
            try
            {
                await _websiteService.SaveWebsiteAsync();
            }
            catch(Exception ex)
            {
                OnError($"Error by saving webSite data to DB [{ex.Message}]");
            }
            var domainPage = new Page()
            {
                Url = domain,
                Website = webSite
            };
            _pageService.CreatePage(domainPage);
            try
            {
                await _pageService.SavePageAsync();
            }
            catch (Exception ex)
            {
                OnError($"Error to save page [{domain}] to BD [{ex.Message}]");
            }

            await mainService.StartLoadingPageAsync(domainPage);
        }

        private async Task MainService_OnPageSuccess(Page downloadedPage, List<string> newLinks)
        {
            downloadedPage.Downloaded = true;
            _pageService.UpdatePage(downloadedPage);
            try
            {
                await _pageService.SavePageAsync();
            }
            catch(Exception ex)
            {
                OnError($"Error to set for page [{downloadedPage.Url}] status 'DOWNLOADED' in BD [{ex.Message}]");
            }

            foreach (var link in newLinks)
                if (await _pageService.PageExist(webSite.Id, link) == false)
                {
                    var page = new Page()
                    {
                        Website = webSite,
                        Url = link
                    };
                    _pageService.CreatePage(page);
                    try
                    {
                        await _pageService.SavePageAsync();
                    }
                    catch (Exception ex)
                    {
                        OnError($"Error to save page [{link}] to BD [{ex.Message}]");
                    }
                    await mainService.StartLoadingPageAsync(page);
                }
        }

        private void MainService_OnError(string message)
        {
            OnError(message);
        }
    }
}
