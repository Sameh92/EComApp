using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IGenericRepositoryForGuidId<T>:IGenereicRepository<T> where T : class
    {
        Task<T> GetAsync(Guid id);
        Task<T> GetById(Guid id,params Expression<Func<T, object>>[] includes);
        Task UpdateAsync(Guid id,T entity);
        Task DeleteAsync(Guid id);
    }
}
