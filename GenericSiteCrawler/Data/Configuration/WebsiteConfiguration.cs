using GenericSiteCrawler.Data.DomainModel;
using System.Data.Entity.ModelConfiguration;

namespace GenericSiteCrawler.Data.Configurations
{
    public class WebsiteConfiguration : EntityTypeConfiguration<Website>
    {
        public WebsiteConfiguration()
        {
            ToTable("Websites", "Data");
        }
    }
}
