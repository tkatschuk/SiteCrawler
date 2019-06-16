using System.Threading.Tasks;

namespace GenericSiteCrawler
{
    internal interface IGenericCrawler
    {
        Task StartCrawlingAsync(string domain);
    }
}
