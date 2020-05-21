using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using RouletteWebApi.Transverse.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Contracts
{
    public interface IRouletteService : IService
    {
        Task<ResponseEntity<Roulette>> GetRouletteById(long id);
        Task<ResponseList<Roulette>> GetAllRoulettes();
        Task<Response> SaveRoulette(Roulette roulette);
        Task<Response> UpdateRoulette(Roulette roulette);
        Task<Response> ChangeRouletteState(long id, bool isOpen);
        Task<Response> ValidateRoulette(long id);
        Task<Response> ValidateOpenRoulette(long id);
        Task<Response> ValidateCloseRoulette(long id);

    }
}
