using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GenericSiteCrawler.Services;

namespace GenericSiteCrawler
{
    public class Crawler
    {
        
        private List<string> Pages { get; set; }

        public void Start(string startUrl)
        {
            var pd = new PageDownloader(startUrl);
            pd.Download();
        }
    }
}
