using System;
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
    public class RouletteController : ControllerBase
    {
        private readonly IRoulette rouletteRepository;
        private readonly IBet betRepository;

        public RouletteController(RouletteContext _context)
        {
            rouletteRepository = new RouletteRepository(_context);
            betRepository = new BetRepository(_context);
        }

        #region API Methods

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Roulette>> PostRoulette([FromForm] Roulette roulette)
        {
            await rouletteRepository.Add(roulette);
            return CreatedAtAction(nameof(GetRoulette), new { id = roulette.Id }, MappersFactory.RouletteCreatedDTO().Map(roulette));
        }

        // GET: api/Roulette
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouletteDTO>>> GetRoulettes()
        {
            IEnumerable<Roulette> roulettes = await rouletteRepository.GetAll();
            return MappersFactory.RouletteDTO().ListMap(roulettes);
        }

        // GET: api/Roulette/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouletteDTO>> GetRoulette(long id)
        {
            Response response = await ValidateGetRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            var roulette = await rouletteRepository.GetById(id);

            return MappersFactory.RouletteDTO().Map(roulette);
        }

        // PUT: api/roulette/1/open
        [HttpPut("{id:long}/open")]
        public async Task<ActionResult<Roulette>> PutOpenRoulette(long id)
        {
            Response response = await ValidatePutOpenRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            var roulette = await rouletteRepository.GetById(id);
            roulette.IsOpen = true;

            await rouletteRepository.Update(roulette); 

            return Ok(new Response() { 
                Code = Enumerators.State.Ok.GetDescription(), 
                Message = "Roulette was opened" 
            });
        }

        // PUT: api/roulette/1/close
        [HttpPut("{id:long}/close")]
        public async Task<ActionResult<IEnumerable<BetDTO>>> PutCloseRoulette(long id)
        {
            Response response = await ValidatePutCloseRoulette(id);
            if (response.Code.Equals(Enumerators.State.Error.GetDescription()))
            {
                return BadRequest(response);
            }

            var roulette = await rouletteRepository.GetById(id);
            roulette.IsOpen = false;
            await rouletteRepository.Update(roulette);

            IEnumerable<Bet> bets = await betRepository.GetAll();
            bets = bets.Where(x => x.Roulette.Id == id).ToList();

            return MappersFactory.BetDTO().ListMap(bets);
        }

        #endregion

        #region Validations

        public async Task<Response> ValidateGetRoulette(long id) 
        {

            #region Validate Roulette
            var roulette = await rouletteRepository.GetById(id);
            if (roulette == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette not found."
                };
            }
            #endregion

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        public async Task<Response> ValidatePutOpenRoulette(long id) 
        {
            #region Validate Roulette

            var roulette = await rouletteRepository.GetById(id);

            if (roulette == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette not found."
                };
            }

            if (roulette.IsOpen)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette is currently open."
                };
            }

            if (roulette.UpdateDate != null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "This roulette game has already been played. It cannot be opened again."
                };
            }

            #endregion

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };
        }

        public async Task<Response> ValidatePutCloseRoulette(long id) 
        {
            var roulette = await rouletteRepository.GetById(id);

            if (roulette == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette not found."
                };
            }

            if (!roulette.IsOpen)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette is currently closed."
                };
            }

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        #endregion

    }
}
