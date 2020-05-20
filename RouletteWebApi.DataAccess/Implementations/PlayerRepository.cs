using Microsoft.EntityFrameworkCore;
using RouletteWebApi.DataAccess;
using RouletteWebApi.DataAccess.Context;
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

        protected IContext _context;
        protected DbSet<Player> _dbset;

        public PlayerRepository(IContext context)
        {
            _context = context;
            _dbset = _context.Set<Player>();
        }

        public async Task<Player> Add(Player entity)
        {
            _dbset.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Player> Delete(Player entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Player> DeleteById(long id)
        {
            Player entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _dbset.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Player> Update(Player entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbset.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Player>> GetAll()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<Player> GetById(long id)
        {
            return await _dbset.FindAsync(id);
        }

        public bool Exist(long id)
        {
            return _dbset.Any(e => e.Id == id);
        }
    }
}
