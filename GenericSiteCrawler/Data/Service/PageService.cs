﻿using GenericSiteCrawler.Data.Repositories.Interfaces;
using GenericSiteCrawler.Data.Service.Interface;
using GenericSiteCrawler.Data.Infrastructure;
using GenericSiteCrawler.Data.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Page>> GetAllPagesAsync(int webSiteId)
        {
            return await _pageRepository.GetManyAsync(x => x.Website.Id == webSiteId);
        }

        public void CreatePage(Page page)
        {
            _pageRepository.Add(page);
        }

        public async Task SavePageAsync()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
