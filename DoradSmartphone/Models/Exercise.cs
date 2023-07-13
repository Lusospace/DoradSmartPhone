using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("exercise")]
    public class Exercise : BaseEntity
    {
        public DateTime StartingDate { get; set; }
        public DateTime FinishingDate { get; set; }
        public string Time { get; set; }
        public string StartingAddress { get; set; }
        public string FinishingAddress { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Speed Speed { get; set; }

        [ForeignKey(typeof(User))]
        public int UserId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Route> Route { get; set; }
    }

}
