using GenericSiteCrawler.Data.Repositories.Interfaces;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Data.Infrastructure;

namespace GenericSiteCrawler.Data.Service
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PageService(
            IPageRepository pageRepository,
            IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _unitOfWork = unitOfWork;
        }
    }
}
