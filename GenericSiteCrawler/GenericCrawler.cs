using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericSiteCrawler
{
    internal class GenericCrawler : IGenericCrawler
    {
        private readonly IPageService _pageService;
        private readonly IWebsiteService _websiteService;

        public event GenericCrawlerMethodContainerError OnError;

        private MainService mainService;
        private string Domain { get; set; }

        private List<string> DownloadedPages = new List<string>() { };

        public GenericCrawler(
            IPageService pageService,
            IWebsiteService websiteService)
        {
            _pageService = pageService;
            _websiteService = websiteService;
        }

        public async Task StartCrawlingAsync(string domain)
        {
            this.Domain = domain;
            mainService = new MainService(domain);
            mainService.OnPageSuccess += MainService_OnPageSuccess;
            mainService.OnError += MainService_OnError;
            await mainService.StartLoadingPageAsync(Domain);
        }

        private async void MainService_OnPageSuccess(List<string> newLinks)
        {
            foreach (var link in newLinks)
            {
                if (!DownloadedPages.Contains(link))
                {
                    await mainService.StartLoadingPageAsync(link);
                    DownloadedPages.Add(link);
                }
            }
        }

        private void MainService_OnError(string message)
        {
            OnError(message);
        }
    }
}
