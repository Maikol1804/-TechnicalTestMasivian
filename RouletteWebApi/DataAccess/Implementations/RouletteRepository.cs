using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RouletteWebApi.DataAccess;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Implementations
{
    public class RouletteRepository : IRoulette
    {
        
        private readonly RouletteContext _context;

        public RouletteRepository(RouletteContext context)
        {
            _context = context;
        }

        public async Task<Roulette> Add(Roulette entity)
        {
            _context.Roulettes.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Roulette> Delete(Roulette entity)
        {
            _context.Roulettes.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Roulette> DeleteById(long id)
        {
            var roulette = await _context.Roulettes.FindAsync(id);
            if (roulette == null)
            {
                return null;
            }

            _context.Roulettes.Remove(roulette);
            await _context.SaveChangesAsync();

            return roulette;
        }

        public async Task<Roulette> Update(Roulette entity)
        {
            _context.Roulettes.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Roulette>> GetAll()
        {
            return await _context.Roulettes.ToListAsync();
        }

        public async Task<Roulette> GetById(long id)
        {
            return await _context.Roulettes.FindAsync(id);
        }

        public bool Exist(long id)
        {
            return _context.Roulettes.Any(e => e.Id == id);
        }
    }
}
