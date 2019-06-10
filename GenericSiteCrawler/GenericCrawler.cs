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
            var pages = await _pageService.GetAllPagesAsync(0);
            Console.WriteLine($"pages = {pages.Count()}");
        }
    }
}
