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
    public class PlayerRepository : IPlayer
    {
        private readonly RouletteContext _context;

        public PlayerRepository(RouletteContext context)
        {
            _context = context;
        }

        public async Task<Player> Add(Player entity)
        {
            _context.Players.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Player> Delete(Player entity)
        {
            _context.Players.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Player> DeleteById(long id)
        {
            Player entity = await _context.Players.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _context.Players.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Player> Update(Player entity)
        {
            entity.UpdateDate = DateTime.Now;
            _context.Players.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Player>> GetAll()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<Player> GetById(long id)
        {
            return await _context.Players.FindAsync(id);
        }

        public bool Exist(long id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
