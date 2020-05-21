using System.Collections.Generic;
using System.Linq;
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
    public class RouletteController : BaseController
    {
        
        public RouletteController(IComponentContext component) : base(component)
        {
        }

        #region API Methods

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Roulette>> PostRoulette([FromForm] Roulette roulette)
        {
            Response response = await rouletteServices.SaveRoulette(roulette);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetRoulette), new { id = roulette.Id }, MappersFactory.RouletteCreatedDTO().Map(roulette));
        }

        // GET: api/Roulette
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouletteDTO>>> GetRoulettes()
        {
            ResponseList<Roulette> responseRoulettes = await rouletteServices.GetAllRoulettes();
            if (responseRoulettes.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(responseRoulettes);
            }
            return MappersFactory.RouletteDTO().ListMap(responseRoulettes.List);
        }

        // GET: api/Roulette/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouletteDTO>> GetRoulette(long id)
        {
            Response response = await rouletteServices.ValidateRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            ResponseEntity<Roulette> responseRoulette = await rouletteServices.GetRouletteById(id);
            if (responseRoulette.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(responseRoulette);
            }

            return MappersFactory.RouletteDTO().Map(responseRoulette.Entity);
        }

        // PUT: api/roulette/1/open
        [HttpPut("{id:long}/open")]
        public async Task<ActionResult<Roulette>> PutOpenRoulette(long id)
        {
            Response response = await rouletteServices.ValidateOpenRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Response responseChange = await rouletteServices.ChangeRouletteState(id, true);
            if (responseChange.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(responseChange);
            }

            return Ok(new Response() { 
                Code = Enumerators.State.Ok.GetDescription(), 
                Message = "Roulette was opened" 
            });
        }

        // PUT: api/roulette/1/close
        [HttpPut("{id:long}/close")]
        public async Task<ActionResult<IEnumerable<BetDTO>>> PutCloseRoulette(long id)
        {
            Response response = await rouletteServices.ValidateCloseRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Response responseChange = await rouletteServices.ChangeRouletteState(id, false);
            if (responseChange.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(responseChange);
            }

            var responseBets = await betServices.GetAllBetsByRoulleteId(id);
            if (responseBets.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(responseBets);
            }

            return MappersFactory.BetDTO().ListMap(responseBets.List);
        }

        #endregion

    }
}
