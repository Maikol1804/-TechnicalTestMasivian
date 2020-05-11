using RouletteWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO.Mappers
{
    public class RouletteCreatedMapper : Mapper<Roulette, RouletteCreatedDTO>
    {
        public override RouletteCreatedDTO Map(Roulette objectToMap)
        {
            return new RouletteCreatedDTO
            {
                Id = objectToMap.Id
            };
        }

        public override IEnumerable<RouletteCreatedDTO> Map(IEnumerable<Roulette> objectsToMap)
        {
            throw new NotImplementedException();
        }
    }
}
