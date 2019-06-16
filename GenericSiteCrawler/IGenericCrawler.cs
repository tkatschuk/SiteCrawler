using System.Threading.Tasks;

namespace GenericSiteCrawler
{
    delegate void GenericCrawlerMethodContainerError(string message);

    interface IGenericCrawler
    {
        Task StartCrawlingAsync(string domain);
        event GenericCrawlerMethodContainerError OnError;
    }
}
