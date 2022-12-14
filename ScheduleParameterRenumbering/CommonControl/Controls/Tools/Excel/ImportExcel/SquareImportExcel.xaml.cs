using ScheduleParameterRenumbering.Internal;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using Microsoft.Win32;
using System.IO;

namespace ScheduleParameterRenumbering
{
    /// <summary>
    /// Interaction logic for FooterPanel.xaml
    /// </summary>
    public partial class SquareImportExcelUserControl : UserControl
    {
        public event RoutedEventHandler Click;
        public delegate void RoutedEventHandler(object sender);

        public bool IsCanceled = false;

        public bool IsColumnIndexChanged=false;
        public bool IsSerialNumber
        {
            get { return (bool)GetValue(IsSerialNumberProperty); }
            set { SetValue(IsSerialNumberProperty, value); }
        }


        public static readonly DependencyProperty IsSerialNumberProperty =
            DependencyProperty.Register("IsSerialNumber", typeof(bool), typeof(SquareImportExcelUserControl), new PropertyMetadata(false));
        public bool IsColumnAsRow
        {
            get { return (bool)GetValue(IsColumnAsRowProperty); }
            set { SetValue(IsColumnAsRowProperty, value); }
        }


        public static readonly DependencyProperty IsColumnAsRowProperty =
            DependencyProperty.Register("IsColumnAsRow", typeof(bool), typeof(ImportExcelUserControl), new PropertyMetadata(false));
        public DataTable DataTableOutput
        {
            get { return (DataTable)GetValue(NameOutputProperty); }
            set { SetValue(NameOutputProperty, value); }
        }


        public static readonly DependencyProperty NameOutputProperty =
            DependencyProperty.Register("DataTableOutput", typeof(DataTable), typeof(SquareImportExcelUserControl), new PropertyMetadata(null));

        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }
        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(string.Empty));
        #region Width
        public new double Width
        {
            get { return (double)GetValue(widthProperty); }
            set { SetValue(widthProperty, value); }
        }
        public static readonly DependencyProperty widthProperty =
            DependencyProperty.Register("Width", typeof(double),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(0D, OnPropertyChangedForWidth));
        private static void OnPropertyChangedForWidth(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.Width = (double)e.NewValue;
        }
        #endregion
        #region Height
        public new double Height
        {
            get { return (double)GetValue(heightProperty); }
            set { SetValue(heightProperty, value); }
        }
        public static readonly DependencyProperty heightProperty =
            DependencyProperty.Register("Height", typeof(double),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(0D, OnPropertyChangedForHeight));
        private static void OnPropertyChangedForHeight(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.Height = (double)e.NewValue;
        }
        #endregion

        #region ToolTip
        #region toolTip
        public new string ToolTip
        {
            get { return (string)GetValue(toolTipProperty); }
            set { SetValue(toolTipProperty, value); }
        }
        public static readonly DependencyProperty toolTipProperty =
            DependencyProperty.Register("ToolTip", typeof(string),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(string.Empty, OnPropertyChangedtoolTip));
        private static void OnPropertyChangedtoolTip(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.ToolTip = (string)e.NewValue;

        }
        #endregion
        #region toolTipPlacement
        public PlacementMode ToolTipPlacement
        {
            get { return (PlacementMode)GetValue(placementProperty); }
            set { SetValue(placementProperty, value); }
        }
        public static readonly DependencyProperty placementProperty =
            DependencyProperty.Register("ToolTipPlacement", typeof(PlacementMode),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(PlacementMode.Left, OnPropertyChangedplacement));
        private static void OnPropertyChangedplacement(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.ToolTipPlacement = (PlacementMode)e.NewValue;
        }
        #endregion
        #region background
        public string ToolTipBackground
        {
            get { return (string)GetValue(TBProperty); }
            set { SetValue(TBProperty, value); }
        }
        public static readonly DependencyProperty TBProperty =
            DependencyProperty.Register("ToolTipBackground", typeof(string),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(string.Empty, OnPropertyChangedTB));
        private static void OnPropertyChangedTB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.ToolTipBackground = (string)e.NewValue;
        }
        #endregion
        #region ForeColor
        public string ToolTipforeColor
        {
            get { return (string)GetValue(tFProperty); }
            set { SetValue(tFProperty, value); }
        }
        public static readonly DependencyProperty tFProperty =
            DependencyProperty.Register("ToolTipforeColor", typeof(string),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(string.Empty, OnPropertyChangedTF));

        private static void OnPropertyChangedTF(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.ToolTipforeColor = (string)e.NewValue;

        }
        #endregion
        #region verticalOffset
        public double ToolTipVO
        {
            get { return (double)GetValue(VFProperty); }
            set { SetValue(VFProperty, value); }
        }
        public static readonly DependencyProperty VFProperty =
            DependencyProperty.Register("ToolTipVO", typeof(double),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(0D, OnPropertyChangedVf));

        private static void OnPropertyChangedVf(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.ToolTipVO = (double)e.NewValue;

        }
        #endregion
        #region horizontalOffset
        public double ToolTipHO
        {
            get { return (double)GetValue(HSProperty); }
            set { SetValue(HSProperty, value); }
        }
        public static readonly DependencyProperty HSProperty =
            DependencyProperty.Register("ToolTipHO", typeof(double),
              typeof(SquareImportExcelUserControl), new PropertyMetadata(0D, OnPropertyChangedHO));

        private static void OnPropertyChangedHO(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SquareImportExcelUserControl control = (SquareImportExcelUserControl)d;
            control.btnImport.ToolTipHO = (double)e.NewValue;

        }
        #endregion
        #endregion

        public SquareImportExcelUserControl()
        {
            InitializeComponent();
            Width = 100;
            Height = 24;
        }

        private async void BtnImport_Click(object sender)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.csv;"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    var view = new ImportExcelFieldsUserControl(openFileDialog.FileName,IsColumnAsRow)
                    {
                        IsSerialNumber = IsSerialNumber,
                        DataContext = new ImportExcelVM()
                    };


                    object identifier = Identifier;
                    //show the dialog
                    var result = await DialogHost.Show(view, identifier, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
                    IsCanceled = ((ImportExcelVM)view.DataContext).isCanceled;
                    IsColumnIndexChanged = ((ImportExcelVM)view.DataContext).isColumnIndexChanged;
                    DataTableOutput = ((ImportExcelVM)view.DataContext).dataTable;
                    Click?.Invoke(this);
                }
            }
        }
        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
          => Debug.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter &&
                parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new ProgressDialogUserControl());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }


    }
}
