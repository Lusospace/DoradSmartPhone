using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("widget_configuration")]
    public class WidgetConfiguration : BaseEntity

    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Widget Widget { get; set; }
    }
}
