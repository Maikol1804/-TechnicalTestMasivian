using RouletteWebApi.Models;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Contracts
{
    public interface IPlayerService : IService
    {
        Task<Player> GetPlayerById(long id);
    }
}
