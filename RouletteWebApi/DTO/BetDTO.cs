using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO
{
    public class BetDTO : BaseDTO
    {
        public PlayerDTO Player { get; set; }
        public RouletteDTO Roulette { get; set; }
        public BetTypeDTO BetType { get; set; }
        public long Amount { get; set; }
    }
}
