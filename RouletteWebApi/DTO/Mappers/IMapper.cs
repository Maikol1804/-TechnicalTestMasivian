using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO.Mappers
{
    public interface IMapper<O,D>
    {
        D Map(O objectToMap);
        ActionResult<IEnumerable<D>> ListMap(IEnumerable<O> objectsToMap);

    }
}
