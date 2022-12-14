using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class MultiSelectUserControl : UserControl
    {
        public event EventHandler DropDownClosed;
        public delegate void EventHandler(object sender);
        private MultiSelect _removeItem = null;
        private bool isInternal = false;
        private bool isClosed = false;
        private double _maxLegthOfItem = 0;

        public Thickness NoDataFoundMargin
        {
            get { return (Thickness)GetValue(NoDataFoundMarginProperty); }
            set { SetValue(NoDataFoundMarginProperty, value); }
        }
        public static readonly DependencyProperty NoDataFoundMarginProperty =
            DependencyProperty.Register("NoDataFoundMargin", typeof(Thickness),
              typeof(MultiSelectUserControl), new PropertyMetadata(new Thickness(10, 40, 12, 0), OnPropertyChangedNoDataFoundMargin));
        private static void OnPropertyChangedNoDataFoundMargin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectUserControl control = (MultiSelectUserControl)d;
            control.txtNoDataFound.Margin = (Thickness)e.NewValue;
        }
        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register("IsRequired", typeof(bool),
              typeof(MultiSelectUserControl), new PropertyMetadata(false, OnPropertyChangedIsRequired));
        private static void OnPropertyChangedIsRequired(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectUserControl control = (MultiSelectUserControl)d;
            control.TxtIsRequired.Visibility = (bool)e.NewValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        public string AddSuffix
        {
            get { return (string)GetValue(AddSuffixProperty); }
            set { SetValue(AddSuffixProperty, value); }
        }
        public static readonly DependencyProperty AddSuffixProperty =
            DependencyProperty.Register("AddSuffix", typeof(string),
              typeof(MultiSelectUserControl), new PropertyMetadata(string.Empty));
        #region label 

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string),
              typeof(MultiSelectUserControl), new PropertyMetadata(string.Empty, OnPropertyChanged));
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectUserControl control = (MultiSelectUserControl)d;
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
              typeof(MultiSelectUserControl), new PropertyMetadata(0D, OnPropertyChangedForWidth));
        private static void OnPropertyChangedForWidth(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectUserControl control = (MultiSelectUserControl)d;
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
                        item.TextBlockMaxWidth = (double)e.NewValue * 2;
                        TextBlock textBlock = new TextBlock();
                        textBlock.Name = "sample";
                        Size size = Utility.MeasureString(item.Name, textBlock);
                        if ((control._maxLegthOfItem - 65) < size.Width)
                        {
                            control._maxLegthOfItem = size.Width + 65;
                        }
                    }
                    foreach (MultiSelect item in multiSelects)
                    {
                        if ((((double)e.NewValue * 2) - 65) > control._maxLegthOfItem &&  (double)e.NewValue < control._maxLegthOfItem)
                            item.ColumnWidth = control._maxLegthOfItem;
                        else if (control.Width > control._maxLegthOfItem)
                        {
                            item.ColumnWidth = (double)e.NewValue-35;
                        }
                        else
                        {
                            item.ColumnWidth = ((double)e.NewValue * 2) - 65;
                        }
                    }
                }
            }
            //if (control.ItemsSource.Count > 0)
            //{
            //    List<MultiSelect> multiSelects = control.cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList();
            //    if (multiSelects.Count > 0)
            //    {
            //        foreach (MultiSelect item in multiSelects)
            //        {

            //            item.TextBlockWidth = (double)e.NewValue;
            //            TextBlock textBlock = new TextBlock();
            //            textBlock.Name = "sample";
            //            Size size = Utility.MeasureString(item.Name, textBlock);
            //            if ((item.TextBlockWidth-30) < size.Width)
            //            {
            //                string displayValue = string.Empty;
            //                for (int i = 0; i < item.Name.Length; i++)
            //                {
            //                    string name = string.Join("", item.Name.Take(i + 1).ToList());
            //                    Size size1 = Utility.MeasureString(name, textBlock);
            //                    if (size1.Width >= (item.TextBlockWidth-30))
            //                    {
            //                        name = string.Join("", item.Name.Take(i - 6).ToList());
            //                        name += "... ";
            //                        displayValue = name;
            //                        break;
            //                    }
            //                }
            //                item.DisplayValue = displayValue;
            //            }
            //            else 
            //            {
            //                item.DisplayValue = item.Name;
            //            }
            //        }

            //        control.cmbMultiSelect.ItemsSource = multiSelects;
            //        control.cmbMultiSelect.UpdateLayout();
            //    }
            //}
        }
        public List<MultiSelect> SelectedItems
        {
            get { return (List<MultiSelect>)GetValue(SIProperty); }
            set { SetValue(SIProperty, value); }

        }


        public static readonly DependencyProperty SIProperty =
            DependencyProperty.Register("SelectedItems", typeof(List<MultiSelect>),
              typeof(MultiSelectUserControl), new PropertyMetadata(new List<MultiSelect>()));

        public List<MultiSelect> ItemsSource
        {
            get { return (List<MultiSelect>)GetValue(MSProperty); }
            set { SetValue(MSProperty, value); }
        }


        public static readonly DependencyProperty MSProperty =
            DependencyProperty.Register("ItemsSource", typeof(List<MultiSelect>),
              typeof(MultiSelectUserControl), new PropertyMetadata(new List<MultiSelect>(), OnPropertyChangedForMS));
        private static void OnPropertyChangedForMS(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectUserControl control = (MultiSelectUserControl)d;

            List<MultiSelect> multiSelects = e.NewValue as List<MultiSelect>;
            if (control.isClosed)
            {

                control.isClosed = false;
                return;
            }
            if (!control.isInternal)
            {
                control.cmbMultiSelect.ItemsSource = e.NewValue as List<MultiSelect>;
                control.SelectedItems = (e.NewValue as List<MultiSelect>).Where(x => x.IsChecked).ToList();

            }
            if (multiSelects.Count() > 0 && control.IsAllOptionEnable && multiSelects.Count(x => x.Name == "Select All") == 0)
            {
                MultiSelect multi = new MultiSelect
                {
                    Name = "Select All",
                    IsChecked = multiSelects.TrueForAll(x => x.IsChecked),
                    IsRemoveItem = false
                };

                multiSelects.Insert(0, multi);

            }


            //if (multiSelects.Count > 0)
            //{
            //    foreach (MultiSelect item in multiSelects)
            //    {

            //        item.IsRemoveItem = item.Name != "Select All" && control.IsAllowToAddItem;
            //        item.TextBlockWidth = control.Width - 30;
            //        item.DisplayValue = item.Name;
            //        TextBlock textBlock = new TextBlock();
            //        textBlock.Name = "sample";
            //        Size size = Utility.MeasureString(item.Name, textBlock);
            //        if (item.TextBlockWidth < size.Width)
            //        {
            //            string displayValue = string.Empty;
            //            for (int i = 0; i < item.Name.Length; i++)
            //            {
            //                string name = string.Join("", item.Name.Take(i + 1).ToList());
            //                Size size1 = Utility.MeasureString(name, textBlock);
            //                if (size1.Width >= item.TextBlockWidth)
            //                {
            //                    name = string.Join("", item.Name.Take(i - 6).ToList());
            //                    name += "... ";
            //                    displayValue = name;
            //                    item.DisplayValue = displayValue;
            //                    break;
            //                }
            //                else
            //                {
            //                    item.DisplayValue = item.Name;
            //                }
            //            }

            //        }
            //    }
            if (multiSelects.Count > 0)
            {
                control.txtNoDataFound.Visibility = Visibility.Collapsed;
                control._maxLegthOfItem = 0;
                foreach (MultiSelect item in multiSelects)
                {

                    item.IsRemoveItem = item.Name != "Select All" && control.IsAllowToAddItem;
                    item.TextBlockMinWidth = control.Width;
                    item.TextBlockMaxWidth = control.Width * 2;
                    TextBlock textBlock = new TextBlock();
                    textBlock.Name = "sample";
                    Size size = Utility.MeasureString(item.Name, textBlock);
                    if ((control._maxLegthOfItem - 65) < size.Width)
                    {
                        control._maxLegthOfItem = size.Width + 65;
                    }
                }
                foreach (MultiSelect item in multiSelects)
                {
                    if (((control.Width * 2) - 65) > control._maxLegthOfItem && control.Width < control._maxLegthOfItem)
                        item.ColumnWidth = control._maxLegthOfItem;
                    else if(control.Width > control._maxLegthOfItem)
                    {
                        item.ColumnWidth = control.Width-35 ;
                    }
                    else
                    {
                        item.ColumnWidth = (control.Width * 2) - 65;
                    }
                }
                if (!control.isInternal)
                {
                    multiSelects[0].DisplayText = "-- Selected (" + multiSelects.Count(x => x.IsChecked && x.Name != "Select All") + ") --";
                    control.cmbMultiSelect.ItemsSource = multiSelects;
                    control.cmbMultiSelect.SelectedIndex = control.IsAllOptionEnable ? 1 : 0;
                }
                else
                {
                    control.cmbMultiSelect.ItemsSource = multiSelects;
                }
            }
            else
            {
                control.txtNoDataFound.Visibility = Visibility.Visible;
            }
            if (control.IsAllowToAddItem)
            {
                control.btnAddItem.Visibility = Visibility.Visible;
                control.cmbMultiSelect.Padding = new Thickness(10, 0, 45, 0);
            }
            else
            {
                control.btnAddItem.Visibility = Visibility.Hidden;
                control.cmbMultiSelect.Padding = new Thickness(10, 0, 10, 0);
            }
            control.isInternal = false;

        }
        public bool IsAllOptionEnable
        {
            get { return (bool)GetValue(AllProperty); }
            set { SetValue(AllProperty, value); }
        }


        public static readonly DependencyProperty AllProperty =
            DependencyProperty.Register("IsAllOptionEnable", typeof(bool),
              typeof(MultiSelectUserControl), new PropertyMetadata(false, OnPropertyChangedForAll));
        private static void OnPropertyChangedForAll(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectUserControl control = (MultiSelectUserControl)d;
            if (control.cmbMultiSelect.ItemsSource != null)
            {
                List<MultiSelect> multiSelects = control.cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList();
                if (multiSelects.Count() > 0 && (bool)e.NewValue && multiSelects.Count(x => x.Name == "Select All") == 0)
                {
                    MultiSelect multi = new MultiSelect
                    {
                        Name = "Select All"
                    };
                    multiSelects.Insert(0, multi);
                    control._maxLegthOfItem =0;
                    foreach (MultiSelect item in multiSelects)
                    {
                        item.IsRemoveItem = item.Name != "Select All" && control.IsAllowToAddItem;
                        item.TextBlockMinWidth = control.Width;
                        item.TextBlockMaxWidth = control.Width * 2;
                        TextBlock textBlock = new TextBlock();
                        textBlock.Name = "sample";
                        Size size = Utility.MeasureString(item.Name, textBlock);
                        if ((control._maxLegthOfItem - 65) < size.Width)
                        {
                            control._maxLegthOfItem = size.Width + 65;
                        }
                    }
                    foreach (MultiSelect item in multiSelects)
                    {
                        if (((control.Width * 2) - 65) > control._maxLegthOfItem && control.Width < control._maxLegthOfItem)
                            item.ColumnWidth = control._maxLegthOfItem;
                        else if (control.Width > control._maxLegthOfItem)
                        {
                            item.ColumnWidth = control.Width-35;
                        }
                        else
                        {
                            item.ColumnWidth = (control.Width * 2) - 65;
                        }
                    }
                    control.cmbMultiSelect.ItemsSource = multiSelects;
                }
            }
        }
        public bool IsAllowToAddItem
        {
            get { return (bool)GetValue(AddItemsProperty); }
            set { SetValue(AddItemsProperty, value); }
        }

        public static readonly DependencyProperty AddItemsProperty =
          DependencyProperty.Register("IsAllowToAddItem", typeof(bool),
            typeof(MultiSelectUserControl), new PropertyMetadata(false, OnPropertyChangedForAddItems));
        private static void OnPropertyChangedForAddItems(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectUserControl control = (MultiSelectUserControl)d;
            if ((bool)e.NewValue)
            {
                control.btnAddItem.Visibility = Visibility.Visible;
                control.cmbMultiSelect.Padding = new Thickness(10, 0, 45, 0);
            }
            else
            {
                control.btnAddItem.Visibility = Visibility.Hidden;
                control.cmbMultiSelect.Padding = new Thickness(10, 0, 10, 0);
            }
        }

        #region regex
        public string Regex
        {
            get { return (string)GetValue(regexProperty); }
            set { SetValue(regexProperty, value); }
        }


        public static readonly DependencyProperty regexProperty =
            DependencyProperty.Register("Regex", typeof(string),
              typeof(MultiSelectUserControl), new PropertyMetadata(string.Empty));


        #endregion
        public MultiSelectUserControl()
        {
            InitializeComponent();
            labelStack.Visibility = Visibility.Collapsed;
            IsAllowToAddItem = false;
            cmbMultiSelect.Padding = new Thickness(10, 0, 15, 0);
            IsAllOptionEnable = false;
        }
        private void Chk_Checked(object sender, RoutedEventArgs e)
        {
            if (_removeItem != null)
            {
                return;
            }
            string content = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            List<MultiSelect> list = cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList();
            List<MultiSelect> temp = new List<MultiSelect>();
            if (this.IsAllOptionEnable && !ItemsSource.Any(x => x.Name == "Select All"))
            {
                MultiSelect multi = new MultiSelect
                {
                    Name = "Select All"
                };
                ItemsSource.Insert(0, multi);
            }
            foreach (MultiSelect item in ItemsSource)
            {
                if (content == "Select All")
                {
                    item.IsChecked = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)sender).IsChecked);
                }
                else if (item.Name == content)
                {
                    item.IsChecked = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)sender).IsChecked);
                }

                temp.Add(item);
            }
            if (temp.Any(x => !x.IsChecked && x.Name != "Select All") && this.IsAllOptionEnable)
            {
                temp[0].IsChecked = false;
            }
            if (this.IsAllOptionEnable && temp.Count(x => x.IsChecked && x.Name != "Select All") == (temp.Count() - 1))
            {

                temp[0].IsChecked = true;
            }
            isInternal = true;
            int count = temp.Count(x => x.IsChecked == true && x.Name != "Select All");
            int index = temp.FindIndex(x => x.Name == content);
            temp[index].DisplayText = "-- Selected (" + count + ") --";
            // cmbMultiSelect.Text = "-- Selected (" + count + ") --";
            ItemsSource.Clear();
            ItemsSource = temp;
            cmbMultiSelect.ItemsSource = ItemsSource;
            cmbMultiSelect.SelectedIndex = index;
            cmbMultiSelect.UpdateLayout();
            cmbMultiSelect.UpdateDefaultStyle();
            cmbMultiSelect.IsDropDownOpen = true;
        }
        private void CmbMultiSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_removeItem != null)
            {
                return;
            }
            MultiSelect obj = (((System.Windows.Controls.Primitives.Selector)sender).SelectedItem as MultiSelect);

            List<MultiSelect> list = cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList();
            if (obj != null && list.Count() > 0)
            {

                int count = list.Count(x => x.Name != "Select All" && x.IsChecked == true);
                foreach (MultiSelect item in list)
                {
                    if (obj.Name == "Select All")
                    {
                        item.IsChecked = obj.IsChecked;
                    }
                    else if (item.Name == obj.Name && IsAllOptionEnable && !item.IsChecked && list[0].Name == "Select All")
                    {
                        list[0].IsChecked = false;
                        break;
                    }
                }

                isInternal = true;
                count = list.Count(x => x.IsChecked == true && x.Name != "Select All");
                list[list.FindIndex(x => x.Name == obj.Name)].DisplayText = "-- Selected (" + count + ") --";
                ItemsSource.Clear();
                ItemsSource = list;
                cmbMultiSelect.ItemsSource = ItemsSource;
                cmbMultiSelect.UpdateLayout();
            }

        }
        private void CmbMultiSelect_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pattern = Regex;
            if (!string.IsNullOrEmpty(pattern) && !string.IsNullOrEmpty(cmbMultiSelect.Text))
            {
                Regex regex = new Regex(pattern);
                if (regex.IsMatch(cmbMultiSelect.Text))
                {
                    cmbMultiSelect.Text = string.Empty;
                    return;
                }
            }
            if (_removeItem != null)
            {
                _removeItem = null;
                return;
            }
            if (!cmbMultiSelect.Text.Contains("--"))
            {
                cmbMultiSelect.ItemsSource = ItemsSource.Where(x => x.Name.ToLower().Contains(cmbMultiSelect.Text.Trim().ToLower()));
                btnAddItem.Cursor = Cursors.Hand;
                if (cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList().Count == 0 && !string.IsNullOrEmpty(cmbMultiSelect.Text.Trim()))
                {
                    btnAddItem.Opacity = 1;
                    //btnAddItem.IsEnabled = true;
                    cmbMultiSelect.IsDropDownOpen = false;
                }

            }
            if (cmbMultiSelect.Text.Contains("--") || string.IsNullOrEmpty(cmbMultiSelect.Text))
            {

                //btnAddItem.Cursor = Cursors.No;
                //btnAddItem.IsEnabled = false;
                //btnAddItem.Opacity = 0.6;
                List<MultiSelect> list = cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList();
                if (list.Count > 0 && !list.Any(c => c.Name == "Select All") && IsAllOptionEnable)
                {

                    MultiSelect multi = new MultiSelect
                    {
                        Name = "Select All",
                        IsRemoveItem = false,

                        TextBlockMinWidth = this.Width,
                        TextBlockMaxWidth = this.Width * 2,
                        ColumnWidth = ((this.Width * 2) - 65) > this._maxLegthOfItem ? this._maxLegthOfItem : ((this.Width * 2) - 65),
                    };
                    list.Insert(0, multi);
                    cmbMultiSelect.ItemsSource = list;
                    ItemsSource.Insert(0, multi);
                }
            }
        }
        private void BtnAddItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!cmbMultiSelect.Text.Contains("--") && !ItemsSource.Any(x => x.Name.ToLower() == cmbMultiSelect.Text.ToLower())
                && !ItemsSource.Any(x => x.Name.ToLower() == cmbMultiSelect.Text.ToLower() + AddSuffix)
                )
            {
                
                TextBlock textBlock = new TextBlock();
                textBlock.Name = "sample";
                Size size = Utility.MeasureString(cmbMultiSelect.Text + AddSuffix, textBlock);
                if ((this._maxLegthOfItem - 65) < size.Width)
                {
                    this._maxLegthOfItem = size.Width + 65;
                }
                MultiSelect multi = new MultiSelect
                {
                    Name = cmbMultiSelect.Text + AddSuffix,
                    IsRemoveItem = IsAllowToAddItem,
                    DisplayText = "-- Selected (" + ItemsSource.Count(x => x.IsChecked && x.Name != "Select All") + ") --"
                };

                ItemsSource.Add(multi);
                foreach (MultiSelect item in ItemsSource)
                {
                    if (((this.Width * 2) - 65) > this._maxLegthOfItem)
                        item.ColumnWidth = this._maxLegthOfItem;
                    else
                    {
                        item.ColumnWidth = (this.Width * 2) - 65;
                    }
                }
                cmbMultiSelect.ItemsSource = ItemsSource;
                cmbMultiSelect.SelectedIndex = ItemsSource.Count - 1;
                cmbMultiSelect.IsDropDownOpen = true;
            }
        }
        private void CmbMultiSelect_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbMultiSelect.ItemsSource == null)
                return;

            if (!(string.IsNullOrEmpty(cmbMultiSelect.Text) || cmbMultiSelect.Text.Contains("--")))
            {
                cmbMultiSelect.ItemsSource = ItemsSource.Where(x => x.Name.ToLower().Contains(""));

            }
            List<MultiSelect> list = cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList().Where(x => x.Name != "Select All").ToList();
            isClosed = true;
            this.ItemsSource = list;
            SelectedItems = list.Where(x => x.IsChecked).ToList();

            if (DropDownClosed != null)
                DropDownClosed(this);
        }

        private void BtnAddItem_MouseEnter(object sender, MouseEventArgs e)
        {
            btnTooltip.IsOpen = true;
            btnTooltip.PlacementTarget = btnAddItem;
            btnTooltip.Placement = PlacementMode.Top;
            btnTooltip.Content = "Add Item";
            btnTooltip.HorizontalOffset = -40;
        }

        private void BtnAddItem_MouseLeave(object sender, MouseEventArgs e)
        {

            btnTooltip.IsOpen = false;
            btnTooltip.Visibility = System.Windows.Visibility.Collapsed;
        }



        private void BtnRemoveItems_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _removeItem = ((System.Windows.FrameworkElement)sender).Tag as MultiSelect;
            List<MultiSelect> multiSelects = cmbMultiSelect.ItemsSource.Cast<MultiSelect>().ToList();
            multiSelects.RemoveAll(x => x.Name == _removeItem.Name);
            multiSelects[multiSelects.Count - 1].DisplayText = "-- Selected (" + multiSelects.Count(x => x.IsChecked == true && x.Name != "Select All") + ") --";
            ItemsSource.RemoveAll(x => x.Name == _removeItem.Name);
            cmbMultiSelect.SelectedItem = null;
            cmbMultiSelect.ItemsSource = multiSelects;
            cmbMultiSelect.SelectedIndex = multiSelects.Count - 1;
            cmbMultiSelect.IsDropDownOpen = true;

        }
    }
}
