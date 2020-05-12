
namespace RouletteWebApi.Models
{
    public class Bet : BaseModel
    {
        public Player Player { get; set; }
        public Roulette Roulette { get; set; }
        public BetType BetType { get; set; }
        public long Amount { get; set; }
    }
}
