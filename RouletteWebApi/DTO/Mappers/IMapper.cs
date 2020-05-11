using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO.Mappers
{
    public interface IMapper<O,D>
    {
        D Map(O objectToMap);

        IEnumerable<D> Map(IEnumerable<O> objectsToMap);

    }
}
