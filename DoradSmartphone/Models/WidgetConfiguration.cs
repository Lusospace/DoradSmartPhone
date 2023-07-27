using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("widget_configuration")]
    public class WidgetConfiguration : BaseEntity

    {
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Widget> Widget { get; set; }
    }
}
