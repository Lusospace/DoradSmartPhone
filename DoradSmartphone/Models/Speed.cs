using SQLite;

namespace DoradSmartphone.Models
{
    [Table("widget_configuration")]
    public class Speed : BaseEntity
    {        
        public float Avg { get; set; }
        public float Max { get; set; }
        public float Min { get; set; }
        public DateTime DateTime { get; set; }

    }
}
