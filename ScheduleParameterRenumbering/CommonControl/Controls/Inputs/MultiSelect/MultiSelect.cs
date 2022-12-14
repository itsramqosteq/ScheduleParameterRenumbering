using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleParameterRenumbering
{
  public  class MultiSelect: BaseClass
    {
      
        private string _displayText = "-- selected(0)--";
     
        public object Item { get; set; }
        public bool IsChecked { get; set; }
        public string DisplayText {
            get
            {
                return _displayText ;
            }
            set
            {
                _displayText = value;
            }
        }
        //public string DisplayValue { get; set; }
        public bool IsShowCheckBox
        {
            get
            {
                return !IsAllowToAddItem;
            }
            
        }
        public double ItemHeight
        {
            get
            {
                return IsAllowToAddItem ? 40:30;
            }

        }
        public double TextBlockMinWidth { get; set; }
        public double TextBlockMaxWidth { get; set; }
        public double ColumnWidth { get; set; }

        public bool IsAllowToSearchItem { get; set; }
        public bool IsAllowToAddItem { get; set; }

        public bool IsRemoveItem { get; set; }
    }
    
}
