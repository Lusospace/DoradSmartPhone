using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("exercise")]
    public class Exercise : BaseEntity
    {        
        public DateTime Date { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Speed Speed { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public User User { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Route> Route { get; set; }
    }
}
