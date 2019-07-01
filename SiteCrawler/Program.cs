using GenericSiteCrawler;
using NLog;
using System;

namespace SiteCrawler
{
    class Program
    {
        private static readonly Logger logg = LogManager.GetLogger(nameof(Program));

        static void Main(string[] args)
        {
            string url = "avpv.kz";
            if (args.Length > 0)
                url = args[0];
            else
            {
                Console.WriteLine("Enter your site domain, e.g. reply.de");
                url = Console.ReadLine();
            }

            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine($"Domain is empty");
                return;
            }

            var crawler = new Crawler();
            crawler.OnError += Crawler_OnError;
            crawler.OnCrawlingProgress += Crawler_OnCrawlingProgress;
            crawler.OnCrawlingProgressCompleted += Crawler_OnCrawlingProgressCompleted;
            crawler.Start(url);
            Console.Write("Press any key for exit...");
            Console.ReadKey();
        }

        private static void Crawler_OnCrawlingProgressCompleted()
        {
            Console.WriteLine("Crawling completed");
        }

        private static void Crawler_OnCrawlingProgress(GenericSiteCrawler.Models.CrawlingProgress data)
        {
            Console.Title = $"Progress {data.ProgressInProcent}%    [Finded pages: {data.PagesFounded} | downloaded: {data.PagesDownloaded} | waiting pages {data.PagesWaitDownloading} | Errors: {data.PagesWithError}]";
        }

        private static void Crawler_OnError(string message)
        {
            Console.WriteLine(message);
            logg.Error(message);
        }
    }
}
