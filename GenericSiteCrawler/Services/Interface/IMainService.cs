using GenericSiteCrawler.Models;

namespace GenericSiteCrawler.Services.Interface
{
    delegate void MainServiceMethodContainerError(string message);
    delegate void MainServiceMethodContainerCrawling(CrawlingProgress data);

    interface IMainService
    {
        void StartCrawling(string domain);
        event MainServiceMethodContainerError OnError;
        event MainServiceMethodContainerCrawling OnMainServiceProgressChanged;
    }
}
