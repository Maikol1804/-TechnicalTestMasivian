using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Contracts
{
    public interface IAdministrationServices : IService
    {
        Task<Roulette> GetRouletteById(long id);
        Task<IEnumerable<Roulette>> GetAllRoulettes();
        Task<Response> SaveRoulette(Roulette roulette);
        Task<Response> UpdateRoulette(Roulette roulette);


        Task<Bet> GetBetById(long id);
        Task<IEnumerable<Bet>> GetAllBets();
        Task<Response> SaveBet(Bet bet);
        Task<Response> UpdateBet(Bet bet);


        Task<Player> GetPlayerById(long id);


        Task<Response> ValidateBetToRead(long id);
        Task<Response> ValidateBetToSave(Bet bet);


        Task<Response> ValidateRoulette(long id);
        Task<Response> ValidateOpenRoulette(long id);

        Task<Response> ValidateCloseRoulette(long id);
    }
}
