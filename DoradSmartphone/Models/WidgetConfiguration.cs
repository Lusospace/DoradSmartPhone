using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("widget_configuration")]
    public class WidgetConfiguration : BaseEntity

    {
        public string Name { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public User User{ get; set; }
        [ManyToMany(typeof(WidgetConfigurationWidget))]
        public List<Widget> Widget { get; set; }
    }
}
