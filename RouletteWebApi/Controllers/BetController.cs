using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.DTO;
using RouletteWebApi.DTO.Mappers;
using RouletteWebApi.Models;
using RouletteWebApi.Transverse;
using RouletteWebApi.Transverse.Helpers;

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
            ResponseList<Bet> responseBets = await betServices.GetAllBets();
            if (responseBets.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(responseBets);
            }
            return MappersFactory.BetDTO().ListMap(responseBets.List);
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

            ResponseEntity<Bet> responseBet = await betServices.GetBetById(id);
            if (responseBet.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(responseBet);
            }

            return MappersFactory.BetDTO().Map(responseBet.Entity);
        }

        #endregion

    }
}
