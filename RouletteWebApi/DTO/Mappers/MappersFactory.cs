using RouletteWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO.Mappers
{
    public static class MappersFactory
    {
        public static IMapper<Roulette, RouletteCreatedDTO> RouletteCreatedDTO()
        {
            return new RouletteCreatedMapper();
        }

        public static IMapper<Player, PlayerDTO> PlayerDTO()
        {
            return new PlayerMapper();
        }

        public static IMapper<Roulette, RouletteDTO> RouletteDTO()
        {
            return new RouletteMapper();
        }

        public static IMapper<BetType, BetTypeDTO> BetTypeDTO()
        {
            return new BetTypeMapper();
        }

        public static IMapper<Bet, BetDTO> BetDTO()
        {
            return new BetMapper();
        }

    }
}
