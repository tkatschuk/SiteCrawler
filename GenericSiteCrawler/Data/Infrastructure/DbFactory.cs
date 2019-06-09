namespace GenericSiteCrawler.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private CrawlerDBContext dbContext;

        public CrawlerDBContext Init()
        {
            return dbContext ?? (dbContext = new CrawlerDBContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
