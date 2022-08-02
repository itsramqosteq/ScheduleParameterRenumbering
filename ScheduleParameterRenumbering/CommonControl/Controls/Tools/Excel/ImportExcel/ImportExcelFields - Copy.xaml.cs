using Autodesk.Revit.UI;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ScheduleParameterRenumbering.Internal
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class ImportExcelFieldscpyUserControl : UserControl
    {
        private Microsoft.Office.Interop.Excel.Application _excelApp;
        private Microsoft.Office.Interop.Excel.Workbook _excelBook;
        private Microsoft.Office.Interop.Excel.Worksheet _workSheet;
        private Microsoft.Office.Interop.Excel.Range _range;
        private string _path = string.Empty;
        private DataTable _dataTable = new DataTable();
        int _totalNumberOfRow = 0;
        bool _isColumnAsRow = false;

        public bool IsSerialNumber
        {
            get { return (bool)GetValue(IsSerialNumberProperty); }
            set { SetValue(IsSerialNumberProperty, value); }
        }


        public static readonly DependencyProperty IsSerialNumberProperty =
            DependencyProperty.Register("IsSerialNumber", typeof(bool), typeof(ImportExcelFieldsUserControl), new PropertyMetadata(false));


        public DataTable ReadCsvFile(string filePath)
        {
            DataTable dtCsv = new DataTable();
            string Fulltext;

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < rowValues.Count(); j++)
                                    {
                                        dtCsv.Columns.Add(rowValues[j]); //add headers  
                                    }
                                }
                                else
                                {
                                    DataRow dr = dtCsv.NewRow();
                                    for (int k = 0; k < rowValues.Count(); k++)
                                    {
                                        dr[k] = rowValues[k].ToString();
                                    }
                                    dtCsv.Rows.Add(dr); //add other rows  
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return dtCsv;
        }

        public ImportExcelFieldscpyUserControl(string fileName, bool IsColumnAsRow)
        {
            InitializeComponent();
            List<string> sheetnames = new List<string>();
            _path = fileName;
            _isColumnAsRow = IsColumnAsRow;
            withColumnContainer.Visibility = IsColumnAsRow ? Visibility.Collapsed  :Visibility.Visible;
            withoutColumnContainer.Visibility = IsColumnAsRow ? Visibility.Visible  :Visibility.Collapsed;
            if (fileName.ToLower().Contains("csv"))
            {
                _dataTable = ReadCsvFile(_path);
                _totalNumberOfRow = _dataTable.Rows.Count;
                excelsheet.IsEnabled = false;
                txtHeaderIndex.Text = "1";

            }
            else
            {
                try
                {
                    _excelApp = new Microsoft.Office.Interop.Excel.Application();
                    _excelBook = _excelApp.Workbooks.Open(_path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                    try
                    {
                        foreach (Microsoft.Office.Interop.Excel.Worksheet worksheet in _excelBook.Worksheets)
                        {
                            sheetnames.Add(worksheet.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Some error has occured. \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        _excelBook.Close(null, null, null);
                        _excelApp.Quit();
                    }
                    excelsheet.ItemsSource = sheetnames;
                    excelsheet.SelectedIndex = 0;
                    if (sheetnames.Count > 0)
                    {
                        txtHeaderIndex.Text = "1";
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Some error has occured. \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }



        private void TxtHeaderIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_path.ToLower().Contains("csv") && !string.IsNullOrEmpty(txtHeaderIndex.Text) && Convert.ToInt32(txtHeaderIndex.Text) > 0)
            {
                try
                {
                    int headerIndex = Convert.ToInt32(txtHeaderIndex.Text);
                    string concatString = string.Empty;
                    if (headerIndex == 1)
                    {
                        List<string> stringList = _dataTable.Columns.Cast<DataColumn>()
                                         .Select(x => x.ColumnName)
                                         .ToList();

                        foreach (string str in stringList)
                        {
                            concatString += str + "\r\n";
                        }

                    }
                    else if (headerIndex > 1 && _dataTable.Rows.Count > 0)
                    {
                        foreach (var str in _dataTable.Rows[headerIndex - 2].ItemArray)
                        {
                            concatString += Convert.ToString(str) + "\r\n";
                        }

                    }
                    txtHeaderIndex.ToolTip = concatString;
                    txtRowStart.Text = _totalNumberOfRow == 0 ? "1" : Convert.ToString(Convert.ToInt32(txtHeaderIndex.Text.Trim()) + 1);
                    txtRowEnd.Text = _totalNumberOfRow == 0 ? "1" : (_dataTable.Rows.Count + 1).ToString();
                }
                catch (Exception exception)
                {
                    System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }


            }
            else
            {
                if (!string.IsNullOrEmpty(txtHeaderIndex.Text) && Convert.ToInt32(txtHeaderIndex.Text) > 0)
                {
                    try
                    {
                        try
                        {

                            if (excelsheet.SelectedItem != null)
                            {
                                _excelApp = new Microsoft.Office.Interop.Excel.Application();
                                _excelBook = _excelApp.Workbooks.Open(_path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                                _workSheet = _excelBook.Worksheets.Item[excelsheet.SelectedIndex + 1];
                                Microsoft.Office.Interop.Excel.Range excelRange = _workSheet.UsedRange;

                                _range = _workSheet.UsedRange;
                                int colCnt = _range.Columns.Count;
                                int rowCount = _range.Rows.Count;

                                txtRowEnd.Text = rowCount.ToString();
                                _totalNumberOfRow = rowCount;
                                _dataTable = new DataTable();
                                string concatString = string.Empty;
                                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                                {
                                    string strColumn = "";
                                    strColumn = Convert.ToString((excelRange.Cells[Convert.ToInt32(txtHeaderIndex.Text), colCnt] as Microsoft.Office.Interop.Excel.Range).Value2);
                                    if (strColumn != null)
                                    {
                                        concatString += strColumn + "\r\n";
                                        _dataTable.Columns.Add(strColumn, typeof(string));
                                    }
                                }
                                txtHeaderIndex.ToolTip = concatString;
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                        finally
                        {
                            _excelBook.Close(null, null, null);
                            _excelApp.Quit();
                        }
                        txtRowStart.Text = _totalNumberOfRow == 1 ? "1" : Convert.ToString(Convert.ToInt32(txtHeaderIndex.Text.Trim()) + 1);
                    }
                    catch (Exception exception)
                    {
                        System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }
            }
        }




        private void PreviewTextInputHeaderIndex(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }



        private void BtnUpload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (_path.ToLower().Contains("csv") && !string.IsNullOrEmpty(txtHeaderIndex.Text) && Convert.ToInt32(txtHeaderIndex.Text) > 0)

                {
                    try
                    {
                        int headerIndex = Convert.ToInt32(txtHeaderIndex.Text);
                        int rowStart = Convert.ToInt32(txtRowStart.Text);
                        if (headerIndex + 1 == rowStart)
                        {
                            rowStart = headerIndex - 1;
                        }
                        else
                        {
                            rowStart = Convert.ToInt32(txtRowStart.Text) - 2;
                        }

                        if (_dataTable.Rows.Count > 0)
                        {
                            int rowEnd = Convert.ToInt32(txtRowEnd.Text);

                            rowEnd = (rowEnd - rowStart) - 1;

                            DataTable dt = _dataTable.AsEnumerable()
                                             .Skip(rowStart)
                                             .Take(rowEnd)
                                               .Cast<DataRow>()
                                            .Where(row => !row.ItemArray.All(field => field is DBNull ||
                                                                      string.IsNullOrWhiteSpace(field as string)))
                                     .CopyToDataTable();
                            ImportExcelVM importExcelViewModel = new ImportExcelVM
                            {
                                isCanceled = false,
                                dataTable = dt

                            };
                            this.DataContext = importExcelViewModel;
                        }
                        else
                        {
                            ImportExcelVM importExcelViewModel = new ImportExcelVM
                            {
                                isCanceled = false,
                                dataTable = _dataTable

                            };
                            this.DataContext = importExcelViewModel;
                        }

                    }
                    catch (Exception exception)
                    {

                        System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }



                }
                else
                {
                    try
                    {
                        if (excelsheet.SelectedItem != null && !string.IsNullOrEmpty(txtHeaderIndex.Text) && Convert.ToInt32(txtHeaderIndex.Text) > 0)
                        {
                            _excelApp = new Microsoft.Office.Interop.Excel.Application();
                            _excelBook = _excelApp.Workbooks.Open(_path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                            _workSheet = _excelBook.Worksheets.Item[excelsheet.SelectedIndex + 1];
                            Microsoft.Office.Interop.Excel.Range excelRange = _workSheet.UsedRange;

                            _range = _workSheet.UsedRange;
                            int colCnt = _range.Columns.Count;
                            int rowCnt = _range.Rows.Count;
                            int rowStart = Convert.ToInt32(txtRowStart.Text);
                            int rowEnd = Convert.ToInt32(txtRowEnd.Text);
                            string strCellData = "";
                            //rowEnd = excelRange.Rows.Count > rowEnd ? (rowEnd - 1) : excelRange.Rows.Count;
                            if (_totalNumberOfRow > 1)
                                for (rowCnt = rowStart; rowCnt <= rowEnd; rowCnt++)
                                {
                                    string strData = "";
                                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                                    {
                                        var value = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                                        strCellData = value != null ? Convert.ToString(value) : "";
                                        strData += strCellData + "♡";
                                    }
                                    strData = strData.Remove(strData.Length - 1, 1);
                                    _dataTable.Rows.Add(strData.Split('♡'));

                                }

                            _excelBook.Close(true, null, null);
                            _excelApp.Quit();
                            _dataTable = _dataTable.Rows
                                 .Cast<DataRow>()
                                 .Where(row => !row.ItemArray.All(field => field is DBNull ||
                                                                  string.IsNullOrWhiteSpace(field as string)))
                                 .CopyToDataTable();
                            _dataTable.AcceptChanges();
                            if (IsSerialNumber)
                                AddAutoIncrementColumn(_dataTable);
                            ImportExcelVM importExcelViewModel = new ImportExcelVM
                            {
                                isCanceled = false,
                                dataTable = _dataTable
                            };
                            this.DataContext = importExcelViewModel;
                        }
                    }
                    catch (Exception exception)
                    {
                        if (exception.Message.Contains("The source contains no DataRows"))
                        {


                            ImportExcelVM importExcelViewModel = new ImportExcelVM
                            {
                                isCanceled = false,
                                dataTable = _dataTable
                            };
                            this.DataContext = importExcelViewModel;
                        }
                        else
                            System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }
            }
            catch (Exception exception)
            {

                System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
        public void AddAutoIncrementColumn(DataTable dt)
        {
            string columnName = "Sno";
            if (!dt.Columns.Contains(columnName))
            {
                DataColumn column = new DataColumn
                {
                    ColumnName = columnName,
                    DataType = System.Type.GetType("System.Int32")
                };
                dt.Columns.Add(column);
            }
            int index = 0;

            foreach (DataRow row in dt.Rows)
            {
                row.SetField(dt.Columns[columnName], ++index);
            }
            dt.Columns[columnName].SetOrdinal(0);
        }





        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((System.Windows.FrameworkElement)sender).Tag == null)
            {
                btnCancelWithout.Background = btnCancel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D85922");
                btnCancelWithout.Background = btnCancel.BorderBrush = btnCancel.Background;
                cancelIconWithout.Foreground =  cancelIcon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFF");
                cancelTextWithout.Foreground = cancelText.Foreground = cancelIcon.Foreground;
            }
            else
            {
              btnImportWithout.Opacity=  btnUpload.Opacity = 1;
            }
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((System.Windows.FrameworkElement)sender).Tag == null)
            {
                btnCancelWithout.Background = btnCancel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFF");
                btnCancelWithout.Background = btnCancel.BorderBrush = btnCancel.Background;
                cancelIconWithout.Foreground = cancelIcon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
                cancelTextWithout.Foreground = cancelText.Foreground = cancelIcon.Foreground;

            }
            else
            {
                btnImportWithout.Opacity = btnUpload.Opacity = 0.6;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ImportExcelVM importExcelViewModel = new ImportExcelVM
            {
                isCanceled = true
            };
            this.DataContext = importExcelViewModel;

        }

        private void txtRowStart_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRowStart.Text) && Convert.ToInt32(txtRowStart.Text) <= Convert.ToInt32(txtHeaderIndex.Text))
            {

                if (_path.ToLower().Contains("csv"))
                    txtRowStart.Text = _totalNumberOfRow == 0 ? "1" : Convert.ToString(Convert.ToInt32(txtHeaderIndex.Text.Trim()) + 1);
                else
                    txtRowStart.Text = _totalNumberOfRow == 1 ? "1" : Convert.ToString(Convert.ToInt32(txtHeaderIndex.Text.Trim()) + 1);
            }


        }

        private void txtRowEnd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRowEnd.Text) && Convert.ToInt32(txtRowEnd.Text) <= Convert.ToInt32(txtRowStart.Text))
            {
                if (_path.ToLower().Contains("csv"))
                    txtRowEnd.Text = _totalNumberOfRow == 0 ? "1" : Convert.ToString(_totalNumberOfRow + 1);
                else
                    txtRowEnd.Text = _totalNumberOfRow == 1 ? "1" : Convert.ToString(_totalNumberOfRow);

            }

        }

        private void txtValue_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void TxtRow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRowStart.Text) && !string.IsNullOrEmpty(txtRowEnd.Text) && Convert.ToInt32(txtRowStart.Text) >= Convert.ToInt32(txtRowEnd.Text))
            {
                if (_path.ToLower().Contains("csv"))
                    txtRowStart.Text = _totalNumberOfRow == 0 ? "1" : Convert.ToString(Convert.ToInt32(txtHeaderIndex.Text.Trim()) + 1);
                else
                    txtRowStart.Text = _totalNumberOfRow == 1 ? "1" : Convert.ToString(Convert.ToInt32(txtHeaderIndex.Text.Trim()) + 1);
            }
            int val = 0;
            if (_path.ToLower().Contains("csv"))
                val = _totalNumberOfRow == 0 ? 1 : _totalNumberOfRow + 1;
            else
                val = _totalNumberOfRow == 1 ? 1 : _totalNumberOfRow;
            if (!string.IsNullOrEmpty(txtRowEnd.Text) && Convert.ToInt32(txtRowEnd.Text) > val)
            {
                if (_path.ToLower().Contains("csv"))
                    txtRowEnd.Text = _totalNumberOfRow == 0 ? "1" : Convert.ToString(_totalNumberOfRow + 1);
                else
                    txtRowEnd.Text = _totalNumberOfRow == 1 ? "1" : Convert.ToString(_totalNumberOfRow);
            }

            if (!string.IsNullOrEmpty(txtRowStart.Text) && !string.IsNullOrEmpty(txtRowEnd.Text) && Convert.ToInt32(txtRowStart.Text) <= Convert.ToInt32(txtRowEnd.Text))
            {
                btnUpload.IsEnabled = true;
                btnUpload.Cursor = Cursors.Arrow;
            }
            else
            {
                btnUpload.Cursor = Cursors.No;
                btnUpload.IsEnabled = false;
            }
        }

        private void TxtRowWithout_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnImportWithout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtRowStartWithout_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtRowEndWithout_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
