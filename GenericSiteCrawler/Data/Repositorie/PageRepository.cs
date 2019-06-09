using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Infrastructure;
using GenericSiteCrawler.Data.Repositories.Interfaces;

namespace GenericSiteCrawler.Data.Repositories
{
    public class PageRepository : RepositoryBase<Page>, IPageRepository
    {
        public PageRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
