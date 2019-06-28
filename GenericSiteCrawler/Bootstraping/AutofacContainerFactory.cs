using Autofac;
using GenericSiteCrawler.Services;
using GenericSiteCrawler.Services.Interface;

namespace GenericSiteCrawler.Bootstraping
{
    public class AutofacContainerFactory
    {
        public static IContainer GetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainService>().As<IMainService>();

            return builder.Build();
        }
    }
}
