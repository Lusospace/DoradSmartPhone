using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoradSmartphone.Models
{
    public class WidgetConfigurationWidget
    {
        [ForeignKey(typeof(Widget))]
        public int WidgetId { get; set; }
        [ForeignKey(typeof(WidgetConfiguration))]
        public int WidgetConfigurationId { get; set; }
    }
}
