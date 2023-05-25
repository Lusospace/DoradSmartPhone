using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("speed")]
    public class Speed : BaseEntity
    {        
        public float Avg { get; set; }
        public float Max { get; set; }        
        public DateTime DateTime { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Exercise Exercise { get; set; }
    }
}
