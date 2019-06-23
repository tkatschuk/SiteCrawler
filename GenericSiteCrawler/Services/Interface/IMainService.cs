using System.Threading.Tasks;

namespace GenericSiteCrawler.Services.Interface
{
    delegate void GenericCrawlerMethodContainerError(string message);

    interface IMainService
    {
        void StartCrawlingAsync(string domain);
        event GenericCrawlerMethodContainerError OnError;
    }
}
