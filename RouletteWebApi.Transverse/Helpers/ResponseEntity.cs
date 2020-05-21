using RouletteWebApi.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RouletteWebApi.Transverse.Helpers
{
    public class ResponseEntity<T> : Response where T : class
    {
        public T Entity { get; set; }
    }
}
