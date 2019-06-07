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
            var crawler = new Crawler();
            crawler.Start("http://zahnarzt-broska.de");
            Console.ReadKey();
        }
    }
}
