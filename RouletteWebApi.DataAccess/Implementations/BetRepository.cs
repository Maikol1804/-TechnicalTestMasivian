using Microsoft.EntityFrameworkCore;
using RouletteWebApi.DataAccess.Context;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Implementations
{
    public class BetRepository : IBet
    {
        protected IContext _context;
        protected DbSet<Bet> _dbset;

        public BetRepository(IContext context)
        {
            _context = context;
            _dbset = _context.Set<Bet>();
        }

        public async Task<Bet> Add(Bet entity)
        {
            _dbset.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Bet> Delete(Bet entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Bet> DeleteById(long id)
        {
            Bet entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _dbset.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Bet> Update(Bet entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbset.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Bet>> GetAll()
        {
            return await _dbset.Include("Player").Include("Roulette").Include("BetType").ToListAsync();
        }

        public async Task<Bet> GetById(long id)
        {
            return await _dbset.FindAsync(id);
        }

        public bool Exist(long id)
        {
            return _dbset.Any(e => e.Id == id);
        }
    }
}
