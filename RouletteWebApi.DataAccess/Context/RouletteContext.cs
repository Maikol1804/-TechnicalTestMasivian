using Microsoft.EntityFrameworkCore;
using RouletteWebApi.DataAccess.Context;
using RouletteWebApi.Models;
using System.Threading.Tasks;

namespace RouletteWebApi.DataAccess
{
    public class RouletteContext : DbContext, IContext
    {
        public RouletteContext(DbContextOptions<RouletteContext> options) : base(options)
        {
        }

        #region DbSet's Definitions

        public DbSet<Player> Players { get; set; }
        public DbSet<Roulette> Roulettes { get; set; }
        public DbSet<BetType> BetTypes { get; set; }
        public DbSet<Bet> Bets { get; set; }

        #endregion

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

    }
}
