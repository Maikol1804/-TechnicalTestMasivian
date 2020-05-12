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

        public BetController(RouletteContext _context)
        {
            betRepository = new BetRepository(_context);
            rouletteRepository = new RouletteRepository(_context);
            playerRepository = new PlayerRepository(_context);
        }

        #region API Methods

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Bet>> PostBet(Bet bet)
        {
            Response response = await ValidatePostBet(bet);
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
            Response response = await ValidateGetBet(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Bet bet = await betRepository.GetById(id);
            return MappersFactory.BetDTO().Map(bet);
        }

        #endregion

        #region Validations

        public async Task<Response> ValidateGetBet(long id)
        {

            #region Validate Roulette
            var bet = await betRepository.GetById(id);
            if (bet == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet not found."
                };
            }
            #endregion

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        public async Task<Response> ValidatePostBet(Bet bet) 
        {
            #region Validate Roulette

            if (bet.Roulette == null || bet.Roulette.Id == 0)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette id is required."
                };
            }

            Roulette roullete = await rouletteRepository.GetById(bet.Roulette.Id);
            if (roullete == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The roulette id is invalid."
                };
            }

            if (!roullete.IsOpen)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The roulette is closed."
                };
            }

            #endregion

            #region Validate Bet Amount

            if (bet.Amount <= 0)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The bet amount is invalid"
                };
            }

            if (bet.Amount > 10000)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The maximum allowed bet is $10000."
                };
            }

            #endregion

            #region Validate Player

            if (bet.Player == null || bet.Player.Id == 0)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Player id is required."
                };
            }

            Player player = await playerRepository.GetById(bet.Player.Id);
            /*if (player.Money <= bet.Amount) {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The player doesn't have enough monetary funds."
                };
            }*/

            if (player != null) 
            {
                List<Bet> bets = await betRepository.GetAll();
                Bet currentBet = bets.FirstOrDefault(x => x.Roulette.Id == bet.Roulette.Id && x.Player.Id == bet.Player.Id);
                if (currentBet != null) 
                {
                    return new Response()
                    {
                        Code = Enumerators.State.Error.GetDescription(),
                        Message = "The player already has a bet on this roulette."
                    };
                }
            }

            #endregion

            #region Validate Bet Type

            if (bet.BetType == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type is required. There are two types, with number or with color"
                };
            }

            if (string.IsNullOrEmpty(bet.BetType.Code))
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type code is required"
                };
            }

            if (!bet.BetType.Code.Equals(Enumerators.BetTypes.Number.GetHashCode().ToString())
                && !bet.BetType.Code.Equals(Enumerators.BetTypes.Color.GetHashCode().ToString()))
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type code is invalid."
                };
            }

            if (string.IsNullOrEmpty(bet.BetType.Value))
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type value is required"
                };
            }

            if (bet.BetType.Code.Equals(Enumerators.BetTypes.Number.GetHashCode().ToString()))
            {
                var isNumericValue = int.TryParse(bet.BetType.Value, out int value);
                if (!isNumericValue || (value < 0 || value > 36))
                {
                    return new Response()
                    {
                        Code = Enumerators.State.Error.GetDescription(),
                        Message = "Bet type value is invalid. The value must be numeric, in the range of 0 to 36"
                    };
                }
            }

            if (bet.BetType.Code.Equals(Enumerators.BetTypes.Color.GetHashCode().ToString()))
            {
                string value = bet.BetType.Value.ToLower();

                if (!value.Equals(Enumerators.Colors.Black.GetDescription().ToLower())
                    && !value.Equals(Enumerators.Colors.Red.GetDescription().ToLower()))
                {
                    return new Response()
                    {
                        Code = Enumerators.State.Error.GetDescription(),
                        Message = "Bet type value is invalid. The value must be a string 'Black' or 'Red'"
                    };
                }
            }

            #endregion

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        #endregion

    }
}
