using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.DataAccess;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.DTO;
using RouletteWebApi.DTO.Mappers;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Implementations;
using RouletteWebApi.Services.Interfaces;
using RouletteWebApi.Servicios;
using RouletteWebApi.Transverse;

namespace RouletteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBet betRepository;
        private readonly IRoulette rouletteRepository;
        private readonly IPlayer playerRepository;
        public AdministrationServices administrationServices;

        public BetController(RouletteContext _context)
        {
            administrationServices = new AdministrationServices(_context);
            betRepository = new BetRepository(_context);
            rouletteRepository = new RouletteRepository(_context);
            playerRepository = new PlayerRepository(_context);
        }

        #region API Methods

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Bet>> PostBet(Bet bet)
        {
            Response response = await administrationServices.ValidateBetToSave(bet);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription())) {
                return BadRequest(response);
            }

            Roulette roullete = await rouletteRepository.GetById(bet.Roulette.Id);
            if (roullete != null) 
                bet.Roulette = roullete;            

            Player player = await playerRepository.GetById(bet.Player.Id);
            if (player != null)
                bet.Player = player;

            await betRepository.Add(bet);
            return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, MappersFactory.BetDTO().Map(bet));
        }

        // GET: api/Bet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BetDTO>>> GetBets()
        {
            IEnumerable<Bet> bets = await betRepository.GetAll();
            return MappersFactory.BetDTO().ListMap(bets);
        }

        // GET: api/Bet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BetDTO>> GetBet(long id)
        {
            Response response = await administrationServices.ValidateBetToRead(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Bet bet = await betRepository.GetById(id);
            return MappersFactory.BetDTO().Map(bet);
        }

        #endregion

    }
}
