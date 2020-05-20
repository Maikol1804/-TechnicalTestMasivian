using Microsoft.EntityFrameworkCore;
using RouletteWebApi.DataAccess.Context;
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
        protected IContext _context;
        protected DbSet<Roulette> _dbset;

        public RouletteRepository(IContext context)
        {
            _context = context;
            _dbset = _context.Set<Roulette>();
        }

        public async Task<Roulette> Add(Roulette entity)
        {
            _dbset.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Roulette> Delete(Roulette entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Roulette> DeleteById(long id)
        {
            Roulette entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _dbset.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Roulette> Update(Roulette entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbset.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Roulette>> GetAll()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<Roulette> GetById(long id)
        {
            return await _dbset.FindAsync(id);
        }

        public bool Exist(long id)
        {
            return _dbset.Any(e => e.Id == id);
        }
    }
}
