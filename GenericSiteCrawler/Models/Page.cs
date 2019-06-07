namespace GenericSiteCrawler.Models
{
    public class Page
    {
        public string Url { get; set; }

        public bool Downloaded { get; set; }

        public bool Error { get; set; }
    }
}
