using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.Models;
using System.Collections.Generic;

namespace RouletteWebApi.DTO.Mappers
{
    public class BetMapper : Mapper<Bet, BetDTO>
    {
        public override BetDTO Map(Bet objectToMap)
        {
            return new BetDTO()
            {
                Id = objectToMap.Id,
                Player = MappersFactory.PlayerDTO().Map(objectToMap.Player),
                Roulette = MappersFactory.RouletteDTO().Map(objectToMap.Roulette),
                BetType = MappersFactory.BetTypeDTO().Map(objectToMap.BetType),
                Amount = objectToMap.Amount
            };
        }

        public override ActionResult<IEnumerable<BetDTO>> ListMap(IEnumerable<Bet> objectsToMap)
        {
            List<BetDTO> objectsList = new List<BetDTO>();

            foreach (Bet objectToMap in objectsToMap)
            {
                objectsList.Add(Map(objectToMap));
            }

            return objectsList;
        }
    }
}
