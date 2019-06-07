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
            string url = "";
            if (args.Length > 0)
                url = args[0];
            else
            {
                Console.WriteLine("Enter your site domain, e.g. reply.de");
                url = Console.ReadLine();
            }

            if (string.IsNullOrEmpty(url))
                url = "www.zahnarzt-broska.de";

            var crawler = new Crawler(url);
            crawler.OnError += Crawler_OnError;
            crawler.Start();
            
            Console.ReadKey();
        }

        private static void Crawler_OnError(string message)
        {
            Console.WriteLine(message);
            logg.Error(message);
        }
    }
}
