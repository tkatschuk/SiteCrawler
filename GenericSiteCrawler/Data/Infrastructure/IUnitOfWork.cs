using System.Threading.Tasks;

namespace GenericSiteCrawler.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
    }
}
