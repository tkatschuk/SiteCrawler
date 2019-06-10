using GenericSiteCrawler.Data.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Data.Service.Interface
{
    public interface IPageService
    {
        Task<IEnumerable<Page>> GetAllPagesAsync(int webSiteId);
    }
}
