using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteWebApi.DataAccess;
using RouletteWebApi.DTO;
using RouletteWebApi.DTO.Mappers;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Implementations;
using RouletteWebApi.Services.Interfaces;

namespace RouletteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IRoulette repository;

        public RouletteController(RouletteContext _context)
        {
            repository = new RouletteRepository(_context);
        }

        // POST: api/Roulette
        [HttpPost]
        public async Task<ActionResult<Roulette>> PostRoulette([FromForm] Roulette roulette)
        {
            await repository.Add(roulette);
            return CreatedAtAction(nameof(GetRoulette), new { id = roulette.Id }, MappersFactory.RouletteCreatedDTO().Map(roulette));
        }

        // GET: api/Roulette
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roulette>>> GetRoulettes()
        {
            return await repository.GetAll();
        }

        // GET: api/Roulette/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouletteCreatedDTO>> GetRoulette(long id)
        {
            var roulette = await repository.GetById(id);

            if (roulette == null)
            {
                return NotFound();
            }

            return MappersFactory.RouletteCreatedDTO().Map(roulette);
        }

        // PUT: api/roulette/1/open
        [HttpPut("{id:long}/open")]
        public async Task<ActionResult<Roulette>> PutOpenRoulette(long id)
        {
            var roulette = await repository.GetById(id);

            if (roulette == null)
            {
                return NotFound();
            }
            if (roulette.IsOpen) 
            {
                return BadRequest("Roulette is currently open");
            }

            roulette.IsOpen = true;
            roulette.UpdateDate = DateTime.Now;

            await repository.Update(roulette); 

            return Ok();
        }

        // PUT: api/roulette/1/close
        [HttpPut("{id:long}/close")]
        public async Task<ActionResult<Roulette>> PutCloseRoulette(long id)
        {
            var roulette = await repository.GetById(id);

            if (roulette == null)
            {
                return NotFound();
            }
            if (!roulette.IsOpen)
            {
                return BadRequest("Roulette is currently close");
            }

            roulette.IsOpen = false;
            roulette.UpdateDate = DateTime.Now;

            await repository.Update(roulette);

            return Ok();
        }

    }
}
