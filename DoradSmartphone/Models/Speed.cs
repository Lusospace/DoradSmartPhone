using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("speed")]
    public class Speed : BaseEntity
    {
        public float Avg { get; set; }
        public float Max { get; set; }

        [ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }
    }
}
