using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GenericSiteCrawler
{
    public class GenericCrawler : IGenericCrawler
    {
        private readonly IPageService _pageService;

        public GenericCrawler(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task TestDB()
        {
            Console.WriteLine($"TestDB");
            var pages = await _pageService.GetAllPagesAsync(1);
            Console.WriteLine($"pages = {pages.Count()}");
            _pageService.CreatePage(new Page()
            {
                Url = "test url 2",
                Website = new Website()
                {
                    EnterUrl = "sdfsdfsdf3"
                }
            });
            await _pageService.SavePageAsync();
            Console.WriteLine("Saved");
        }
    }
}
