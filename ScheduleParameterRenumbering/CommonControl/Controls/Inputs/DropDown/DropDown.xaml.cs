using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class DropDownUserControl : UserControl
    {
        public event EventHandler DropDownClosed;
        public event SelectionChangedEventHandler SelectionChanged;
        public delegate void EventHandler(object sender);
        public delegate void SelectionChangedEventHandler(object sender);
     

        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register("IsRequired", typeof(bool),
              typeof(DropDownUserControl), new PropertyMetadata(false, OnPropertyChangedIsRequired));
        private static void OnPropertyChangedIsRequired(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropDownUserControl control = (DropDownUserControl)d;
            control.TxtIsRequired.Visibility = (bool)e.NewValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        #region label 

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string),
              typeof(DropDownUserControl), new PropertyMetadata(string.Empty, OnPropertyChanged));
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropDownUserControl control = (DropDownUserControl)d;
            control.lbl.Text = (string)e.NewValue;
            if (string.IsNullOrEmpty((string)e.NewValue))
            {
                control.labelStack.Visibility = Visibility.Collapsed;
                control.cmbMultiSelect.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                control.labelStack.Visibility = Visibility.Visible;
                control.cmbMultiSelect.Margin = new Thickness(0, 8, 0, 0);
            }
        }
        #endregion
        public new double Width
        {
            get { return (double)GetValue(widthProperty); }
            set { SetValue(widthProperty, value); }
        }


        public static readonly DependencyProperty widthProperty =
            DependencyProperty.Register("Width", typeof(double),
              typeof(DropDownUserControl), new PropertyMetadata(0D, OnPropertyChangedForWidth));
        private static void OnPropertyChangedForWidth(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropDownUserControl control = (DropDownUserControl)d;
            control.cmbMultiSelect.Width = (double)e.NewValue;
            control.grdContainer.Width = (double)e.NewValue;



            if (control.ItemsSource.Count > 0)
            {
                List<MultiSelect> multiSelects = control.cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList();
                if (multiSelects.Count > 0)
                {
                    foreach (MultiSelect item in control.cmbMultiSelect.ItemsSource)
                    {

                        item.TextBlockMinWidth = (double)e.NewValue;
                    }
                }
            }

        }
        public MultiSelect SelectedItem
        {
            get { return (MultiSelect)GetValue(SIProperty); }
            set { SetValue(SIProperty, value); }

        }


        public static readonly DependencyProperty SIProperty =
            DependencyProperty.Register("SelectedItem", typeof(MultiSelect),
              typeof(DropDownUserControl), new PropertyMetadata(null, OnPropertyChangedForMSS));
        private static void OnPropertyChangedForMSS(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropDownUserControl control = (DropDownUserControl)d;
            MultiSelect obj = e.NewValue as MultiSelect;
            if (obj != null && control.ItemsSource.Count > 0)
            {
                int index = control.ItemsSource.FindIndex(x => x.Name == obj.Name);
                control.cmbMultiSelect.SelectedIndex = index;
                control.lblPlaceHolder.Visibility = Visibility.Collapsed;

            }
            else
            {
                control.cmbMultiSelect.SelectedIndex = -1;
                control.lblPlaceHolder.Visibility = Visibility.Visible;
            }

        }

        public List<MultiSelect> ItemsSource
        {
            get { return (List<MultiSelect>)GetValue(MSProperty); }
            set { SetValue(MSProperty, value); }
        }


        public static readonly DependencyProperty MSProperty =
            DependencyProperty.Register("ItemsSource", typeof(List<MultiSelect>),
              typeof(DropDownUserControl), new PropertyMetadata(new List<MultiSelect>(), OnPropertyChangedForMS));
        private static void OnPropertyChangedForMS(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropDownUserControl control = (DropDownUserControl)d;

            control.cmbMultiSelect.SelectedItem = null;
            control.cmbMultiSelect.ItemsSource = e.NewValue as List<MultiSelect>;
            control.cmbMultiSelect.DisplayMemberPath = "Name";
        }

        public string HintText
        {
            get { return (string)GetValue(placeHolderProperty); }
            set { SetValue(placeHolderProperty, value); }
        }


        public static readonly DependencyProperty placeHolderProperty =
            DependencyProperty.Register("HintText", typeof(string),
              typeof(DropDownUserControl), new PropertyMetadata(string.Empty, OnPropertyChangedForplaceHolder));
        private static void OnPropertyChangedForplaceHolder(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropDownUserControl control = (DropDownUserControl)d;
            if (!string.IsNullOrEmpty((string)e.NewValue))
                control.lblPlaceHolder.Text = "-- " + (string)e.NewValue + " --";

        }
        public DropDownUserControl()
        {
            InitializeComponent();
            labelStack.Visibility = Visibility.Collapsed;
        }

        private void CmbMultiSelect_DropDownClosed(object sender, EventArgs e)
        {
            MultiSelect list = ((System.Windows.Controls.Primitives.Selector)sender).SelectedItem as MultiSelect;
            if (list != null)
            {
                lblPlaceHolder.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblPlaceHolder.Visibility = Visibility.Visible;
            }
            DropDownClosed?.Invoke(this);
        }

        private void cmbMultiSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MultiSelect list = ((System.Windows.Controls.Primitives.Selector)sender).SelectedItem as MultiSelect;
            if (list != null)
            {
                SelectedItem = list;
            }
            SelectionChanged?.Invoke(this);
        }

        private void cmbMultiSelect_DropDownOpened(object sender, EventArgs e)
        {
            lblPlaceHolder.Visibility = Visibility.Collapsed;
        }

        private void lblPlaceHolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cmbMultiSelect.IsDropDownOpen = true;
        }
    }
}
