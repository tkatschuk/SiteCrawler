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

            // DB Factory
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(WebsiteRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            // Services
            builder.RegisterAssemblyTypes(typeof(WebsiteService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
