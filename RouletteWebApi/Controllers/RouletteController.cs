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
    public class RouletteController : ControllerBase
    {
        public AdministrationServices administrationServices;

        public RouletteController(RouletteContext _context)
        {
            administrationServices = new AdministrationServices(_context);
        }

        #region API Methods

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Roulette>> PostRoulette([FromForm] Roulette roulette)
        {
            Response response = await administrationServices.SaveRoulette(roulette);
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
            IEnumerable<Roulette> roulettes = await administrationServices.GetAllRoulettes();
            return MappersFactory.RouletteDTO().ListMap(roulettes);
        }

        // GET: api/Roulette/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouletteDTO>> GetRoulette(long id)
        {
            Response response = await administrationServices.ValidateRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Roulette roulette = await administrationServices.GetRouletteById(id);
            return MappersFactory.RouletteDTO().Map(roulette);
        }

        // PUT: api/roulette/1/open
        [HttpPut("{id:long}/open")]
        public async Task<ActionResult<Roulette>> PutOpenRoulette(long id)
        {
            Response response = await administrationServices.ValidateOpenRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Roulette roulette = await administrationServices.GetRouletteById(id);
            roulette.IsOpen = true;

            response = await administrationServices.UpdateRoulette(roulette);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
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
            Response response = await administrationServices.ValidateCloseRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            Roulette roulette = await administrationServices.GetRouletteById(id);
            roulette.IsOpen = false;

            response = await administrationServices.UpdateRoulette(roulette);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            IEnumerable<Bet> bets = await administrationServices.GetAllBets();
            bets = bets.Where(x => x.Roulette.Id == id).ToList();

            return MappersFactory.BetDTO().ListMap(bets);
        }

        #endregion

    }
}
