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
        public double XPosition { get; internal set; }
        public double YPosition { get; internal set; }
        public double ZPosition { get; internal set; }
        public double RelativeXPosition { get; set; }
        public double RelativeYPosition { get; set; }
        public double GlassXPosition { get; set; }
        public double GlassYPosition { get; set; }
        public bool IsInvisible { get; set; }

        [ForeignKey(typeof(WidgetConfiguration))]
        public int WidgetConfigurationId { get; set; }

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
