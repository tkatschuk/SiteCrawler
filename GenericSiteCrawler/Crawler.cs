using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using GenericSiteCrawler.Bootstraping;
using GenericSiteCrawler.Services;

namespace GenericSiteCrawler
{
    public class Crawler
    {
        public delegate void MethodContainerError(string message);
        public event MethodContainerError OnError;

        private List<string> DownloadedPages { get; set; } = new List<string>();

        private bool IsInProcess { get; set; }
        private MainService mainService;
        private string Domain { get; set; }

        public Crawler(string domain)
        {
            //mainService = new MainService(domain);
            //Domain = domain;
        }

        public void Start()
        {
            IsInProcess = true;
            var container = AutofacContainerFactory.GetAutofacContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var page = scope.Resolve<IGenericCrawler>();
                page.TestDB();
            }

            //mainService.OnPageSuccess += MainService_OnPageSuccess;
            //mainService.OnError += MainService_OnError;
            //mainService.Start(Domain);
        }

        private void MainService_OnError(string message)
        {
            OnError(message);
        }

        private void MainService_OnPageSuccess(List<string> newLinks)
        {
            foreach (var link in newLinks)
            {
                if (!DownloadedPages.Contains(link))
                {
                    mainService.Start(link);
                    DownloadedPages.Add(link);
                }
            }
        }

        public void TestDB()
        {
            
        }

    }
}