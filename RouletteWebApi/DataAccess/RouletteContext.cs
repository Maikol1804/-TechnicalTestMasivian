using Microsoft.EntityFrameworkCore;
using RouletteWebApi.Models;

namespace RouletteWebApi.DataAccess
{
    public class RouletteContext : DbContext
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

    }
}
