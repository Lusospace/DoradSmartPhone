using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("speed")]
    public class Speed : BaseEntity
    {
        public double Avg { get; set; }
        public double Max { get; set; }

        [ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }
    }
}
