using GenericSiteCrawler.Data.Repositories.Interfaces;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Data.Infrastructure;
using GenericSiteCrawler.Data.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Website>> GetAllWebsitesAsync()
        {
            return await _websiteRepository.GetAllAsync();
        }

        public void CreateWebsite(Website website)
        {
            _websiteRepository.Add(website);
        }

        public async Task SaveWebsiteAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        public void SaveWebsite()
        {
            _unitOfWork.Commit();
        }
    }
}
