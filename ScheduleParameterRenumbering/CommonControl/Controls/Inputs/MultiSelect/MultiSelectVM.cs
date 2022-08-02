using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleParameterRenumbering
{
    public class MultiSelectVM : ViewModelBase
    {
        private List<MultiSelect> _multiSelects;
        public List<MultiSelect> MultiSelects
        {
            get => _multiSelects;
            set => SetProperty(ref _multiSelects, value);
        }
      
    }
}
