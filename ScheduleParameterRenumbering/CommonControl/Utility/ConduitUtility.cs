using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleParameterRenumbering
{
    public partial class Utility
    {
        public static double GetConduitLength(Element e)
        {
            return e.LookupParameter("Length").AsDouble();
        }
        public static Line GetLineFromConduit(Conduit conduit, bool setZvalueZero = false)
        {
            if (!setZvalueZero)
                return (conduit.Location as LocationCurve).Curve as Line;
            else
            {
                Line line = (conduit.Location as LocationCurve).Curve as Line;
                return Line.CreateBound(GetXYvalue(line.GetEndPoint(0)), GetXYvalue(line.GetEndPoint(1)));
            }
        }
        public static Conduit GetMinLengthConduit(List<Conduit> conduits)
        {
            return conduits.OrderBy(x => x.LookupParameter("Length").AsDouble()).FirstOrDefault();
        }
        public static Conduit GetMaxLengthConduit(List<Conduit> conduits)
        {
            return conduits.OrderByDescending(x => x.LookupParameter("Length").AsDouble()).FirstOrDefault();
        }
        public static void GetStartEndPointForConduit(Conduit conduit, ref XYZ startPoint, ref XYZ endPoint)
        {
            Line conduitLine = (conduit.Location as LocationCurve).Curve as Line;
            startPoint = conduitLine.GetEndPoint(0);
            endPoint = conduitLine.GetEndPoint(1);
        }
        public static Conduit CreateConduit(Document doc, Conduit refConduit, XYZ startPoint, XYZ endPoint)
        {
            ElementId condtypeId = refConduit.GetTypeId();
            ElementId referenceLevel = refConduit.LookupParameter("Reference Level").AsElementId();
            double diameter = refConduit.LookupParameter("Diameter(Trade Size)").AsDouble();
            Conduit newConduit = Conduit.Create(doc, condtypeId, startPoint, endPoint, referenceLevel);
            Parameter diap = newConduit.LookupParameter("Diameter(Trade Size)");
            diap.Set(diameter);
            return newConduit;
        }
        public static Conduit FindOuterConduit(List<Element> conduitlist)
        {
            Conduit outerconduit = null;
            List<double> distancecollection = new List<double>();
            double max_distance;
            XYZ point1 = (conduitlist[0].Location as LocationCurve).Curve.GetEndPoint(0);
            XYZ point2 = (conduitlist[0].Location as LocationCurve).Curve.GetEndPoint(1);
            if (Math.Round(point1.X, 4) == Math.Round(point2.X, 4) && Math.Round(point1.Y, 4) == Math.Round(point2.Y, 4))
            {
                foreach (Conduit cond in conduitlist)
                {
                    if (conduitlist[0].Id != cond.Id)
                    {
                        //findintersection
                        XYZ con1_firstconduitpoint = (conduitlist[0].Location as LocationCurve).Curve.GetEndPoint(0);
                        XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                        double distance = Math.Pow(con1_firstconduitpoint.X - con2_firstconduitpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - con2_firstconduitpoint.Y, 2);
                        distance = Math.Sqrt(distance);
                        distancecollection.Add(distance);

                    }
                }
                max_distance = distancecollection.Max<double>();
                foreach (Conduit cond in conduitlist)
                {
                    if (conduitlist[0].Id != cond.Id)
                    {
                        //findintersection
                        XYZ con1_firstconduitpoint = (conduitlist[0].Location as LocationCurve).Curve.GetEndPoint(0);
                        XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                        XYZ con2_secondconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(1);
                        double distance = Math.Pow(con1_firstconduitpoint.X - con2_firstconduitpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - con2_firstconduitpoint.Y, 2);
                        distance = Math.Sqrt(distance);

                        if (distance == max_distance)
                        {
                            outerconduit = cond;
                        }

                    }
                }
                return outerconduit;

            }
            else
            {
                foreach (Conduit cond in conduitlist.Skip(1))
                {

                    //findintersection
                    Line conduitline_dir = (conduitlist[0].Location as LocationCurve).Curve as Line;
                    XYZ cond1_crossproduct = conduitline_dir.Direction.CrossProduct(XYZ.BasisZ);
                    XYZ con1_firstconduitpoint = (conduitlist[0].Location as LocationCurve).Curve.GetEndPoint(0);
                    XYZ con1_secondconduitpoint = con1_firstconduitpoint + cond1_crossproduct.Multiply(5);

                    XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                    XYZ con2_secondconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(1);

                    XYZ intersectionpoint = FindIntersectionPoint(con1_firstconduitpoint, con1_secondconduitpoint, con2_firstconduitpoint, con2_secondconduitpoint);
                    double distance = 0;
                    if (intersectionpoint != null)
                        distance = Math.Pow(con1_firstconduitpoint.X - intersectionpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - intersectionpoint.Y, 2);

                    distance = Math.Sqrt(distance);
                    distancecollection.Add(distance);


                }
                max_distance = distancecollection.Max<double>();
                foreach (Conduit cond in conduitlist.Skip(1))
                {

                    //findintersection
                    Line conduitline_dir = (conduitlist[0].Location as LocationCurve).Curve as Line;
                    XYZ cond1_crossproduct = conduitline_dir.Direction.CrossProduct(XYZ.BasisZ);
                    XYZ con1_firstconduitpoint = (conduitlist[0].Location as LocationCurve).Curve.GetEndPoint(0);
                    XYZ con1_secondconduitpoint = con1_firstconduitpoint + cond1_crossproduct.Multiply(5);

                    XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                    XYZ con2_secondconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(1);

                    XYZ intersectionpoint = FindIntersectionPoint(con1_firstconduitpoint, con1_secondconduitpoint, con2_firstconduitpoint, con2_secondconduitpoint);
                    double distance = 0;
                    if (intersectionpoint != null)
                        distance = Math.Pow(con1_firstconduitpoint.X - intersectionpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - intersectionpoint.Y, 2);
                    distance = Math.Sqrt(distance);

                    if (distance == max_distance)
                    {
                        outerconduit = cond;
                    }


                }
                return outerconduit;
            }
        }
        public static List<Element> ConduitInOrder(List<Element> conduits)
        {
            Conduit outerconduit = FindOuterConduit(conduits);
            List<Element> orederedconduits = new List<Element>();
            List<double> dis_collection = new List<double>();
            XYZ point1 = (conduits[0].Location as LocationCurve).Curve.GetEndPoint(0);
            XYZ point2 = (conduits[0].Location as LocationCurve).Curve.GetEndPoint(1);
            if (Math.Round(point1.X, 4) == Math.Round(point2.X, 4) && Math.Round(point1.Y, 4) == Math.Round(point2.Y, 4))
            {

                foreach (Conduit cond in conduits)
                {
                    if (outerconduit.Id != cond.Id)
                    {
                        //findintersection
                        XYZ con1_firstconduitpoint = (outerconduit.Location as LocationCurve).Curve.GetEndPoint(0);
                        XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                        XYZ con2_secondconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(1);

                        double distance = Math.Pow(con1_firstconduitpoint.X - con2_firstconduitpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - con2_firstconduitpoint.Y, 2);
                        distance = Math.Sqrt(distance);
                        dis_collection.Add(distance);

                    }
                }
                dis_collection = dis_collection.OrderBy(o => o).ToList();
                orederedconduits.Add(outerconduit);
                foreach (double dou in dis_collection)
                {
                    foreach (Conduit cond in conduits)
                    {
                        if (outerconduit.Id != cond.Id)
                        {
                            //findintersection
                            XYZ con1_firstconduitpoint = (outerconduit.Location as LocationCurve).Curve.GetEndPoint(0);
                            XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                            XYZ con2_secondconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(1);

                            double distance = Math.Pow(con1_firstconduitpoint.X - con2_firstconduitpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - con2_firstconduitpoint.Y, 2);
                            distance = Math.Sqrt(distance);

                            if (dou == distance)
                            {
                                orederedconduits.Add(cond);
                            }

                        }
                    }
                }
                return orederedconduits;
            }
            else
            {
                foreach (Conduit cond in conduits)
                {
                    if (outerconduit.Id != cond.Id)
                    {
                        //findintersection
                        Line conduitline_dir = (outerconduit.Location as LocationCurve).Curve as Line;
                        XYZ cond1_crossproduct = conduitline_dir.Direction.CrossProduct(XYZ.BasisZ);
                        XYZ con1_firstconduitpoint = (outerconduit.Location as LocationCurve).Curve.GetEndPoint(0);
                        XYZ con1_secondconduitpoint = con1_firstconduitpoint + cond1_crossproduct.Multiply(5);

                        XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                        XYZ con2_secondconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(1);

                        XYZ intersectionpoint = FindIntersectionPoint(con1_firstconduitpoint, con1_secondconduitpoint, con2_firstconduitpoint, con2_secondconduitpoint);
                        double distance = 0;
                        if (intersectionpoint != null)
                            distance = Math.Pow(con1_firstconduitpoint.X - intersectionpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - intersectionpoint.Y, 2);
                        distance = Math.Sqrt(distance);
                        dis_collection.Add(distance);

                    }
                }
                dis_collection = dis_collection.OrderBy(o => o).ToList();
                orederedconduits.Add(outerconduit);
                foreach (double dou in dis_collection)
                {
                    foreach (Conduit cond in conduits)
                    {
                        if (outerconduit.Id != cond.Id)
                        {
                            //findintersection
                            Line conduitline_dir = (outerconduit.Location as LocationCurve).Curve as Line;
                            XYZ cond1_crossproduct = conduitline_dir.Direction.CrossProduct(XYZ.BasisZ);
                            XYZ con1_firstconduitpoint = (outerconduit.Location as LocationCurve).Curve.GetEndPoint(0);
                            XYZ con1_secondconduitpoint = con1_firstconduitpoint + cond1_crossproduct.Multiply(5);

                            XYZ con2_firstconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(0);
                            XYZ con2_secondconduitpoint = (cond.Location as LocationCurve).Curve.GetEndPoint(1);

                            XYZ intersectionpoint = FindIntersectionPoint(con1_firstconduitpoint, con1_secondconduitpoint, con2_firstconduitpoint, con2_secondconduitpoint);
                            double distance = 0;
                            if (intersectionpoint != null)
                                distance = Math.Pow(con1_firstconduitpoint.X - intersectionpoint.X, 2) + Math.Pow(con1_firstconduitpoint.Y - intersectionpoint.Y, 2);
                            distance = Math.Sqrt(distance);

                            if (dou == distance)
                            {
                                orederedconduits.Add(cond);
                            }

                        }
                    }
                }
                return orederedconduits;
            }

        }
        public static Dictionary<Conduit, List<XYZ>> GetClosestSideAxisForConduit(List<Conduit> conduits)
        {
            Dictionary<Conduit, List<XYZ>> keyValuePairsFirst = new Dictionary<Conduit, List<XYZ>>();
            Dictionary<Conduit, List<XYZ>> keyValuePairsSec = new Dictionary<Conduit, List<XYZ>>();
            XYZ startKey = null;
            XYZ endKey = null;
            GetStartEndPointForConduit(conduits[0], ref startKey, ref endKey);
            keyValuePairsFirst.Add(conduits[0], new List<XYZ>() { startKey, endKey });
            keyValuePairsSec.Add(conduits[0], new List<XYZ>() { endKey, startKey });
            double startCount = 0;
            double endCount = 0;
            foreach (Conduit con in conduits.Skip(1))
            {
                XYZ start = null;
                XYZ end = null;
                GetStartEndPointForConduit(con, ref start, ref end);
                if (startKey.DistanceTo(start) < startKey.DistanceTo(end))
                {
                    keyValuePairsFirst.Add(con, new List<XYZ>() { start, end });
                    startCount += startKey.DistanceTo(start);
                }
                else
                {
                    keyValuePairsFirst.Add(con, new List<XYZ>() { end, start });
                    startCount += startKey.DistanceTo(end);
                }

                if (endKey.DistanceTo(end) < endKey.DistanceTo(start))
                {
                    keyValuePairsSec.Add(con, new List<XYZ>() { end, start });
                    endCount += endKey.DistanceTo(end);
                }
                else
                {
                    keyValuePairsSec.Add(con, new List<XYZ>() { start, end });
                    endCount += endKey.DistanceTo(start);
                }
            }

            return startCount < endCount ? keyValuePairsFirst : keyValuePairsSec;
        }
        public static List<Element> GetConduitsByReference(IList<Reference> References, Document doc)
        {
            List<Element> conduits = new List<Element>();
            foreach (Reference r in References)
            {
                Element e = doc.GetElement(r);
                if (e.GetType() == typeof(Conduit))
                {
                    conduits.Add(e);
                }
            }
            return conduits;
        }
    }
}
