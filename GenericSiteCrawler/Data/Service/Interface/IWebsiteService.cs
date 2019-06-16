using GenericSiteCrawler.Data.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Data.Service.Interface
{
    public interface IWebsiteService
    {
        Task<IEnumerable<Website>> GetAllWebsitesAsync();

        void CreateWebsite(Website website);

        void SaveWebsite();
        Task SaveWebsiteAsync();
    }
}
