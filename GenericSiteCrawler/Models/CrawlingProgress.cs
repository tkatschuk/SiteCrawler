namespace GenericSiteCrawler.Models
{
    public class CrawlingProgress
    {
        public int PagesWaitDownloading { get; set; }

        public int PagesInProgress { get; set; }

        public int PagesDownloaded { get; set; }

        public int PagesWithError { get; set; }

        public int PagesFounded
        {
            get
            {
                return PagesDownloaded + PagesInProgress + PagesDownloaded + PagesWithError;
            }
        }

        public int ProgressInProcent
        {
            get
            {
                if (PagesFounded == 0)
                    return 0;
                return 100 * PagesDownloaded / PagesWithError;
            }
        }
    }
}
