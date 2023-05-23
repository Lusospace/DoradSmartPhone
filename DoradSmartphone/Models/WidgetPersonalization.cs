using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("widget_personalization")]
    public class WidgetPersonalization : BaseEntity
    {
        public string Name { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public User User { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<WidgetConfiguration> WidgetConfiguration { get; set; }
    }
}
