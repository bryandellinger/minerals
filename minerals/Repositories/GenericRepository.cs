using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    /// <summary>
    /// taken from https://dotnettutorials.net/lesson/generic-repository-pattern-csharp-mvc/
    /// </summary>

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext context;
        private readonly DbSet<T> table = null;

        public GenericRepository(DataContext ctx)
        {
            context = ctx;
            table = context.Set<T>();
        }

        public void Delete(long id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
            context.SaveChanges();
        }

        public IEnumerable<T> GetAll() => table.ToList();
        public async Task<IEnumerable<T>> GetAllAsync() => await table.ToListAsync().ConfigureAwait(false);
        public T GetById(long id) => table.Find(id);
        public async Task<T> GetByIdAsync(long id) => await table.FindAsync(id);

        public T Insert(T entity)
        {
           table.Add(entity);
           context.SaveChanges();
           return entity;
        }

    }
}
