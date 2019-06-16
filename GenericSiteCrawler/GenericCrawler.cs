using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GenericSiteCrawler
{
    internal class GenericCrawler : IGenericCrawler
    {
        private readonly IPageService _pageService;
        private readonly IWebsiteService _websiteService;

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
            mainService = new MainService(Domain);
            mainService.OnPageSuccess += MainService_OnPageSuccess;
            mainService.OnError += MainService_OnError;
            mainService.Start(Domain);
        }

        private void MainService_OnPageSuccess(List<string> newLinks)
        {
            foreach (var link in newLinks)
            {
                if (!DownloadedPages.Contains(link))
                {
                    mainService.Start(link);
                    DownloadedPages.Add(link);
                }
            }
        }

        private void MainService_OnError(string message)
        {
            OnError(message);
        }

        public async Task TestDB()
        {
            Console.WriteLine($"TestDB");
            var sites = await _websiteService.GetAllWebsitesAsync();
            Console.WriteLine($"sites = {sites.Count()}");
            _websiteService.CreateWebsite(new Website()
            {
                EnterUrl = $"Test from {DateTime.Now.ToString("HH:mm:ss.fff")}"
            });
            _websiteService.SaveWebsite();
            Console.WriteLine("Saved");
        }
    }
}
