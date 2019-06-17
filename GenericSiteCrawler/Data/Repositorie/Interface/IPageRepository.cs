using GenericSiteCrawler.Data.Infrastructure;
using GenericSiteCrawler.Data.DomainModel;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace GenericSiteCrawler.Data.Repositories.Interfaces
{
    public interface IPageRepository : IRepository<Page>
    {
        Task<bool> AnyPage(Expression<Func<Page, bool>> where);
    }
}
