using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoradSmartphone.Models
{
    [Table("user")]
    public class User : BaseEntity
    {        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public Exercise Exercise { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public WidgetConfiguration WidgetConfiguration { get; set; }
    }
}
