using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RouletteWebApi.DataAccess.Context
{
    public interface IContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
