using System;
using System.Collections.Generic;

namespace GenericSiteCrawler.Data.DomainModel
{
    public class Website
    {
        public int Id { get; set; }

        public string EnterUrl { get; set; }

        public DateTime TimeAdded { get; set; } = DateTime.Now;

        public virtual List<Page> Pages { get; set; }
    }
}
