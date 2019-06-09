using System.Threading.Tasks;

namespace GenericSiteCrawler.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private CrawlerDBContext _dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public CrawlerDBContext DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.Commit();
        }

        public async Task CommitAsync()
        {
            await DbContext.CommitAsync();
        }
    }
}
