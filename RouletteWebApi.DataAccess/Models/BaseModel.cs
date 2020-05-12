using System;

namespace RouletteWebApi.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            CreatedDate = DateTime.Now;
            Active = true;
            IsDeleted = false;
        }

        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

    }
}
