using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Services.Interface;
using System;
using System.Collections.Generic;
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

        private PageLoader pageLoader;
        private string Domain { get; set; }

        public MainService(
            IPageService pageService,
            IWebsiteService websiteService)
        {
            _pageService = pageService;
            _websiteService = websiteService;
        }

        public void StartCrawlingAsync(string domain)
        {
            //this.Domain = domain;
            pageLoader = new PageLoader(domain);
            //pageLoader.OnPageSuccess += MainService_OnPageSuccess;
            pageLoader.OnError += MainService_OnError;

            webSite = new Website()
            {
                EnterUrl = domain
            };
            _websiteService.CreateWebsite(webSite);
            try
            {
                _websiteService.SaveWebsite();
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
                _pageService.SavePage();
            }
            catch (Exception ex)
            {
                OnError($"Error to save page [{domain}] to BD [{ex.Message}]");
            }
            Work(domainPage.Url);
        }

        private List<string> LoadedLinks = new List<string>() { };

        private void Work(string url)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {url}");

            LoadedLinks.Add(url);
            var links = new List<string>();
            if (pageLoader.StartLoading(url, out links)) // if has links for next pages
            {
                foreach(var p in links)
                {
                    if (LoadedLinks.Contains(p) == false)
                    {
                        //Work(p);

                        var thread = new Thread(() =>
                        {
                            Work(p);
                        });
                        thread.Start();
                    }
                }
            }
        }

        private void MainService_OnPageSuccess(Page downloadedPage, List<string> newLinks)
        {
            downloadedPage.Downloaded = true;
            _pageService.UpdatePage(downloadedPage);
            try
            {
                _pageService.SavePage();
            }
            catch(Exception ex)
            {
                OnError($"Error to set for page [{downloadedPage.Url}] status 'DOWNLOADED' in BD [{ex.Message}]");
            }

            foreach (var link in newLinks)
                if (_pageService.PageExist(webSite.Id, link) == false)
                {
                    var page = new Page()
                    {
                        Website = webSite,
                        Url = link
                    };
                    _pageService.CreatePage(page);
                    try
                    {
                        _pageService.SavePage();
                    }
                    catch (Exception ex)
                    {
                        OnError($"Error to save page [{link}] to BD [{ex.Message}]");
                    }

                    //pageLoader.StartLoadingPageAsync(page);

                    var thread = new Thread(() =>
                    {
                        //pageLoader.StartLoadingPageAsync(page);
                    });
                    thread.Start();
                    thread.Join();

                    //var thread = new Thread(SendPageToPageLoader);
                    //thread.StartAsync(page);
                    //await pageLoader.StartLoadingPageAsync(page);
                }
        }

        /*
        private async void SendPageToPageLoader(object pageObject)
        {
            var page = (Page)pageObject;
            await pageLoader.StartLoadingPageAsync(page);
        }*/

        private void MainService_OnError(string message)
        {
            OnError(message);
        }
    }
}
