using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.Models;
using System.Collections.Generic;

namespace RouletteWebApi.DTO.Mappers
{
    public class BetTypeMapper : Mapper<BetType, BetTypeDTO>
    {
        public override BetTypeDTO Map(BetType objectToMap)
        {
            return new BetTypeDTO
            {
                Id = objectToMap.Id,
                Code = objectToMap.Code,
                Value = objectToMap.Value
            };
        }

        public override ActionResult<IEnumerable<BetTypeDTO>> ListMap(IEnumerable<BetType> objectsToMap)
        {
            List<BetTypeDTO> objectsList = new List<BetTypeDTO>();

            foreach (BetType objectToMap in objectsToMap)
            {
                objectsList.Add(Map(objectToMap));
            }

            return objectsList;
        }
    }
}
