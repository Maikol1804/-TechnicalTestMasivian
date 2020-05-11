using Microsoft.EntityFrameworkCore;
using RouletteWebApi.DataAccess;
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
        private readonly RouletteContext _context;

        public BetRepository(RouletteContext context)
        {
            _context = context;
        }

        public async Task<Bet> Add(Bet entity)
        {
            _context.Bets.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Bet> Delete(Bet entity)
        {
            _context.Bets.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Bet> DeleteById(long id)
        {
            Bet entity = await _context.Bets.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _context.Bets.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Bet> Update(Bet entity)
        {
            entity.UpdateDate = DateTime.Now;
            _context.Bets.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Bet>> GetAll()
        {
            return await _context.Bets.Include("Player").Include("Roulette").ToListAsync();
        }

        public async Task<Bet> GetById(long id)
        {
            return await _context.Bets.FindAsync(id);
        }

        public bool Exist(long id)
        {
            return _context.Bets.Any(e => e.Id == id);
        }
    }
}
