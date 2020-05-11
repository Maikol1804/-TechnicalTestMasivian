using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.DataAccess;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.DTO;
using RouletteWebApi.DTO.Mappers;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Implementations;
using RouletteWebApi.Services.Interfaces;
using RouletteWebApi.Transverse;

namespace RouletteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBet repository;
        private readonly IRoulette rouletteRepository;

        public BetController(RouletteContext _context)
        {
            repository = new BetRepository(_context);
            rouletteRepository = new RouletteRepository(_context);
        }

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Bet>> PostBet(Bet bet)
        {
            if (bet.Roulette == null || bet.Roulette.Id == 0)
            {
                return BadRequest("Roulette id is required.");
            }

            Roulette roullete = await rouletteRepository.GetById(bet.Roulette.Id);
            if (roullete == null)
            {
                return BadRequest("The roulette id is invalid.");
            }

            if (!roullete.IsOpen) 
            {
                return BadRequest("The roulette is closed.");
            }

            if (bet.Amount <= 0) 
            {
                return BadRequest(new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The bet amount is invalid"
                });
            }

            if (bet.Amount > 10000)
            {
                return BadRequest("The maximum allowed bet is $10000.");
            }

            if (bet.Player == null || bet.Player.Id == 0) 
            {
                return BadRequest("Player id is required.");
            }

            //TODO Find Player
            /*var playerFound = playerRepository.Get(bet.Player.Id);
            if (playerFound.Money <= bet.Amount) {
                return BadRequest("The player doesn't have enough monetary funds.");
            }*/

            if (bet.BetType == null) {
                return BadRequest("Bet type is required. There are two types, with number or with color");
            }

            if (string.IsNullOrEmpty(bet.BetType.Code))
            {
                return BadRequest("Bet type code is required");
            }

            if (!bet.BetType.Code.Equals(Enumerators.BetTypes.Number.GetHashCode().ToString())
                || bet.BetType.Code.Equals(Enumerators.BetTypes.Color.GetHashCode().ToString()))
            {
                return BadRequest("Bet type code is invalid.");
            }

            if (string.IsNullOrEmpty(bet.BetType.Value))
            {
                return BadRequest("Bet type value is required");
            }

            if (bet.BetType.Code.Equals(Enumerators.BetTypes.Number.GetHashCode()))
            {
                var isNumericValue = int.TryParse(bet.BetType.Value, out int value);
                if (!isNumericValue || (value < 0 || value > 36)) 
                {
                    return BadRequest("Bet type value is invalid. The value must be numeric, in the range of 0 to 36");
                }
            }

            if (bet.BetType.Code.Equals(Enumerators.BetTypes.Color.GetHashCode())) 
            {
                string value = bet.BetType.Value.ToLower();

                if (!value.Equals(Enumerators.Colors.Black.GetDescription().ToLower())
                    || !value.Equals(Enumerators.Colors.Red.GetDescription().ToLower())) {
                    return BadRequest("Bet type value is invalid. The value must be a string 'Black' or 'Red'");
                }
            }

            bet.Roulette = roullete;
            await repository.Add(bet);
            return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);
        }

        // GET: api/Bet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bet>>> GetBets()
        {
            return await repository.GetAll();
        }

        // GET: api/Bet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bet>> GetBet(long id)
        {
            var bet = await repository.GetById(id);

            if (bet == null)
            {
                return NotFound();
            }

            return bet;
        }

    }
}
