using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Interfaces
{
    interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Delete(T entity);
        Task<T> DeleteById(long id);
        Task<T> Update(T entity);
        Task<T> GetById(long id);
        Task<List<T>> GetAll();
        bool Exist(long id);

    }
}
