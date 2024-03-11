using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IGenericRepositoryForIntegerId<T>:IGenereicRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<T> GetById(int id,params Expression<Func<T, object>>[] includes);
        Task UpdateAsync(int id,T entity);
        Task DeleteAsync(int id);
    }
}
