using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO.Mappers
{
    public class PlayerMapper : Mapper<Player, PlayerDTO>
    {
        public override PlayerDTO Map(Player objectToMap)
        {
            return new PlayerDTO
            {
                Id = objectToMap.Id
            };
        }

        public override ActionResult<IEnumerable<PlayerDTO>> ListMap(IEnumerable<Player> objectsToMap)
        {
            List<PlayerDTO> objectsList = new List<PlayerDTO>();

            foreach (Player objectToMap in objectsToMap)
            {
                objectsList.Add(Map(objectToMap));
            }

            return objectsList;
        }
    }
}
