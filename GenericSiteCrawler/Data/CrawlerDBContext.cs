using System.Data.Entity;
using System.Threading.Tasks;
using GenericSiteCrawler.Data.Configurations;
using GenericSiteCrawler.Data.DomainModel;

namespace GenericSiteCrawler.Data
{
    public class CrawlerDBContext : DbContext
    {
        public CrawlerDBContext() : base("CrawlerDB") { }

        public DbSet<Website> Websites { get; set; }
        public DbSet<Page> Pages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new WebsiteConfiguration());
            modelBuilder.Configurations.Add(new PageConfiguration());
        }

        public static CrawlerDBContext Create()
        {
            return new CrawlerDBContext();
        }

        public virtual void Commit()
        {
            SaveChanges();
        }

        public virtual async Task CommitAsync()
        {
            await SaveChangesAsync();
        }
    }
}
