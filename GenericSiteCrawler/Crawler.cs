using Autofac;
using GenericSiteCrawler.Bootstraping;
using GenericSiteCrawler.Tools;
using System;
using System.Threading.Tasks;

namespace GenericSiteCrawler
{
    public class Crawler
    {
        public delegate void MethodContainerError(string message);
        public event MethodContainerError OnError;        

        public async Task StartAsync(string Url)
        {
            string domain;
            try
            {
                domain = LinkNormalization.GetDomain(Url);
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                return;
            }

            var container = AutofacContainerFactory.GetAutofacContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var crawler = scope.Resolve<IGenericCrawler>();
                crawler.OnError += Crawler_OnError;
                await crawler.StartCrawlingAsync(domain);
            }
        }

        private void Crawler_OnError(string message)
        {
            OnError(message);
        }
    }
}