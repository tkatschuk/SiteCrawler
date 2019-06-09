using System;

namespace GenericSiteCrawler.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        CrawlerDBContext Init();
    }
}
