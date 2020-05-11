using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.DTO.Mappers
{
    public abstract class Mapper<O, D> : IMapper<O, D>
    {
        public abstract D Map(O objectToMap);
        public abstract IEnumerable<D> Map(IEnumerable<O> objectsToMap);
    }
}
