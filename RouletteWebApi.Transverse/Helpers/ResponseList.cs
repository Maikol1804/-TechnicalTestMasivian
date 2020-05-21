using RouletteWebApi.DTO;
using System.Collections.Generic;

namespace RouletteWebApi.Transverse.Helpers
{
    public class ResponseList<T> : Response where T : class
    {
        public IEnumerable<T> List { get; set; }
    }
}
