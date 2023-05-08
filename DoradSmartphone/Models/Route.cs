using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("routes")]
    public class Route : BaseEntity
    {        
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Exercise Exercise { get; set; }
    }
}
