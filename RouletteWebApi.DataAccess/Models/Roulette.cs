
namespace RouletteWebApi.Models
{
    public class Roulette : BaseModel
    {
        public Roulette()
        {
            IsOpen = false;
        }

        public bool IsOpen { get; set; }

    }
}
