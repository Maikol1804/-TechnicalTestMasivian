using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.DTO;
using RouletteWebApi.DTO.Mappers;
using RouletteWebApi.Models;
using RouletteWebApi.Transverse;

namespace RouletteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : BaseController
    {
        public BetController(IComponentContext component) : base(component)
        {
        }

        #region API Methods

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Bet>> PostBet(Bet bet)
        {
            Response response = await betServices.ValidateBetToSave(bet);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription())) {
                return BadRequest(response);
            }

            Roulette roullete = await rouletteServices.GetRouletteById(bet.Roulette.Id);
            if (roullete != null) 
                bet.Roulette = roullete;

            Player player = await playerServices.GetPlayerById(bet.Player.Id);
            if (player != null)
                bet.Player = player;

            response = await betServices.SaveBet(bet);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, MappersFactory.BetDTO().Map(bet));
        }

        // GET: api/Bet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BetDTO>>> GetBets()
        {
            IEnumerable<Bet> bets = await betServices.GetAllBets();
            return MappersFactory.BetDTO().ListMap(bets);
        }

        // GET: api/Bet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BetDTO>> GetBet(long id)
        {
            Response response = await betServices.ValidateBetToRead(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Bet bet = await betServices.GetBetById(id);
            return MappersFactory.BetDTO().Map(bet);
        }

        #endregion

    }
}
