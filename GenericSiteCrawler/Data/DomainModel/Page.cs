using System;

namespace GenericSiteCrawler.Data.DomainModel
{
    public class Page
    {
        public int Id { get; set; }

        public Website Website { get; set; }

        public string Url { get; set; }

        public bool Downloaded { get; set; }

        public bool Error { get; set; }

        public DateTime TimeAdded { get; set; } = DateTime.Now;
    }
}
