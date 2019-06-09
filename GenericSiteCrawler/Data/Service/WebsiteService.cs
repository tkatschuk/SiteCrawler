using GenericSiteCrawler.Data.Repositories.Interfaces;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Data.Infrastructure;

namespace GenericSiteCrawler.Data.Service
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IWebsiteRepository _websiteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WebsiteService(
            IWebsiteRepository websiteRepository,
            IUnitOfWork unitOfWork)
        {
            _websiteRepository = websiteRepository;
            _unitOfWork = unitOfWork;
        }
    }
}
