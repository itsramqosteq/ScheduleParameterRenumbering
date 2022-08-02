using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ScheduleParameterRenumbering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScheduleParameterRenumbering
{
    [Transaction(TransactionMode.Manual)]

    public class SampleHandler : IExternalEventHandler
    {
        DateTime startDate = DateTime.UtcNow;
        UIDocument _uiDoc = null;
        Document _doc = null;
        public void Execute(UIApplication uiApp)
        {

            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;

            try
            {
                // Get active view type
                Autodesk.Revit.DB.View activeView = _doc.ActiveView;

                ////////////////////////////
                // Count schedule entries //
                ////////////////////////////
                // check if active view is a schedule
                if (activeView is Autodesk.Revit.DB.ViewSchedule)
                {
                    ////////////////////////////////////
                    // Active/open view is a schedule //
                    ////////////////////////////////////
                    // Create new container for output string
                    StringBuilder sb = new StringBuilder();
                    ViewSchedule view = _doc.ActiveView as ViewSchedule;
                    //if (view==null) return;
                    TableData table = view.GetTableData();
                    TableSectionData section = table.GetSectionData(SectionType.Body);
                    int value = (bool)ParentUserControl.Instance.chkSkipLastRow.IsChecked ? section.NumberOfRows - 1 : section.NumberOfRows;
                    for (int i = 1; i < value; i++)
                    {
                        // Create list of element rows to loop though
                        List<ElementId> elems = new FilteredElementCollector(_doc, view.Id).ToElementIds().ToList();
                        // Create list of elements on row
                        List<Element> ElementsOnRow = new List<Element>();
                        List<ElementId> Remaining = null;
                        // Set static row (2) to analyze

                        int RowToAnalyse = i;

                        using (Transaction t = new Transaction(_doc, "dummy"))
                        {
                            t.Start();
                            // Remove row (2) to analyze
                            using (SubTransaction st = new SubTransaction(_doc))
                            {
                                st.Start();
                                section.RemoveRow(RowToAnalyse);
                                st.Commit();
                            }

                            // Iterate through set of elements
                            Remaining = new FilteredElementCollector(_doc, view.Id).ToElementIds().ToList();
                            t.RollBack();
                        }


                        // List each component in row (2)
                        int ctr = 1;
                        int startValue = 0;

                        // Get each element in schedule row (2) based on ID
                        foreach (ElementId id in elems)
                        {
                            if (Remaining.Contains(id)) continue;
                            ElementsOnRow.Add(_doc.GetElement(id));

                        }

                        // List quantity of elements on row (2)
                        sb.AppendLine(string.Format("{0} elements on row {1}", ElementsOnRow.Count, RowToAnalyse));
                        sb.AppendLine("");

                        using (Transaction t = new Transaction(_doc, "Renumbering"))
                        {
                            t.Start();
                            foreach (Element e in ElementsOnRow)
                            {

                                Parameter parameter = e.LookupParameter(ParentUserControl.Instance.txtRackIdMapping.Text);
                                parameter.Set(ParentUserControl.Instance.txtPrefix.Text + i.ToString());

                            }
                            t.Commit();
                        }
                    }

                }
                else
                {
                    // Active/open view is NOT a schedule
                    TaskDialog.Show("Error", "Make sure a schedule is opened before running this tool");
                }



            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Some error has occured. \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        public string GetName()
        {
            return "Revit Addin";
        }
    }
}


