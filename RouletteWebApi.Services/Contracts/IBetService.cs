using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using RouletteWebApi.Transverse.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Contracts
{
    public interface IBetService :  IService
    {
        Task<ResponseEntity<Bet>> GetBetById(long id);
        Task<ResponseList<Bet>> GetAllBets();
        Task<ResponseList<Bet>> GetAllBetsByRoulleteId(long id);
        Task<Response> SaveBet(Bet bet);
        Task<Response> UpdateBet(Bet bet);
        Task<Response> ValidateBetToRead(long id);
        Task<Response> ValidateBetToSave(Bet bet);
    }
}
