using GenericSiteCrawler;
using NLog;
using System;
using System.Threading.Tasks;

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
            crawler.Start(url).Wait();
            //Console.WriteLine("END");
            //Console.ReadKey();
        }

        private static void Crawler_OnError(string message)
        {
            Console.WriteLine(message);
            logg.Error(message);
        }
    }
}
