using GenericSiteCrawler;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCrawler
{
    class Program
    {
        private static readonly Logger logg = LogManager.GetLogger(nameof(Program));

        static void Main(string[] args)
        {
            var crawler = new Crawler("zahnarzt-broska.de");
            crawler.OnError += Crawler_OnError;
            crawler.Start();
            
            Console.ReadKey();
        }

        private static void Crawler_OnError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
