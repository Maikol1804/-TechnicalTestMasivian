using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Contracts
{
    public interface IRouletteService : IService
    {
        Task<Roulette> GetRouletteById(long id);
        Task<IEnumerable<Roulette>> GetAllRoulettes();
        Task<Response> SaveRoulette(Roulette roulette);
        Task<Response> UpdateRoulette(Roulette roulette);
        Task<Response> ValidateRoulette(long id);
        Task<Response> ValidateOpenRoulette(long id);
        Task<Response> ValidateCloseRoulette(long id);

    }
}
