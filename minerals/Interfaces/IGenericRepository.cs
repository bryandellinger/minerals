
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(long id);
        Task<T> GetByIdAsync(long id);
        T Insert(T entity);
        void Delete(long id);
    }
}
