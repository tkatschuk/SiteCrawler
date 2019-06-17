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

        private Website webSite = null;

        private MainService mainService;
        private string Domain { get; set; }

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
