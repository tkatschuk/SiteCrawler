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

        public void Start(string Url)
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
            var scope = container.BeginLifetimeScope();

            var mainService = scope.Resolve<IMainService>();
            mainService.OnError += (string message) =>
            {
                OnError(message);
            };
            mainService.OnMainServiceProgressChanged += (CrawlingProgress data) =>
            {
                OnCrawlingProgress(data);
            };
            mainService.OnMainServiceProgressCompleted += () =>
            {
                OnCrawlingProgressCompleted();
            };

            mainService.StartCrawling(domain);
        }
    }
}