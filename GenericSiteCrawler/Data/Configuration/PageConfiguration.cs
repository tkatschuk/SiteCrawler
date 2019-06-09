using GenericSiteCrawler.Data.DomainModel;
using System.Data.Entity.ModelConfiguration;

namespace GenericSiteCrawler.Data.Configurations
{
    public class PageConfiguration : EntityTypeConfiguration<Page>
    {
        public PageConfiguration()
        {
            ToTable("Pages", "Data");
        }
    }
}
