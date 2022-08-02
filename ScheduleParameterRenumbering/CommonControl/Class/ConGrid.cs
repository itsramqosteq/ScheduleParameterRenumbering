using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleParameterRenumbering
{
    public class ConduitGrid
    {
        public Element Conduit { get; set; }
        public Line ConduitLine { get; set; }
        public XYZ StartPoint { get; set; }
        public XYZ EndPoint { get; set; }
        public XYZ MidPoint { get; set; }
        public double Length { get; set; }
        public Element Grid { get; set; }
        public Line GridLine { get; set; }
        public double Distance { get; set; }
        public XYZ IntersectionPoint { get; set; }
        public ConduitGrid(Element a_Conduit,Element a_Grid,double a_Distance,XYZ a_IntersectionPoint)
        {
            Conduit = a_Conduit;
            Grid = a_Grid;
            Distance = a_Distance;
            IntersectionPoint = a_IntersectionPoint;
            ConduitLine = (Conduit.Location as LocationCurve).Curve as Line;
            StartPoint = ConduitLine.GetEndPoint(0);
            StartPoint = new XYZ(StartPoint.X, StartPoint.Y, 0);
            EndPoint = ConduitLine.GetEndPoint(1);
            EndPoint = new XYZ(EndPoint.X, EndPoint.Y, 0);
            MidPoint = (StartPoint + EndPoint) / 2;
            GridLine = (a_Grid.Location as LocationCurve).Curve as Line;
            Length = Conduit.LookupParameter("Length").AsDouble();
        }
    }
}
