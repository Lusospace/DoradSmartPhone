using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("routes")]
    public class Route : BaseEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }
    }
}
