using GenericSiteCrawler.Data.DomainModel;
using GenericSiteCrawler.Data.Infrastructure;
using GenericSiteCrawler.Data.Repositories.Interfaces;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Data.Repositories
{
    public class PageRepository : RepositoryBase<Page>, IPageRepository
    {
        public PageRepository(IDbFactory dbFactory) : base(dbFactory)
        {            
        }

        public async Task<bool> AnyPage(Expression<Func<Page, bool>> where)
        {
            return await DbContext.Pages.AnyAsync(where);
        }
    }
}
