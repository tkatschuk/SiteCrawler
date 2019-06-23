using GenericSiteCrawler.Data.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Data.Service.Interface
{
    public interface IPageService
    {
        Task<IEnumerable<Page>> GetAllPagesAsync(int webSiteId);
        void CreatePage(Page page);
        void UpdatePage(Page page);
        void SavePage();
        Task SavePageAsync();
        Task<bool> PageExistAsync(int webSiteId, string url);
        bool PageExist(int webSiteId, string url);
    }
}
