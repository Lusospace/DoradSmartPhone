using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("widget")]
    public class Widget : BaseEntity
    {        
        public string Name { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        [ManyToMany(typeof(WidgetConfigurationWidget))]
        public WidgetConfiguration WidgetConfiguration { get; set; }
    }
}
