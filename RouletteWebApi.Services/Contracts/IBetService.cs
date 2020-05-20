using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Contracts
{
    public interface IBetService :  IService
    {
        Task<Bet> GetBetById(long id);
        Task<IEnumerable<Bet>> GetAllBets();
        Task<Response> SaveBet(Bet bet);
        Task<Response> UpdateBet(Bet bet);
        Task<Response> ValidateBetToRead(long id);
        Task<Response> ValidateBetToSave(Bet bet);
    }
}
