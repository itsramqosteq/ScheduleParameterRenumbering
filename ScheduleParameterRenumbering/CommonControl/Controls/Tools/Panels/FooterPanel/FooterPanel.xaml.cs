using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScheduleParameterRenumbering
{
    /// <summary>
    /// Interaction logic for FooterPanel.xaml
    /// </summary>
    public partial class FooterPanelUserControl : UserControl
    {
        public FooterPanelUserControl()
        {
            InitializeComponent();
            TextVersion.Text = "V " +Utility.GetAssemblyFileVersion();
        }
    }
}
