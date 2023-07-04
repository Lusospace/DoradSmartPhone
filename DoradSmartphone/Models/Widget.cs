using SQLite;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DoradSmartphone.Models
{
    [Table("widget")]
    public class Widget : BaseEntity, INotifyPropertyChanged
    {
        private bool isSelected;
        public string Name { get; set; }
        public string FileName { get; set; }
        public int XPosition { get; internal set; }
        public int YPosition { get; internal set; }

        [ForeignKey(typeof(WidgetConfiguration))]
        public int WidgetConfigurationId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public WidgetConfiguration WidgetConfiguration { get; set; }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
