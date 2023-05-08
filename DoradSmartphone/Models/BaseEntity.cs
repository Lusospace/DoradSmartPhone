using SQLite;

namespace DoradSmartphone.Models
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
