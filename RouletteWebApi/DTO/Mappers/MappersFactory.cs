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
    }
}
