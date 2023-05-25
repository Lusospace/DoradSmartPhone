using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("widget")]
    public class Widget : BaseEntity
    {        
        public string Name { get; set; }
        public string FileName { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public WidgetConfiguration WidgetConfiguration { get; set; }
    }
}
