using RouletteWebApi.Models;
using RouletteWebApi.Transverse.Helpers;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Contracts
{
    public interface IPlayerService : IService
    {
        Task<ResponseEntity<Player>> GetPlayerById(long id);
    }
}
