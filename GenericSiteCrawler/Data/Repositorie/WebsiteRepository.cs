using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Infrastructure;
using GenericSiteCrawler.Data.Repositories.Interfaces;

namespace GenericSiteCrawler.Data.Repositories
{
    public class WebsiteRepository : RepositoryBase<Website>, IWebsiteRepository
    {
        public WebsiteRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
