using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO.Mappers
{
    public class RouletteMapper : Mapper<Roulette, RouletteDTO>
    {
        public override RouletteDTO Map(Roulette objectToMap)
        {
            return new RouletteDTO
            {
                Id = objectToMap.Id,
                IsOpen = objectToMap.IsOpen
            };
        }

        public override ActionResult<IEnumerable<RouletteDTO>> ListMap(IEnumerable<Roulette> objectsToMap)
        {
            List<RouletteDTO> objectsList = new List<RouletteDTO>();

            foreach (Roulette objectToMap in  objectsToMap) {
                objectsList.Add(Map(objectToMap));
            }

            return objectsList;
        }
    }
}
