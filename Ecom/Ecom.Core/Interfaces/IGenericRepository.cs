using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        IEnumerable<T> GetAll();
        //with include
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        // get by Id with Include
        Task<T> GetByIdAsync(int id,params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(int id);
        Task<int> CountAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(int id,T entity);
        Task DeleteAsync(int id);
    }
}
