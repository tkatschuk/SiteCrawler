using Autofac;
using GenericSiteCrawler.Data.Infrastructure;
using GenericSiteCrawler.Data.Repositories;
using GenericSiteCrawler.Data.Service;

namespace GenericSiteCrawler.Bootstraping
{
    public class AutofacContainerFactory
    {
        public static IContainer GetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<GenericCrawler>().As<IGenericCrawler>();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<DbFactory>().As<IDbFactory>();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(WebsiteRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            // Services
            builder.RegisterAssemblyTypes(typeof(WebsiteService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            return builder.Build();
        }
    }
}
