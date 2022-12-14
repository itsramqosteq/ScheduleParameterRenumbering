using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleParameterRenumbering
{
    public class ImportExcelVM : ViewModelBase
    {
        private DataTable _dataTable;
        public DataTable dataTable
        {
            
                get => _dataTable;
                set => SetProperty(ref _dataTable, value);
            
        }
        private bool _isCanceled;
        public bool isCanceled
        {

            get => _isCanceled;
            set => SetProperty(ref _isCanceled, value);

        }
        private bool _isColumnIndexChanged;
        public bool isColumnIndexChanged
        {

            get => _isColumnIndexChanged;
            set => SetProperty(ref _isColumnIndexChanged, value);

        }
    }
}
