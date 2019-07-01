namespace GenericSiteCrawler.Models
{
    public class CrawlingProgress
    {
        public CrawlingProgress(
            int _pagesWaitDownloading,
            int _pagesInProgress,
            int _pagesDownloaded,
            int _pagesWithError)
        {
            PagesWaitDownloading = _pagesWaitDownloading;
            PagesInProgress = _pagesInProgress;
            PagesDownloaded = _pagesDownloaded;
            PagesWithError = _pagesWithError;
        }

        public int PagesWaitDownloading { get; set; }

        public int PagesInProgress { get; set; }

        public int PagesDownloaded { get; set; }

        public int PagesWithError { get; set; }

        public int PagesFounded
        {
            get
            {
                return PagesDownloaded + PagesInProgress + PagesWaitDownloading + PagesWithError;
            }
        }

        public int ProgressInProcent
        {
            get
            {
                if (PagesFounded == 0)
                    return 0;
                return 100 * (PagesDownloaded + PagesWithError) / PagesFounded;
            }
        }
    }
}
