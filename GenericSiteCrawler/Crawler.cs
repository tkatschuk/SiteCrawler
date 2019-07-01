using Autofac;
using GenericSiteCrawler.Bootstraping;
using GenericSiteCrawler.Models;
using GenericSiteCrawler.Services.Interface;
using GenericSiteCrawler.Tools;
using System;

namespace GenericSiteCrawler
{
    public class Crawler
    {
        public delegate void MethodContainerCrawling(CrawlingProgress data);
        public event MethodContainerCrawling OnCrawlingProgress;

        public delegate void MethodContainerCompleted();
        public event MethodContainerCompleted OnCrawlingProgressCompleted;

        public delegate void MethodContainerError(string message);
        public event MethodContainerError OnError;

        /// <summary>
        /// Start crawling
        /// </summary>
        /// <param name="Url">Domain</param>
        public void Start(string Url)
        {
            string domain;
            try
            {
                domain = LinkNormalization.GetDomain(Url); //Try get domain-url
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                return;
            }

            var container = AutofacContainerFactory.GetAutofacContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var mainService = scope.Resolve<IMainService>(); //Get mainService-object
                mainService.OnError += (string message) =>
                {
                    OnError(message);
                };
                mainService.OnMainServiceProgressChanged += (CrawlingProgress data) => //Send event "Progress of crawling changed"
                {
                    OnCrawlingProgress(data);
                };

                mainService.StartCrawling(domain); //Start  crawling
                OnCrawlingProgressCompleted();
            }
        }
    }
}