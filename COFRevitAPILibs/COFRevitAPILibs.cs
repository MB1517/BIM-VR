using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace COFRevitAPILibs
{
    /// <summary>
    /// Tiện ích cho Geometry (hình học)
    /// </summary>
    public partial class GeoUtil
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        // class constructor
        public GeoUtil() { }
        public GeoUtil(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;
        }
        public IEnumerable<Solid> GetSolids(Element element)
        {
            var geometry = element.get_Geometry(new Options { ComputeReferences = true });

            return GetSolids(geometry)
                .Where(x => x.Volume > 0);
        }
        /// <summary>
        /// Lấy các solid của đối tượng GeometryObject
        /// </summary>
        /// <param name="geometryElement"></param>
        /// <returns></returns>
        public static IEnumerable<Solid> GetSolids(IEnumerable<GeometryObject> geometryElement)
        {
            foreach (var geometry in geometryElement)
            {
                var solid = geometry as Solid;
                if (solid != null)
                    yield return solid;

                var instance = geometry as GeometryInstance;
                if (instance != null)
                    foreach (var instanceSolid in GetSolids(instance.GetSymbolGeometry()))
                        yield return instanceSolid;

                var element = geometry as GeometryElement;
                if (element != null)
                    foreach (var elementSolid in GetSolids(element))
                        yield return elementSolid;
            }
        }
        /// <summary>
        /// Phương thức tạo Line từ Detail line được chọn
        /// </summary>
        /// <returns></returns>
        public Line LineByPickDetailLine()
        {
            DetailLine dtline = (DetailLine)doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn detail line").ElementId);
            Line line = Line.CreateBound(dtline.GeometryCurve.GetEndPoint(0), dtline.GeometryCurve.GetEndPoint(1));
            return line;
        }
        /// <summary>
        /// Tạo model line, trả về List Curve
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="crv"></param>
        /// <param name="isListCurve"></param>
        /// <returns></returns>
        public ModelCurveArray CreateModelLinesFromCurveArray(CurveArray crv)
        {
            XYZ origin = new XYZ(0, 0, 0);
            XYZ normal = new XYZ(0, 0, 1);
            Transaction t = new Transaction(doc, "ModelCurve");
            t.Start();
            Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin);
            SketchPlane sketch = SketchPlane.Create(doc, geomPlane);
            ModelCurveArray line1 = doc.Create.NewModelCurveArray(crv, sketch) as ModelCurveArray;
            t.Commit();
            return line1;
        }
        /// <summary>
        /// Tạo model line từ Curve
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public ModelCurve CreateModelLinesFromCurve(Curve c)
        {
            CurveArray crv = new CurveArray();
            crv.Append(c);
            XYZ origin = new XYZ(0, 0, 0);
            XYZ normal = new XYZ(0, 0, 1);
            Transaction t = new Transaction(doc, "ModelCurve");
            t.Start();
            Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin);
            SketchPlane sketch = SketchPlane.Create(doc, geomPlane);
            ModelCurveArray mca = doc.Create.NewModelCurveArray(crv, sketch) as ModelCurveArray;
            ModelCurve mc = mca.get_Item(0);
            t.Commit();
            return mc;
        }
        /// <summary>
        /// Tạo model line từ 1 Curve và Origin
        /// </summary>
        /// <param name="c"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public ModelCurve CreateModelLinesFromCurve(Curve c, XYZ origin)
        {
            CurveArray crv = new CurveArray();
            crv.Append(c);
            XYZ normal = new XYZ(0, 0, 1);
            Transaction t = new Transaction(doc, "ModelCurve");
            t.Start();
            Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin);
            SketchPlane sketch = SketchPlane.Create(doc, geomPlane);
            ModelCurveArray mca = doc.Create.NewModelCurveArray(crv, sketch) as ModelCurveArray;
            ModelCurve mc = mca.get_Item(0);
            t.Commit();
            return mc;
        }
        /// <summary>
        /// Tìm giao điểm Curve vs Element
        /// </summary>
        /// <param name="c"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public XYZ IntersectPoint(Curve c, Element e)
        {
            // CHUA XONG , DANG PHAT TRIEN VA KIEM TRA LAI
            List<Solid> solids = GetSolids(e).ToList();
            Solid solid = solids[0];
            SolidCurveIntersection solidCurveIntersect = solid.IntersectWithCurve(c, new SolidCurveIntersectionOptions());
            IEnumerator<Curve> intersectCurves = solidCurveIntersect.GetEnumerator();
            List<Curve> listCurveIntersect = new List<Curve>();
            while (intersectCurves.MoveNext())
            {
                listCurveIntersect.Add(intersectCurves.Current);
            }
            if (listCurveIntersect.Count == 1)
            {
                Curve c1 = listCurveIntersect[0];
                //IntersectionResultArray resarray = new IntersectionResultArray();
                //SetComparisonResult curveIntersecRes = c1.Intersect(c2, out resarray);
                //if (curveIntersecRes == SetComparisonResult.Subset && resarray.Size == 1)
                //{
                //    if (c1.GetEndPoint(0).Equals(c.GetEndPoint(0)) || c1.GetEndPoint(0).Equals(c.GetEndPoint(1)))
                //    {
                //        return c1.GetEndPoint(0);
                //    }
                //    else
                //    {
                //        return c1.GetEndPoint(1);
                //    }
                //}

                if (c1.GetEndPoint(0).Equals(c.GetEndPoint(0)))
                {
                    return c1.GetEndPoint(1);
                }
                else if (c1.GetEndPoint(0).Equals(c.GetEndPoint(1)))
                {
                    return c1.GetEndPoint(1);
                }
                else
                {
                    return c1.GetEndPoint(0);
                }
            }
            return null;
        }
        /// <summary>
        /// Tìm Curve Trên cùng của 1 bề mặt Face
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public Curve TopCurveOfFace(Face face)
        {
            List<CurveLoop> listCurveLoop = face.GetEdgesAsCurveLoops().ToList();
            CurveLoop curveLoop = listCurveLoop[0];
            XYZ ptTest = new XYZ(0, 0, 0);
            Curve TopCurve = Line.CreateBound((new XYZ(0, 0, 0)), (new XYZ(10, 0, 0))) as Curve;
            foreach (Curve c in curveLoop)
            {
                XYZ ptMid = GetMidPoint(c);
                if (ptMid.Z > ptTest.Z)
                {
                    ptTest = ptMid;
                    TopCurve = c;
                }
            }

            return TopCurve;
        }
        /// <summary>
        /// Tìm Curve Dưới cùng của 1 bề mặt Face
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public Curve BottomCurveOfFace(Face face)
        {
            List<CurveLoop> listCurveLoop = face.GetEdgesAsCurveLoops().ToList();
            CurveLoop curveLoop = listCurveLoop[0];
            XYZ ptTest = new XYZ(0, 0, 100000000 / 304);
            Curve bottomCurve = Line.CreateBound((new XYZ(0, 0, 0)), (new XYZ(10, 0, 0))) as Curve;
            foreach (Curve c in curveLoop)
            {
                XYZ ptMid = GetMidPoint(c);
                if (ptMid.Z < ptTest.Z)
                {
                    ptTest = ptMid;
                    bottomCurve = c;
                }
            }

            return bottomCurve;
        }
        /// <summary>
        /// Tìm các Curve của 1 bề mặt Face
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public List<Curve> CurvesOfFace(Face face)
        {
            List<CurveLoop> listCurveLoop = face.GetEdgesAsCurveLoops().ToList();
            CurveLoop curveLoop = listCurveLoop[0];
            List<Curve> listCurve = new List<Curve>();
            foreach (Curve c in curveLoop)
            {
                listCurve.Add(c);
            }

            return listCurve;
        }
        /// <summary>
        /// Lấy trung điểm của 1 Curve
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public XYZ GetMidPoint(Curve c)
        {
            XYZ pt0 = c.GetEndPoint(0);
            XYZ pt1 = c.GetEndPoint(1);
            XYZ ptMid = new XYZ((pt0.X + pt1.X) / 2, (pt0.Y + pt1.Y) / 2, (pt0.Z + pt1.Z) / 2);
            return ptMid;
        }
        /// <summary>
        /// Tạo mới 1 Curve với thông số Z mới
        /// </summary>
        /// <param name="c"></param>
        /// <param name="newZ"></param>
        /// <returns></returns>
        public Curve newCurveDifferZ(Curve c, double newZ)
        {
            XYZ pt1 = new XYZ(c.GetEndPoint(0).X, c.GetEndPoint(0).Y, newZ);
            XYZ pt2 = new XYZ(c.GetEndPoint(1).X, c.GetEndPoint(1).Y, newZ);
            Curve newC = Line.CreateBound(pt1, pt2) as Curve;
            return newC;
        }
        /// <summary>
        /// Chọn Planar Face
        /// </summary>
        /// <returns></returns>
        public PlanarFace PickPlanarFace()
        {
            Reference faceRef = uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Face, "Please pick a planar face to set the work plane. ESC for cancel.");
            GeometryObject geoObject = doc.GetElement(faceRef).GetGeometryObjectFromReference(faceRef);
            PlanarFace planarFace = geoObject as PlanarFace;
            return planarFace;
        }
        /// <summary>
        ///Lấy Curve Thấp nhất từ DS Curve
        /// </summary>
        /// <param name="lC1"></param>
        /// <returns></returns>
        public Curve GetLowestCurve(List<Curve> lC1)
        {
            List<Double> listZ1 = new List<double>();
            foreach (Curve c1 in lC1)
            {
                XYZ pt = c1.Evaluate(0.5, false);
                double z = pt.Z;
                listZ1.Add(z);
            }
            int i = 0;
            for (int j = 1; j < listZ1.Count; j++)
            { if (listZ1[j] < listZ1[i]) i = j; }
            Curve foundC = lC1[i];

            return foundC;
        }
        /// <summary>
        ///Lấy DS Curve của mặt Ngoài = true | mặt Trong = False
        /// </summary>
        /// <param name="plnF1"></param>
        /// <param name="whichFace"></param>
        /// <returns></returns>
        public List<Curve> GetCurvesOfPlanarFace(Wall w, bool whichFace = true)
        {
            if (whichFace)
            {
                IList<Reference> ilistFacesExt = HostObjectUtils.GetSideFaces(w as HostObject, ShellLayerType.Exterior);//MessageBox.Show(ilistFacesExt.Count.ToString());        	
                GeometryObject geoE = doc.GetElement(ilistFacesExt[0]).GetGeometryObjectFromReference(ilistFacesExt[0]);
                PlanarFace plnF1 = geoE as PlanarFace; //MessageBox.Show(plnF1.Area.ToString()); 
                IList<CurveLoop> ilCL1 = plnF1.GetEdgesAsCurveLoops(); //MessageBox.Show(ilCL1.Count.ToString()); 
                CurveLoop cl1 = ilCL1[0]; //MessageBox.Show(cl1.Count<Curve>().ToString());
                List<Curve> lC1 = cl1.ToList<Curve>(); //MessageBox.Show(lC1.Count.ToString());
                return lC1;
            }
            else
            {
                IList<Reference> ilistFacesExt = HostObjectUtils.GetSideFaces(w as HostObject, ShellLayerType.Interior);//MessageBox.Show(ilistFacesExt.Count.ToString());        	
                GeometryObject geoE = doc.GetElement(ilistFacesExt[0]).GetGeometryObjectFromReference(ilistFacesExt[0]);
                PlanarFace plnF1 = geoE as PlanarFace; //MessageBox.Show(plnF1.Area.ToString()); 
                IList<CurveLoop> ilCL1 = plnF1.GetEdgesAsCurveLoops(); //MessageBox.Show(ilCL1.Count.ToString()); 
                CurveLoop cl1 = ilCL1[0]; //MessageBox.Show(cl1.Count<Curve>().ToString());
                List<Curve> lC1 = cl1.ToList<Curve>(); //MessageBox.Show(lC1.Count.ToString());
                return lC1;
            }
        }
        /// <summary>
        /// Lấy các Face song song với Line
        /// </summary>
        /// <param name="wSo"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<Face> ParallelFacesOfSolid(Solid wSo, Line line)
        {
            XYZ pt1 = line.Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);

            FaceArray faceArr = wSo.Faces;
            List<Face> listFace = new List<Face>();
            foreach (Face f in faceArr)
            {
                XYZ pt2 = f.ComputeNormal(new UV()).Normalize();
                double x = Math.Round(pt2.X, 3);
                double y = Math.Round(pt2.Y, 3);
                double z = Math.Round(pt2.Z, 3);

                bool flag1 = Math.Abs(z) != 1;

                bool flag2 = Math.Abs(y) == Math.Abs(x1) && Math.Abs(x) == Math.Abs(y1);

                if (flag1 && flag2) listFace.Add(f);
            }
            return listFace;
        }
        /// <summary>
		/// Lấy các Face song song với Curve
		/// </summary>
		/// <param name="wSo"></param>
		/// <param name="cr"></param>
		/// <returns></returns>
		public List<Face> ParallelFacesOfSolid(Solid wSo, Curve cr)
        {

            Line line = Line.CreateBound(cr.GetEndPoint(0), cr.GetEndPoint(1));

            XYZ pt1 = line.Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);

            FaceArray faceArr = wSo.Faces;
            List<Face> listFace = new List<Face>();
            foreach (Face f in faceArr)
            {
                XYZ pt2 = f.ComputeNormal(new UV()).Normalize();
                double x = Math.Round(pt2.X, 3);
                double y = Math.Round(pt2.Y, 3);
                double z = Math.Round(pt2.Z, 3);

                bool flag1 = Math.Abs(z) != 1;

                bool flag2 = Math.Abs(y) == Math.Abs(x1) && Math.Abs(x) == Math.Abs(y1);

                if (flag1 && flag2) listFace.Add(f);
            }
            return listFace;
        }        
        /// <summary>
        /// Lấy các Curve của Mặt phẳng tham chiếu ref plane
        /// </summary>
        /// <param name="rp"></param>
        /// <returns></returns>
        public Curve GetCurveOfReferencePlane(ReferencePlane rp)
        {
            Curve c = null;
            //GeometryElement geoElem = rp.get_Geometry(new Options());
            //foreach (GeometryObject geoObj in geoElem)
            //{
            //    c =  geoObj as Curve; ;
            //    break;
            //}
            IList<Curve> IlistCurve = rp.GetCurvesInView(DatumExtentType.ViewSpecific, doc.ActiveView);
            c = IlistCurve.First();
            return c;
        }
        /// <summary>
        /// Lấy Điểm của Curve tại 1 tham số
        /// </summary>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public XYZ GetPointOfCurveAtParameter(Curve c, double d)
        {

            double x = c.GetEndPoint(0).X + (c.GetEndPoint(1).X - c.GetEndPoint(0).X) * d;
            double y = c.GetEndPoint(0).Y + (c.GetEndPoint(1).Y - c.GetEndPoint(0).Y) * d;
            double z = c.GetEndPoint(0).Z + (c.GetEndPoint(1).Z - c.GetEndPoint(0).Z) * d;
            XYZ pt = new XYZ(x, y, z);
            return pt;
        }
        /// <summary>
        /// Lấy điểm Pháp tuyến
        /// </summary>
        /// <param name="direct"></param>
        /// <returns></returns>
        public XYZ normalDirect(XYZ direct)
        {
            XYZ normal = Transform.CreateRotation(XYZ.BasisZ, Math.PI / 2).OfPoint(direct).Normalize();
            return normal;
        }
        /// <summary>
        /// Lấy điểm Pháp tuyến
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public XYZ normalDirect(XYZ direct, double angle)
        {
            XYZ normal = Transform.CreateRotation(XYZ.BasisZ, angle).OfPoint(direct).Normalize();
            return normal;
        }
        /// <summary>
        /// Lấy điểm Pháp tuyến
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public XYZ normalDirect(XYZ direct, bool reverse = true)
        {
            XYZ normal = Transform.CreateRotation(XYZ.BasisZ, Math.PI / (-2)).OfPoint(direct).Normalize();
            return normal;
        }
        /// <summary>
        /// Tạo chữ thập từ 1 điểm
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="w"></param>
        /// <param name="d"></param>
        /// <param name="pt0"></param>
        /// <param name="direct"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public List<Line> CrossLinesFromPoint(double offset, double w, double d, XYZ pt0, XYZ direct, double angle) // tao dau chu Thap
        {
            List<Line> listRes = new List<Line>();
            XYZ normal = new XYZ();
            if (angle == 0) normal = normalDirect(direct, Math.PI / 2);
            else if (angle != 0 && angle % Math.PI / 2 == 0) normal = normalDirect(direct, Math.PI);
            else normal = normalDirect(direct, Math.PI / (-2));
            XYZ pt = Transform.CreateTranslation(normal.Multiply(Math.Abs(d / 2))).OfPoint(pt0);//(normal.Multiply(d/2)).OfPoint(pt0);//offset+ //new XYZ(normal.X*d/2,normal.Y*d/2,normal.Z)

            //                    pt1                
            //      -------------+-------------
            //      |        b    d/2         |
            //      |    -w/2    .    w/2   c |
            //  pt4 +    a        pt          + pt2
            //      |            -d/2    d    |
            //      -------------+-------------
            //                    pt3
            //            double x = pt.X;
            //            double y = pt.Y;
            //            double z = pt.Z;//            
            //            double xx = direct.X;
            //            double yy = direct.Y;//            
            //            double a = -xx*w/2;
            //            double b = yy*d/2;
            //            double c = xx*w/2;
            //            double d = -yy*d/2;            
            XYZ pt1 = Transform.CreateTranslation(direct.Multiply(w / 2)).OfPoint(pt);
            XYZ pt2 = Transform.CreateTranslation(normal.Multiply(d / 2)).OfPoint(pt);
            XYZ pt3 = Transform.CreateTranslation(direct.Multiply(-w / 2)).OfPoint(pt);
            XYZ pt4 = Transform.CreateTranslation(normal.Multiply(-d / 2)).OfPoint(pt);

            XYZ pt101 = Transform.CreateTranslation(normal.Multiply(d / 2)).OfPoint(pt1);
            XYZ pt102 = Transform.CreateTranslation(normal.Multiply(-d / 2)).OfPoint(pt1);

            XYZ pt301 = Transform.CreateTranslation(normal.Multiply(d / 2)).OfPoint(pt3);
            XYZ pt302 = Transform.CreateTranslation(normal.Multiply(-d / 2)).OfPoint(pt3);

            XYZ pt201 = Transform.CreateTranslation(direct.Multiply(w / 2)).OfPoint(pt2);
            XYZ pt202 = Transform.CreateTranslation(direct.Multiply(-w / 2)).OfPoint(pt2);

            XYZ pt401 = Transform.CreateTranslation(direct.Multiply(w / 2)).OfPoint(pt4);
            XYZ pt402 = Transform.CreateTranslation(direct.Multiply(-w / 2)).OfPoint(pt4);

            Line line1 = Line.CreateBound(pt101, pt102);
            Line line2 = Line.CreateBound(pt201, pt202);
            Line line3 = Line.CreateBound(pt301, pt302);
            Line line4 = Line.CreateBound(pt401, pt402);

            listRes.Add(line1); listRes.Add(line2); listRes.Add(line3); listRes.Add(line4);

            return listRes;
        }
        /// <summary>
        /// Tạo Line Phương Y từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line YlineByPoint(XYZ pt, double d = 30)
        {
            XYZ pt1 = new XYZ(pt.X, pt.Y + d, 0);
            XYZ pt2 = new XYZ(pt.X, pt.Y - d, 0);
            Line line = Line.CreateBound(pt1, pt2);
            return line;
        }
        /// <summary>
        /// Tạo Line Phương Z từ point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line ZlineByPoint(XYZ pt, double d = 30)
        {
            XYZ pt1 = new XYZ(pt.X, pt.Y, pt.Z + d);
            XYZ pt2 = new XYZ(pt.X, pt.Y, pt.Z - d);
            Line line = Line.CreateBound(pt1, pt2);
            return line;
        }
        /// <summary>
        /// Tạo Line Phương X từ point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line XlineByPoint(XYZ pt, double d = 30)
        {
            XYZ pt1 = new XYZ(pt.X + d, pt.Y, 0);
            XYZ pt2 = new XYZ(pt.X - d, pt.Y, 0);
            Line line = Line.CreateBound(pt1, pt2);
            return line;
        }
        /// <summary>
        /// Kiểm tra liệu Điểm đầu có nằm ngoài
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="P3"></param>
        /// <returns></returns>
        public bool CheckIfFirstPointIsOutSide(XYZ P1, XYZ P2, XYZ P3)
        {
            double P12 = Math.Abs(P1.DistanceTo(P2));
            double P13 = Math.Abs(P1.DistanceTo(P3));
            double P23 = Math.Abs(P2.DistanceTo(P3));

            double cos1 = (P13 * P13 + P12 * P12 - P23 * P23) / (2 * P13 * P12);
            double cos2 = (P12 * P12 + P23 * P23 - P13 * P13) / (2 * P12 * P23);
            double cos3 = (P13 * P13 + P23 * P23 - P12 * P12) / (2 * P13 * P23);

            if (cos2 < 0 || cos3 < 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Kiểm tra 2 curve liệu có Song song
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public bool IsParallel(Curve c1, Curve c2)
        {
            XYZ pt1 = Line.CreateBound(c1.GetEndPoint(0), c1.GetEndPoint(1)).Direction.Normalize();
            XYZ pt2 = Line.CreateBound(c2.GetEndPoint(0), c2.GetEndPoint(1)).Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == x2 || y1 == y2)//|| pt1.Z == pt2.Z
                return true;
            return false;
        }
        /// <summary>
        /// Góc của 1 Curve với Trục X phương Ngang
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public double AngleOfCurve(Curve c)
        {
            XYZ linePt = new XYZ();
            if (c.GetEndPoint(0).X > c.GetEndPoint(1).X) linePt = c.GetEndPoint(0);//.Normalize();
            else linePt = c.GetEndPoint(1);//.Normalize();
            XYZ linePt2 = XaxisLineFromPoint(linePt, 10).Direction.Normalize();
            XYZ linePt1 = (c as Line).Direction.Normalize();
            double angle = linePt1.AngleTo(linePt2);
            return angle;
        }
        /// <summary>
        /// Đường Line trục X phương ngang
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="plusD"></param>
        /// <returns></returns>
        public Line XaxisLineFromPoint(XYZ pt, double plusD)
        {
            Line line = Line.CreateBound(pt, new XYZ(pt.X + plusD, pt.Y, pt.Z));
            return line;
        }
        /// <summary>
        /// Đường Line trục Z
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="plusD"></param>
        /// <returns></returns>
        public Line ZaxisLineFromPoint(XYZ pt, double plusD)
        {
            Line line = Line.CreateBound(pt, new XYZ(pt.X, pt.Y, pt.Z + plusD));
            return line;
        }
        /// <summary>
        /// Đường Line trục Z
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="plusD"></param>
        /// <param name="minorD"></param>
        /// <returns></returns>
        public Line ZaxisLineFromPoint(XYZ pt, double plusD, double minorD)
        {
            Line line = Line.CreateBound(new XYZ(pt.X, pt.Y, pt.Z - minorD), new XYZ(pt.X, pt.Y, pt.Z + plusD));
            return line;
        }
        /// <summary>
        /// Tạo điểm đối xứng pt2 với pt1 qua 1 điểm pt
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="pt1"></param>
        /// <returns></returns>
        public XYZ PointMirrorPoint(XYZ pt, XYZ pt1)
        {
            double x, y, x1, y1, x2, y2, deltaX, deltaY;
            x = pt.X; y = pt.Y;
            x1 = pt1.X; y1 = pt1.Y;
            deltaX = x - x1;
            deltaY = y - y1;
            x2 = x + deltaX;
            y2 = y + deltaY;
            XYZ pt2 = new XYZ(x2, y2, pt1.Z);
            return pt2;
        }
        
    }
    /// <summary>
    /// Class So sánh các đối tượng Giao cắt (Intersect)
    /// </summary>
    public partial class IntersectComparation
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        public List<Curve> listCurve1, listCurve2;

        public int OverlapCount1Turn = 0;
        public int EqualCount1Turn = 0;
        public int SubsetCount1Turn = 0;

        public bool hasOverlap = false;
        public bool hasEqual = false;
        public bool hasSubset = false;

        public List<XYZ> OverlapXYZs;
        public List<Curve> OverLapCurves, EqualCurves, SubsetCurves;

        public Dictionary<string, OverlapElements> IntersectOverlapElementDictionary;
        public List<OverlapElements> overlapElements;
        public List<EqualElements> equalElements;
        public List<SubsetElements> subsetElements;

        // class constructor
        public IntersectComparation() { }
        public IntersectComparation(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app,
            List<Curve> _listCurve1, List<Curve> _listCurve2)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;
            foreach (Curve c1 in listCurve1)
            {
                foreach (Curve c2 in listCurve2)
                {
                    IntersectionResultArray intersectResArr = new IntersectionResultArray();
                    SetComparisonResult setCompareRes = c1.Intersect(c2, out intersectResArr);
                    if (setCompareRes == SetComparisonResult.Overlap)
                    {
                        OverlapCount1Turn += 1;
                        OverlapXYZs.Add(intersectResArr.get_Item(0).XYZPoint);
                        OverLapCurves.Add(c1); OverLapCurves.Add(c2);
                        overlapElements.Add(new OverlapElements(c1, intersectResArr.get_Item(0).XYZPoint));
                        overlapElements.Add(new OverlapElements(c2, intersectResArr.get_Item(0).XYZPoint));
                    }
                    else if (setCompareRes == SetComparisonResult.Equal) { EqualCount1Turn += 1; equalElements.Add(new EqualElements(c1, c2)); }
                    else if (setCompareRes == SetComparisonResult.Subset) { SubsetCount1Turn += 1; subsetElements.Add(new SubsetElements(c1, c2, intersectResArr.get_Item(0).XYZPoint)); }
                }
            }
            if (OverlapCount1Turn > 0) hasOverlap = true;
            if (EqualCount1Turn > 0) hasEqual = true;
            if (SubsetCount1Turn > 0) hasSubset = true;
        }
        public bool DoesDisjoint()
        {
            bool flag = false;
            int i = 0;
            foreach (Curve c1 in listCurve1)
            {
                foreach (Curve c2 in listCurve2)
                {
                    IntersectionResultArray intersectResArr = new IntersectionResultArray();
                    SetComparisonResult setCompareRes = c1.Intersect(c2, out intersectResArr);
                    if (setCompareRes == SetComparisonResult.Overlap
                        || setCompareRes == SetComparisonResult.Equal
                        || setCompareRes == SetComparisonResult.Subset) i += 1;
                }
            }
            if (i > 0) flag = true;
            return flag;
        }
        public bool DoesOverlapTotally()
        {
            bool flag = false;
            //List<XYZ> listPt = new List<XYZ>();
            //int i = 0;
            //foreach (Curve c1 in listCurve1)
            //{
            //    foreach (Curve c2 in listCurve2)
            //    {
            //        IntersectionResultArray intersectResArr = new IntersectionResultArray();
            //        SetComparisonResult setCompareRes = c1.Intersect(c2, out intersectResArr);
            //        if (setCompareRes == SetComparisonResult.Overlap)
            //        {
            //            i += 1;
            //            listPt.Add(intersectResArr.get_Item(0).XYZPoint);
            //        }
            //    }
            //}
            //bool flagOverlap = i > 0;
            //bool flagListOfTwo = listPt.Count == 2;
            //bool flag2PointNotOverLap = false;
            //XYZ pt1 = listPt[0];
            //XYZ pt2 = listPt[1];
            //if (pt1.X != pt2.X || pt1.Y != pt2.Y || pt1.Z != pt2.Z) flag2PointNotOverLap = true;
            //if (flagOverlap && flagListOfTwo && flag2PointNotOverLap) flag = true;
            if (OverlapCount1Turn > 2 && EqualCount1Turn == 0 && SubsetCount1Turn == 0) flag = true;
            return flag;
        }

    }
    /// <summary>
    /// Class các Yếu tố Trùng lắp (Overlapse) trong so sánh Giao cắt
    /// </summary>
    public partial class OverlapElements
    {
        public string key;
        public Curve curve { get; set; }
        public XYZ XYZintersect { get; set; }
        public OverlapElements(Curve _curve, XYZ _XYZintersect, string _key)
        {
            curve = _curve;
            XYZintersect = _XYZintersect;
            key = _key;
        }
        public OverlapElements(Curve _curve, XYZ _XYZintersect)
        {
            curve = _curve;
            XYZintersect = _XYZintersect;
        }
        //public Dictionary<string,IntersectElement> IntersectOverlapElementDictionary()
        //{
        //    var elements = new Dictionary<string, IntersectElement>();
        //    //AddToDictionary();
        //    return elements;
        //}
        private static void AddToDictionary(Dictionary<string, OverlapElements> elements, Curve curve, XYZ XYZintersect, string key)
        {
            OverlapElements theElement = new OverlapElements(curve, XYZintersect, key);
            elements.Add(key: theElement.key, value: theElement);
        }
    }
    /// <summary>
    /// Class các Yếu tố Bằng nhau (Equal) trong so sánh Giao cắt
    /// </summary>
    public partial class EqualElements
    {
        public string key;
        public List<Curve> EqualCurves;
        public EqualElements(Curve _curve1, Curve _curve2)
        {
            EqualCurves.Add(_curve1);
            EqualCurves.Add(_curve2);
        }
    }
    /// <summary>
    /// Class các Yếu tố Chính phụ Subset trong so sánh Giao cắt
    /// </summary>
    public partial class SubsetElements
    {
        public string key;
        public List<Curve> SubsetCurves;
        public XYZ subsetXYZ;
        public SubsetElements(Curve _curve1, Curve _curve2, XYZ _pt)
        {
            SubsetCurves.Add(_curve1);
            SubsetCurves.Add(_curve2);
            subsetXYZ = _pt;
        }
    }
    /// <summary>
    /// Class công tác Giao nhập (Join) Hình học (Geometry)
    /// </summary>
    public partial class JoinGeometry
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;

        public List<Element> FOUNDATIONS, COLUMNS, BEAMS, SLABS;


        // class constructor
        public JoinGeometry(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;
        }
        public void FilterStructureElements(List<Element> listElems, ref List<Element> _foundations, ref List<Element> _columns, ref List<Element> _beams, ref List<Element> _slabs)
        {
            //foreach (Element e in listElems)
            //{
            //    if (e.Category.Name == "Structural Foundation") _foundations.Add(e);
            //    else if (e.Category.Name == "Structural Foundation") _foundations.Add(e);
            //    else if (e.Category.Name == "Structural Column") _foundations.Add(e);
            //    else if (e.Category.Name == "Structural Framing") _foundations.Add(e);
            //    else if (e.Category.Name == "Floors") _foundations.Add(e);
            //}
        }
    }
    /// <summary>
    /// TIện ích cho Ghi chú triển khai 2D (annotation)
    /// </summary>
    public partial class AnnoUtils
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        // class constructor
        public AnnoUtils() { }
        public AnnoUtils(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;
        }
        /// <summary>
        /// Tìm ID Type của 1 Annotation symbol
        /// </summary>
        /// <param name="name"></param>
        /// <param name="annoSymbolId1"></param>
        /// <returns></returns>
        public ElementId GenericAnnoTypeId(string name, ElementId annoSymbolId1)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<ElementId> annoSymTypeIds = collector.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsElementType().ToElementIds();
            ElementId typeid = annoSymTypeIds.First();
            foreach (ElementId id in annoSymTypeIds)
            {
                if (doc.GetElement(id).Name == name)
                { typeid = id; break; }
            }
            MessageBox.Show(string.Format("Name of Family symbol: {0}\nType ID Cote Symbol: {1}", doc.GetElement(annoSymbolId1).Name, annoSymbolId1));
            return typeid;
        }
    }
    /// <summary>
    /// Bộ lọc Iselection cho TextNote
    /// </summary>
    public class TextNoteIselectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Text Notes")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    
    /// <summary>
    /// TIện ích cho Khung nhìn View
    /// </summary>
    public partial class ViewUtils
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        public static Color RED = new Color(255, 0, 0);
        public static Color GREEN = new Color(0, 128, 0);
        public static Color YELLOW = new Color(255, 255, 0);
        public static Color CYAN = new Color(0, 255, 255);
        public static Color MAGENTA = new Color(255, 0, 255);
        public static Color ORANGE = new Color(255, 128, 0);
        public static Color BLUE = new Color(0, 0, 255);
        public static Color PURPLE = new Color(128, 0, 255);

        public Color CUSTOM(byte R, byte G, byte B)
        {
            return new Color(R, G, B);
        }
        // class constructor
        public ViewUtils() { }
        public ViewUtils(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;
        }
        /*
        public List<Element> Override2ElementsColor(Color color1, Color color2, Autodesk.Revit.DB.View v)
        {
            OverrideGraphicSettings ogs1 = new OverrideGraphicSettings();
            OverrideGraphicSettings ogs2 = new OverrideGraphicSettings();
            ElementId fpatId = null;
            GetOGS_PatternID(color1, ref ogs1, ref fpatId);
            GetOGS_PatternID(color2, ref ogs2, ref fpatId);
            List<Element> listE1 = new List<Element>();
            List<Element> listE2 = new List<Element>();
            switch (Ribbon.SelectionMode)
            {
                case "Chọn Single Element":
                    {
                        Transaction t = new Transaction(doc, "VG 2 Element");
                        t.Start();
                        listE1.AddRange(OverrideSingleElementColor(color1, v, ogs1, fpatId));
                        listE2.AddRange(OverrideSingleElementColor(color2, v, ogs2, fpatId));
                        t.Commit();
                        break;
                    }
                case "Chọn Multi Elements":
                    {
                        MessageBox.Show("Không thể dùng cách chọn Multi Elements\nXin hãy chọn cách Single Element", "Thông báo"); break;
                    }
            }
            List<Element> listE = new List<Element>(); listE.AddRange(listE1); listE.AddRange(listE2);
            return listE;
        }
        
        public List<Element> OverrideElementColor(Color color, Autodesk.Revit.DB.View v)
        {
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ElementId fpatId = null;
            GetOGS_PatternID(color, ref ogs, ref fpatId);
            List<Element> listE = new List<Element>();
            switch (Ribbon.SelectionMode)
            {
                case "Chọn Single Element":
                    {
                        Transaction t = new Transaction(doc, "VG CAD 1 Element");
                        t.Start();
                        listE.AddRange(OverrideSingleElementColor(color, v, ogs, fpatId));
                        t.Commit();
                        break;
                    }
                case "Chọn Multi Elements":
                    {
                        Transaction t = new Transaction(doc, "VG CAD Multi Elements");
                        t.Start();
                        listE.AddRange(OverrideMultiElementsColor(color, v, ogs, fpatId));
                        t.Commit();
                        break;
                    }
            }

            return listE;
        }

        
        public void GetOGS_PatternID(Color color, ref OverrideGraphicSettings ogs, ref ElementId fpatId)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<ElementId> fillPatIDs = collector.OfClass(typeof(FillPatternElement)).WhereElementIsNotElementType().ToElementIds();

            foreach (ElementId id in fillPatIDs)
            {
                Element ee = doc.GetElement(id);
                if (ee.Name == "Solid fill")
                { fpatId = ee.Id; break; }
            }
            ogs.SetProjectionFillColor(color);
            ogs.SetProjectionFillPatternId(fpatId);
            ogs.SetCutFillColor(color);
            ogs.SetCutFillPatternId(fpatId);
            ogs.SetSurfaceTransparency(Ribbon.OEtransparency);
            ogs.SetProjectionLineColor(color);
            ogs.SetCutLineColor(color);
        }
        */
        /// <summary>
        /// Ghi đè (Override) Màu hiển thị 1 Đối tượng riêng lẻ
        /// </summary>
        /// <param name="color"></param>
        /// <param name="v"></param>
        /// <param name="ogs"></param>
        /// <param name="fpatId"></param>
        /// <returns></returns>
        public List<Element> OverrideSingleElementColor(Color color, Autodesk.Revit.DB.View v, OverrideGraphicSettings ogs, ElementId fpatId)
        {
            List<Element> listE = new List<Element>();
            Element e = doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Element"));
            //Transaction t = new Transaction(doc, "VG CAD 1 Element");
            //t.Start();
            v.SetElementOverrides(e.Id, ogs);
            //t.Commit();
            listE.Add(e);
            return listE;
        }
        /// <summary>
        /// Ghi đè (Override) Màu hiển thị 1 Đối tượng riêng lẻ
        /// </summary>
        /// <param name="e"></param>
        /// <param name="color"></param>
        /// <param name="v"></param>
        /// <param name="ogs"></param>
        /// <param name="fpatId"></param>
        /// <returns></returns>
        public List<Element> OverrideSingleElementColor(Element e, Color color, Autodesk.Revit.DB.View v, OverrideGraphicSettings ogs, ElementId fpatId)
        {
            List<Element> listE = new List<Element>();
            //Element e = doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Element"));
            //Transaction t = new Transaction(doc, "VG CAD 1 Element");
            //t.Start();
            v.SetElementOverrides(e.Id, ogs);
            //t.Commit();
            listE.Add(e);
            return listE;
        }
        /// <summary>
        /// Ghi đè (Override) Màu hiển thị Nhiều Đối tượng
        /// </summary>
        /// <param name="color"></param>
        /// <param name="v"></param>
        /// <param name="ogs"></param>
        /// <param name="fpatId"></param>
        /// <returns></returns>
        public List<Element> OverrideMultiElementsColor(Color color, Autodesk.Revit.DB.View v, OverrideGraphicSettings ogs, ElementId fpatId)
        {
            List<Element> listE = new List<Element>();
            IList<Reference> listREF = uiApp.ActiveUIDocument.Selection.PickObjects(ObjectType.Element, "Chọn Multi Elements");
            foreach (Reference rf in listREF) listE.Add(doc.GetElement(rf));
            foreach (Element e in listE)
            {
                //Transaction t = new Transaction(doc, "VG CAD Multi Elements");
                //t.Start();
                v.SetElementOverrides(e.Id, ogs);
                //t.Commit();
            }
            return listE;
        }
        /// <summary>
        /// Ghi đè (Override) Màu hiển thị Nhiều Đối tượng
        /// </summary>
        /// <param name="listE"></param>
        /// <param name="color"></param>
        /// <param name="v"></param>
        /// <param name="ogs"></param>
        /// <param name="fpatId"></param>
        /// <returns></returns>
        public List<Element> OverrideMultiElementsColor(List<Element> listE, Color color, Autodesk.Revit.DB.View v, OverrideGraphicSettings ogs, ElementId fpatId)
        {
            foreach (Element e in listE)
            {
                //Transaction t = new Transaction(doc, "VG CAD Multi Elements");
                //t.Start();
                v.SetElementOverrides(e.Id, ogs);
                //t.Commit();
            }
            return listE;
        }
        public void OverrideElement(Element elm)
        {
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ogs.SetProjectionLineColor(new Color(0, 255, 0));
            doc.ActiveView.SetElementOverrides(elm.Id, ogs);
        }
        /// <summary>
        /// Cài lại hiển thị 1 Đối tượng
        /// </summary>
        /// <param name="elm"></param>
        public void ResetOverrideElement(Element elm)
        {
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            doc.ActiveView.SetElementOverrides(elm.Id, ogs);
        }
        /// <summary>
        /// Cài lại hiển thị 1 Thể loại Category
        /// </summary>
        /// <param name="btcat"></param>
        public void ResetOE(BuiltInCategory btcat)
        {
            Autodesk.Revit.DB.View v = doc.ActiveView;
            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Element> listElem = collector.OfCategory(btcat).WhereElementIsNotElementType().ToElements().ToList();
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            foreach (Element e in listElem)
            {
                //Transaction t = new Transaction(doc, "Reset OE");
                //t.Start();
                v.SetElementOverrides(e.Id, ogs);
                //t.Commit();
            }
        }
        /// <summary>
        /// Lấy tất cả các View Thống kê
        /// </summary>
        /// <returns></returns>
        public List<ViewSchedule> GetAllViewSchedule()
        {
            List<ViewSchedule> listVS = new List<ViewSchedule>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> listE = collector.OfClass(typeof(ViewSchedule)).WhereElementIsNotElementType().ToElements().ToList();
            foreach (Element e in listE)
            {
                listVS.Add(e as ViewSchedule);
            }
            return listVS;
        }
        /// <summary>
        /// Xuất nhiều View thống kê
        /// </summary>
        /// <param name="path"></param>
        /// <param name="prefix"></param>
        /// <param name="suffix"></param>
        /// <param name="extension"></param>
        /// <param name="sbres"></param>
        /// <param name="startWith"></param>
        public void ExportBatchViewSchedules(string path, string prefix, string suffix, string extension, ref StringBuilder sbres, string startWith)
        {
            List<ViewSchedule> listVS_i = this.GetAllViewSchedule();
            List<ViewSchedule> listVS = new List<ViewSchedule>();
            foreach (ViewSchedule vs in listVS_i)
            {
                if (vs.Name.StartsWith(startWith))
                {
                    listVS.Add(vs);
                }
            }
            ViewScheduleExportOptions exportoption = new ViewScheduleExportOptions();
            try
            {
                foreach (ViewSchedule vs in listVS)
                {
                    string name = prefix + vs.Name + suffix + "." + extension;
                    sbres.AppendLine(path + @"\" + name);
                    vs.Export(path, name, exportoption);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }
    }
    /// <summary>
    /// Tiện ích Revit
    /// </summary>
    public class RevitUtil
    {

        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;

        public string projectNamePath;
        public string projectName;
        public string projectDirectory;

        public RevitUtil(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;

            projectNamePath = doc.PathName;
            int lastIdx = projectNamePath.LastIndexOf(@"\");
            projectDirectory = projectNamePath.Substring(0, lastIdx + 1);
            projectName = projectNamePath.Substring(projectDirectory.Length, projectNamePath.LastIndexOf(@".rvt") - lastIdx - 1);
        }
        /// <summary>
        /// Ghi tập tin Txt
        /// </summary>
        /// <param name="path"></param>
        /// <param name="lines"></param>
        public void WriteToTxt(string path, List<string> lines)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }
    }
    /// <summary>
    /// Tiện ích Tường Wall
    /// </summary>
    public class WallUtil
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        public WallUtil() { }
        /// <summary>
        /// Lấy tất cả Tường trong dự án
        /// </summary>
        /// <returns></returns>
        public List<Wall> getAllWallInstance()
        {
            List<Wall> listWalls = new List<Wall>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> elems = collector.OfClass(typeof(Wall)).WhereElementIsNotElementType().ToElements() as IList<Element>;
            foreach (Element e in elems) listWalls.Add(e as Wall);
            return listWalls;
        }
        /// <summary>
        /// Tìm List Wall giao với Room
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public List<Wall> findWallsIntersectRoom(Room rm)
        {
            List<Wall> listWalls = new List<Wall>();
            GeometryElement roomGeoElem = rm.get_Geometry(new Options());
            Solid roomSolid = null;
            foreach (GeometryObject roomGeoObj in roomGeoElem)
            {
                roomSolid = roomGeoObj as Solid;
                //if (roomSolid != null) break;
            }
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> elems = collector.OfClass(typeof(Wall))
                                            .WhereElementIsNotElementType().WherePasses(new ElementIntersectsSolidFilter(roomSolid))
                                            .ToElements() as IList<Element>;
            foreach (Element e in elems) listWalls.Add(e as Wall);
            return listWalls;
        }
    }
    /// <summary>
    /// Bộ lọc Iselection cho Wall
    /// </summary>
    public class WallIselectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Walls")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    /// <summary>
    /// Bộ lọc Iselection cho Cột Dầm Kết cấu
    /// </summary>
    public class StrucColumnFraming : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Structural Columns" || element.Category.Name == "Structural Framing")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    /// <summary>
    /// Bộ lọc Iselection cho Lưới trục Grid
    /// </summary
    public class GridSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Grids")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }

    /// <summary>
    /// Bộ lọc Iselection cho ImportInstance
    /// </summary
    public class ImportInstanceFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.GetType().ToString() == "Autodesk.Revit.DB.ImportInstance")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    /// <summary>
    /// Bộ lọc Iselection cho Cao độ Level
    /// </summary
    public class LevelSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Levels")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    /// <summary>
    /// Bộ lọc Iselection cho Dầm Beam
    /// </summary
    public class BeamSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Structural Framing")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    /// <summary>
    /// Tiện ích Sàn Floor
    /// </summary>
    public class FloorUtils
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        /// <summary>
        /// Tìm Curve tại 1 điểm trên Sàn
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public Curve FindCurveOfFloorAtPoint(XYZ pt, Floor floor)
        {
            Face tf = floor.GetGeometryObjectFromReference((HostObjectUtils.GetTopFaces(floor as HostObject))[0]) as Face;
            IList<CurveLoop> listcurveloop = tf.GetEdgesAsCurveLoops();
            CurveLoop curveloop = listcurveloop[0];
            double d = 1200 / 304.8;
            XYZ pt1 = new XYZ(pt.X, pt.Y, pt.Z + d);
            XYZ pt2 = new XYZ(pt.X, pt.Y, pt.Z - d);
            Curve c = Line.CreateBound(pt1, pt2) as Curve;
            Curve cAp = c;
            foreach (Curve cc in curveloop)
            {
                SetComparisonResult res = cc.Intersect(c);
                if (res != SetComparisonResult.Disjoint)
                {
                    cAp = cc; break;
                }
            }// MessageBox.Show(Math.Round(cAp.Length * 304.8).ToString());
            return cAp;
        }
        /// <summary>
        /// Tìm Sàn trên cùng tại 1 điểm
        /// </summary>
        /// <param name="listFloorAtPt"></param>
        /// <param name="pt"></param>
        /// <param name="dd"></param>
        /// <returns></returns>
        public Floor FindTopFloorAtPoint(List<Floor> listFloorAtPt, XYZ pt, double dd = 0)
        {
            int id = 0;
            for (int i = 0; i < listFloorAtPt.Count; i++)
            {
                double z = FloorTopFaceZatPoint(listFloorAtPt[i], pt);
                if (z > dd) { dd = z; id = i; }
            }
            Floor floor = listFloorAtPt[id];
            return floor;
        }
        /// <summary>
        /// Tìm Sàn trên cùng tài 1 điểm
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public List<Floor> FindFloorAtPoint(XYZ pt)
        {
            List<Floor> listFloor = new List<Floor>();
            IList<Element> ilistElement = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Floors)
                .WhereElementIsNotElementType().ToElements();
            string viewLevel = doc.ActiveView.LookupParameter("Associated Level").AsString();
            foreach (Element e in ilistElement)
            {
                Floor fl = e as Floor;
                string floorLevel = doc.GetElement(fl.LevelId).Name;
                if (floorLevel == viewLevel)
                    listFloor.Add(fl);
            }
            List<Floor> listFloorAtPt = new List<Floor>();
            double d = 1200 / 304.8;
            XYZ pt1 = new XYZ(pt.X, pt.Y, pt.Z + d);
            XYZ pt2 = new XYZ(pt.X, pt.Y, pt.Z - d);
            Curve c = Line.CreateBound(pt1, pt2) as Curve;
            foreach (Floor f in listFloor)
            {
                IList<Reference> topFaces = HostObjectUtils.GetTopFaces(f as HostObject);
                Face tf = f.GetGeometryObjectFromReference(topFaces[0]) as Face;
                SetComparisonResult res = tf.Intersect(c);
                if (res != SetComparisonResult.Disjoint) listFloorAtPt.Add(f);
            }//MessageBox.Show(string.Format("Số sàn tại Point = {0}",listFloorAtPt.Count));
            return listFloorAtPt;
        }
        /// <summary>
        /// Tìm Bề mặt trên cùng của Sàn
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        private double FloorTopFaceZ(Floor floor)
        {
            BoundingBoxXYZ bb = floor.get_BoundingBox(doc.ActiveView);
            XYZ max = bb.Max;
            XYZ min = bb.Min;
            XYZ point = new XYZ(max.X - min.X, max.Y - min.Y, max.Z - min.Z);
            FloorFace topFace = FloorFace.Top;
            XYZ topPt = floor.GetVerticalProjectionPoint(point, topFace);
            //FloorFace bottonFace = FloorFace.Bottom;
            //XYZ bottomPt = floor.GetVerticalProjectionPoint(point, bottonFace);
            //double doDaySanAtPoint = (topPt.Z - bottomPt.Z) * 304.8;//MessageBox.Show("Độ dày sàn tại điểm chọn là: " + Math.Round(doDaySanAtPoint, 2).ToString(), "Thông báo Đọc độ dày by Point");
            return Math.Round(topPt.Z * 304.8);
        }
        /// <summary>
        /// Tìm Bề mặt trên cùng của Sàn tại 1 điểm
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public double FloorTopFaceZatPoint(Floor floor, XYZ point)
        {
            FloorFace topFace = FloorFace.Top;
            XYZ topPt = floor.GetVerticalProjectionPoint(point, topFace);
            double dist = Math.Round(topPt.Z * 304.8);
            return dist;
        }
        /// <summary>
        /// Liệu điểm có nằm trong Khu vực của Sàn
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool PointInFloorArea(Floor floor, XYZ point)
        {
            bool flag = false;
            FloorFace topFace = FloorFace.Top;
            XYZ topPt = floor.GetVerticalProjectionPoint(point, topFace);
            double dist = Math.Round(topPt.Z * 304.8);

            HostObject ho = floor as HostObject;
            IList<Reference> listref = HostObjectUtils.GetTopFaces(ho);
            Face f = floor.GetGeometryObjectFromReference(listref[0]) as Face;

            IList<CurveLoop> listCL = f.GetEdgesAsCurveLoops();
            CurveLoop curveLoop = listCL[0];
            CurveLoop newCurveLoop = new CurveLoop();
            foreach (Curve c in curveLoop)
            {
                XYZ p0 = c.GetEndPoint(0);
                XYZ p1 = c.GetEndPoint(1);

                XYZ p00 = new XYZ(p0.X, p0.Y, 0);
                XYZ p11 = new XYZ(p1.X, p1.Y, 0);

                newCurveLoop.Append(Line.CreateBound(p00, p11));
            }
            CurveArray curvearray = new CurveArray();
            //f.IsInside()


            return flag;
        }
    }
    /// <summary>
    /// Bộ lọc Iselection cho Sàn
    /// </summary>
    public class FloorselectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Floors")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    /// <summary>
    /// Tiện ích Family
    /// </summary>
    public class FamilyUtils
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        /// <summary>
        /// Liệu Family có tồn tại trong dự án
        /// </summary>
        /// <param name="annoSymbolName"></param>
        /// <returns></returns>
        public bool FamilyExist(string annoSymbolName)
        {
            ICollection<ElementId> annoSymTypeIds = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsElementType().ToElementIds();
            bool familyExist = false;
            foreach (ElementId id in annoSymTypeIds) // TÌM FAMILY CÓ TỒN TẠI TRONG DỰ ÁN HAY KHÔNG
            {
                if (doc.GetElement(id).Name == annoSymbolName)
                {
                    familyExist = true;
                    break;
                }
            }
            return familyExist;
        }
        /// <summary>
        /// Tải Family vào dự án
        /// </summary>
        /// <param name="annoSymbolName"></param>
        /// <param name="familyFileName"></param>
        /// <param name="familyExist"></param>
        /// <returns></returns>
        public ElementId LoadFamily(string annoSymbolName, string familyFileName, bool familyExist)
        {
            ICollection<ElementId> annoSymTypeIds = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsElementType().ToElementIds();
            ElementId annoSymbolId = annoSymTypeIds.First();
            if (!familyExist)
            {
                Transaction t = new Transaction(doc, "Load Family"); t.Start();
                doc.LoadFamily(familyFileName); t.Commit();
                foreach (ElementId id in annoSymTypeIds)
                {
                    if (doc.GetElement(id).Name == annoSymbolName)
                    { annoSymbolId = id; break; }
                }
                return annoSymbolId;
            }
            else return null;
        }
    }
    /// <summary>
    /// Tiện ích Liên kết Link Import
    /// </summary>
    public class LinkUtils
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;

        public static IEnumerable<ExternalFileReference> GetLinkedFileReferences(Document _document)
        {
            var collector = new FilteredElementCollector(_document);
            var linkedElements = collector.OfClass(typeof(RevitLinkType)).Select(x => x.GetExternalFileReference()).ToList();
            return linkedElements;
        }
        public static IEnumerable<Document> GetLinkedDocuments(Document _document)
        {
            var linkedfiles = GetLinkedFileReferences(_document);
            var linkedFileNames = linkedfiles
              .Select(x => ModelPathUtils
               .ConvertModelPathToUserVisiblePath(
                 x.GetAbsolutePath())).ToList();

            return _document.Application.Documents
              .Cast<Document>()
              .Where(doc => linkedFileNames.Any(
               fileName => doc.PathName.Equals(fileName)));
        }
    }
    /// <summary>
    /// Tiện ích TAG
    /// </summary>
    public partial class TagUtil
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        // class constructor
        public TagUtil() { }
        public TagUtil(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;
        }
        #region FUNCTION
        public bool HasTag(Element e, List<IndependentTag> listTags)
        {
            bool flag = false;
            foreach (IndependentTag tag in listTags)
            {
                if (tag.TaggedLocalElementId == e.Id) { flag = true; break; }
            }
            return flag;
        }
        /// <summary>
        /// Lấy DS element không được TAG
        /// </summary>
        /// <param name="builtInCat"></param>
        /// <returns></returns>
        public List<Element> NonTaggedElements(BuiltInCategory builtInCat)
        {
            string elementTypeName = builtInCat.ToString();
            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Element> listElemTag = collector.OfCategory(BuiltInCategory.OST_StructuralFramingTags).WhereElementIsNotElementType().ToElements().ToList();
            FilteredElementCollector collector1 = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Element> listElemFraming = collector1.OfCategory(builtInCat).WhereElementIsNotElementType().ToElements().ToList();
            List<IndependentTag> listFrameTag = new List<IndependentTag>();
            foreach (Element e in listElemTag)
            {
                IndependentTag tag = e as IndependentTag;
                listFrameTag.Add(tag);
            }
            List<Element> listNonTag = new List<Element>();
            foreach (Element frame in listElemFraming)
            {
                if (!HasTag(frame, listFrameTag)) listNonTag.Add(frame);
            }
            MessageBox.Show(string.Format("Loại cấu kiện: {0}\n Số lượng cấu kiện không có Tag : {1}", elementTypeName, listNonTag.Count), "Thông báo kiểm tra");
            return listNonTag;
        }
        //HIDDEN
        /*
        public List<Element> NonTaggedElements(BuiltInCategory builtInCat, BuiltInCategory builtInTagCat, ref List<Element> listHasTag)
        {
            ViewPlan v = doc.ActiveView as ViewPlan;
            string elementTypeName = builtInCat.ToString();
            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Element> listElemTag = collector.OfCategory(builtInTagCat).WhereElementIsNotElementType().ToElements().ToList();
            FilteredElementCollector collector1 = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Element> listElem = collector1.OfCategory(builtInCat).WhereElementIsNotElementType().ToElements().ToList();
            List<IndependentTag> listTag = new List<IndependentTag>();
            foreach (Element e in listElemTag)
            {
                IndependentTag tag = e as IndependentTag;
                listTag.Add(tag);
            }
            List<Element> listNonTag = new List<Element>();
            foreach (Element ee in listElem)
            {
                if (!HasTag(ee, listTag)) listNonTag.Add(ee);
                else listHasTag.Add(ee);
            }
            ViewUtils vU = new ViewUtils(uiApp, doc, app);
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ElementId fpatId = null;
            vU.GetOGS_PatternID(ViewUtils.ORANGE, ref ogs, ref fpatId);
            List<Element> listE = vU.OverrideMultiElementsColor(listNonTag, ViewUtils.RED, v, ogs, fpatId);
            OverrideGraphicSettings ogs2 = new OverrideGraphicSettings();
            ElementId fpatId2 = null;
            vU.GetOGS_PatternID(ViewUtils.CYAN, ref ogs2, ref fpatId2);
            List<Element> listE2 = vU.OverrideMultiElementsColor(listHasTag, ViewUtils.YELLOW, v, ogs2, fpatId2);
            MessageBox.Show(string.Format("Loại cấu kiện: {0}\n Số lượng cấu kiện không có Tag : {1}\nSố lượng cấu kiện có Tag: {2}", elementTypeName, listNonTag.Count, listHasTag.Count), "Thông báo kiểm tra");
            return listNonTag;
        }*/
        /// <summary>
        /// Kiểm tra liệu Tag có gắn cho HOst
        /// </summary> 
        public void CheckTagHost()
        {
            GeoUtil geO = new GeoUtil(uiApp, doc, app);
            TagUtil tU = new TagUtil(uiApp, doc, app);
            ViewUtils vU = new ViewUtils(uiApp, doc, app);

            Autodesk.Revit.DB.View view = doc.ActiveView;

            //Collect all framing tag in active view;					
            FilteredElementCollector FramTagCollector = new FilteredElementCollector(doc, view.Id)
                .OfCategory(BuiltInCategory.OST_StructuralFramingTags)
                .WhereElementIsNotElementType();

            //Collect all framing in active view;					
            FilteredElementCollector FramCollector = new FilteredElementCollector(doc, view.Id)
                .OfCategory(BuiltInCategory.OST_StructuralFraming)
                .WhereElementIsNotElementType();

            List<Element> _FramTag = new List<Element>(FramTagCollector);
            List<XYZ> _pointList = new List<XYZ>();

            //Lay diem dat Tag
            int warnCount = 0;
            using (Transaction t = new Transaction(doc, "Get Point"))
            {
                t.Start();
                foreach (Element item in _FramTag)
                {
                    //Neu Leader Line = false -> Lay Tag Head Position
                    //Neu Leader Line = true -> Lay Tag Leader
                    XYZ P1 = null;
                    IndependentTag _tag = item as IndependentTag;


                    if (_tag.HasLeader)
                    {
                        if (_tag.LeaderEndCondition == LeaderEndCondition.Free)
                        {
                            P1 = _tag.LeaderEnd;
                        }
                        else
                        {
                            _tag.LeaderEndCondition = LeaderEndCondition.Free;

                            P1 = _tag.LeaderEnd;

                            _tag.LeaderEndCondition = LeaderEndCondition.Attached;
                        }
                    }
                    else
                    {
                        P1 = _tag.TagHeadPosition;
                    }
                    _pointList.Add(P1);

                    //Lay host cua tag
                    Element _host = _tag.GetTaggedLocalElement();

                    LocationCurve _curveLoc = _host.Location as LocationCurve;

                    if (_curveLoc == null) continue;

                    XYZ P2 = _curveLoc.Curve.GetEndPoint(0);
                    XYZ P3 = _curveLoc.Curve.GetEndPoint(1);

                    if (!geO.CheckIfFirstPointIsOutSide(new XYZ(P1.X, P1.Y, 0), new XYZ(P2.X, P2.Y, 0), new XYZ(P3.X, P3.Y, 0))) //Neu Tag nam ngoai dam -> warning tag
                    {
                        vU.OverrideElement(item);
                        warnCount++;
                    }
                    else //Neu tag nam trong dam -> Kiem tra khoang cach tu Tag den Host co phai la khoang cach gan nhat hay khong
                    {
                        double minDistance = _curveLoc.Curve.Distance(P1);
                        Element minElement = null;

                        foreach (Element fram in FramCollector)
                        {
                            LocationCurve _curve = fram.Location as LocationCurve;
                            if (_curve == null)
                            {
                                continue;
                            }

                            XYZ _P2 = _curve.Curve.GetEndPoint(0);
                            XYZ _P3 = _curve.Curve.GetEndPoint(1);

                            if (geO.CheckIfFirstPointIsOutSide(new XYZ(P1.X, P1.Y, 0), new XYZ(_P2.X, _P2.Y, 0), new XYZ(_P3.X, _P3.Y, 0))) //Neu Tag nam trong dam
                            {
                                double distance = _curve.Curve.Distance(P1);
                                if (distance <= minDistance)
                                {
                                    minDistance = distance;
                                    minElement = fram;
                                }
                            }
                        }

                        if (minElement.Id.ToString() != _host.Id.ToString())
                        {
                            vU.OverrideElement(item);
                            warnCount++;
                        }
                        else
                        {
                            vU.ResetOverrideElement(item);
                        }
                    }
                }
                t.Commit();
            }
            TaskDialog.Show("RV", warnCount.ToString() + " Tags need to be reviewed!");
        }
        #endregion
    }
    public class DimUtil
    {
        public UIApplication uiApp;
        public Autodesk.Revit.Creation.Application app;
        public Document doc;
        // class constructor
        public GeoUtil geO = new GeoUtil();
        public DimUtil() { }
        public DimUtil(UIApplication _uiApp, Document _doc, Autodesk.Revit.Creation.Application _app)
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;
        }
        /// <summary>
        /// Phương thức tạo Dim cho 1 Tường chọn
        /// </summary>
        /// <returns></returns>
        public Dimension Dim1Wall()
        {
            GeoUtil geoU = new GeoUtil(uiApp, doc, app);
            Dimension d;
            Wall w = (Wall)doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Wall").ElementId);//CHỌN WALL
            IList<Reference> ilistFacesExt = HostObjectUtils.GetSideFaces(w as HostObject, ShellLayerType.Exterior);//MessageBox.Show(ilistFacesExt.Count.ToString());
            IList<Reference> ilistFacesInt = HostObjectUtils.GetSideFaces(w as HostObject, ShellLayerType.Interior);//MessageBox.Show(ilistFacesExt.Count.ToString());
            ReferenceArray refs = new ReferenceArray();
            refs.Append(ilistFacesExt[0]);
            refs.Append(ilistFacesInt[0]);
            Line dimLine = geoU.LineByPickDetailLine();
            Autodesk.Revit.DB.View actView = doc.ActiveView;

            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                d = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return null;
            }
            t.Commit();
            return d;
        }
        public Dimension DimFromFaceReference(List<Face> listFace, Line dimLine)
        {
            Dimension dim;
            Autodesk.Revit.DB.View actView = doc.ActiveView;
            ReferenceArray refs = new ReferenceArray();
            foreach (Face f in listFace)
            {
                Reference rf = f.Reference;
                refs.Append(rf);
            }
            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                dim = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return null;
            }
            t.Commit();
            return dim;
        }
        /// <summary>
        /// Tạo DIMENSION cho Detail Line
        /// </summary>
        public void DimByDetailLine()
        {
            DetailCurve e = (DetailCurve)doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Line DIM").ElementId);
            Autodesk.Revit.DB.View actView = doc.ActiveView;
            Line dimLine = Line.CreateBound(e.GeometryCurve.GetEndPoint(0), e.GeometryCurve.GetEndPoint(1));
            ReferenceArray refs = new ReferenceArray();
            Reference ref0 = dimLine.GetEndPointReference(0);
            Reference ref1 = dimLine.GetEndPointReference(1);
            refs.Append(ref0);
            refs.Append(ref1);
            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                Dimension dim = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return;
            }
            t.Commit();
            MessageBox.Show("Finish creating DIM", "");
        }
        /// <summary>
        /// Tạo Dimension từ 3 Detail Line
        /// </summary>
        public void DimBy3DetailLine()
        {
            DetailCurve e = (DetailCurve)doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Line DIM").ElementId);//chọn Line
            DetailCurve e1 = (DetailCurve)doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Line Reference 1").ElementId);//chọn Line
            DetailCurve e2 = (DetailCurve)doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Line Reference 2").ElementId);//chọn Line   
            Autodesk.Revit.DB.View actView = doc.ActiveView;
            Line dimLine = Line.CreateBound(e.GeometryCurve.GetEndPoint(0), e.GeometryCurve.GetEndPoint(1));
            ReferenceArray refs = new ReferenceArray();
            Reference ref1 = e1.GeometryCurve.Reference;
            Reference ref2 = e2.GeometryCurve.Reference;
            refs.Append(ref1);
            refs.Append(ref2);
            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                Dimension dim = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return;
            }
            t.Commit();
            //MessageBox.Show("Finish creating DIM", "");
        }
        /// <summary>
        /// Phương thức xác định đối tượng có DIM chưa
        /// </summary>
        /// <param name="e"></param>
        /// <param name="listDim"></param>
        /// <returns></returns>
        public bool HasDimension(Element e, List<Dimension> listDim)
        {
            bool flagRes = false;
            int i = 0;
            foreach (Dimension dim in listDim)
            {
                ReferenceArray refarray = dim.References;
                foreach (Reference r in refarray)
                {
                    bool flag = r.ElementId == e.Id;//MessageBox.Show(flag.ToString());
                    if (flag) { i += 1; }
                }
            }
            if (i > 0) flagRes = true;
            return flagRes;
        }
        /// <summary>
        /// Phương thức xác định 2 Trục định vị gần Đối tượng nhất
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public List<Grid> GetNearGridsOfElement(XYZ pt)
        {
            List<Grid> lgr = new List<Grid>();
            FilteredElementCollector collect = new FilteredElementCollector(doc);
            IList<Element> elems = collect.OfCategory(BuiltInCategory.OST_Grids).WhereElementIsNotElementType().ToElements();
            foreach (Element e in elems)
            {
                lgr.Add(e as Grid);
            }
            List<double> ld = new List<double>();
            foreach (Grid g in lgr)
            {
                double d = g.Curve.Distance(pt);
                ld.Add(d);
            }
            int i = 0;
            for (int j = 0; j < ld.Count; j++)
            {
                if (ld[j] < ld[i]) i = j;
            }
            return lgr;
        }
        /// <summary>
        /// Phương thức chọn đối tượng và lấy point
        /// </summary>
        /// <param name="uiApp"></param>
        /// <param name="doc"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public XYZ PickElementForPoint(UIApplication uiApp, Document doc, ref Element elem)
        {
            Element e = doc.GetElement(uiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Pick an Element"));
            LocationPoint locPt = e.Location as LocationPoint;
            XYZ pt = locPt.Point;
            return pt;
        }
        // class funciton
        /// <summary>
        /// Phương thức tìm Tường của Room
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public List<Wall> FindWallsOfRoom(Document doc, Room r)
        {
            List<Wall> listWalls = new List<Wall>();

            IList<IList<BoundarySegment>> ListListBSegs = r.GetBoundarySegments(new SpatialElementBoundaryOptions()); //MessageBox.Show(ListListBSegs.Count.ToString());

            IList<BoundarySegment> ListBSegs = ListListBSegs.First(); //MessageBox.Show(ListBSegs.Count.ToString());

            string s = "";
            foreach (BoundarySegment bs in ListBSegs)
            {
                Wall w = (Wall)doc.GetElement(bs.ElementId);
                listWalls.Add(w);// Nếu là tường thì Add vô List
                s += w.Id.ToString() + "\n";
            }
            return listWalls;
        }
        /// <summary>
        /// Nhấp chuột để Pick Point
        /// </summary>
        /// <returns></returns>
        public XYZ pickForTestPoint()
        {
            XYZ pt = uiApp.ActiveUIDocument.Selection.PickPoint("Chọn 1 điểm bất kì để đặt DIM");

            Autodesk.Revit.DB.View actview = doc.ActiveView;

            string s = actview.Category.Name;
            ViewPlan viewplan = actview as ViewPlan;
            double viewCutPlaneoffset = 0;
            if (viewplan == null) return null;
            else
            {
                viewCutPlaneoffset = viewplan.GetViewRange().GetOffset(PlanViewPlane.CutPlane);
                //MessageBox.Show(viewoffset.ToString(), "");
            }
            XYZ pt2 = new XYZ(pt.X, pt.Y, pt.Z + viewCutPlaneoffset);
            return pt2;
        }
        public XYZ pickForTestPoint(bool hasPlane = true)
        {
            XYZ pt = uiApp.ActiveUIDocument.Selection.PickPoint("Chọn 1 điểm bất kì để đặt DIM");

            Autodesk.Revit.DB.View actview = doc.ActiveView;

            //SketchPlane spl = actview.SketchPlane;
            //FilteredElementCollector collector = new FilteredElementCollector(doc);
            //List<Element> listElem = collector.OfCategory(BuiltInCategory.OST_Grids).WhereElementIsNotElementType().ToElements().ToList();
            //List<Grid> listGridParal = new List<Grid>();
            //foreach (Element e in listElem)
            //{
            //    Grid gr = e as Grid;
            //    if (IsParallel(spl,gr))
            //    {
            //        listGridParal.Add(gr);
            //    }
            //}
            //Grid gridParal = listGridParal[0];
            Transaction tt = new Transaction(doc, "Set workplane");
            tt.Start();
            Plane plane = Plane.CreateByNormalAndOrigin(doc.ActiveView.ViewDirection, doc.ActiveView.Origin);
            SketchPlane sp = SketchPlane.Create(doc, plane);
            doc.ActiveView.SketchPlane = sp;
            //doc.ActiveView.ShowActiveWorkPlane();
            tt.Commit();

            //string s = actview.Category.Name;
            //ViewPlan viewplan = actview as ViewPlan;
            //double viewCutPlaneoffset = 0;
            //if (viewplan == null) return null;
            //else
            //{
            //    viewCutPlaneoffset = viewplan.GetViewRange().GetOffset(PlanViewPlane.CutPlane);
            //    //MessageBox.Show(viewoffset.ToString(), "");
            //}
            //XYZ pt2 = new XYZ(pt.X, pt.Y, pt.Z + viewCutPlaneoffset);
            XYZ pt2 = new XYZ(pt.X, pt.Y, pt.Z);
            return pt2;
        }

        /// <summary>
        /// Tạo Line Vertical từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Line _YdirectLineFromPoint(XYZ pt)
        {
            double d = 30;
            Line line = YlineByPoint(pt, d);
            return line;
        }
        public Line _ZdirectLineZFromPoint(XYZ pt)
        {
            double d = 30;
            Line line = ZlineByPoint(pt, d);
            return line;
        }
        /// <summary>
        /// Tạo Line Vertical từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line _YdirectLineFromPoint(XYZ pt, double d = 30)
        {
            Line line = YlineByPoint(pt, d);
            return line;
        }
        /// <summary>
        /// Tạo Line vertical từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line YlineByPoint(XYZ pt, double d = 30)
        {
            XYZ pt1 = new XYZ(pt.X, pt.Y + d, 0);
            XYZ pt2 = new XYZ(pt.X, pt.Y - d, 0);
            Line line = Line.CreateBound(pt1, pt2);
            return line;
        }
        public Line ZlineByPoint(XYZ pt, double d = 30)
        {
            XYZ pt1 = new XYZ(pt.X, pt.Y, pt.Z + d);
            XYZ pt2 = new XYZ(pt.X, pt.Y, pt.Z - d);
            Line line = Line.CreateBound(pt1, pt2);
            return line;
        }
        /// <summary>
        /// Tạo Line Horizontal từ point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line XlineByPoint(XYZ pt, double d = 30)
        {
            XYZ pt1 = new XYZ(pt.X + d, pt.Y, 0);
            XYZ pt2 = new XYZ(pt.X - d, pt.Y, 0);
            Line line = Line.CreateBound(pt1, pt2);
            return line;
        }
        /// <summary>
        /// Tạo Detail Line Vertical từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public DetailCurve VerticalDetailLineFromPoint(XYZ pt)
        {
            double d = 30;
            Line line = YlineByPoint(pt, d);
            DetailCurve dtLine = doc.Create.NewDetailCurve(doc.ActiveView, line as Curve);
            return dtLine;
        }

        /// <summary>
        /// Tạo Detail Line Horizontal từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public DetailCurve HorizontalDetailLineFromPoint(XYZ pt)
        {
            double d = 30;
            Line line = XlineByPoint(pt, d);
            DetailCurve dtLine = doc.Create.NewDetailCurve(doc.ActiveView, line as Curve);
            return dtLine;
        }
        /// <summary>
        /// Tạo Line Horizontal từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Line _HorizontalLineFromPoint(XYZ pt)
        {
            double d = 30;
            Line line = XlineByPoint(pt, d);
            return line;
        }
        /// <summary>
        /// Tạo Line Horizontal từ Point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line _HorizontalLineFromPoint(XYZ pt, double d = 30)
        {
            Line line = XlineByPoint(pt, d);
            return line;
        }
        /// <summary>
        /// Tạo List RefPlane từ List Line
        /// </summary>
        /// <param name="listLine"></param>
        /// <returns></returns>
        public List<ReferencePlane> CreateListRefPlaneFromListLines(List<Line> listLine)
        {
            List<ReferencePlane> listRefPlane = new List<ReferencePlane>();
            /*
            Transaction t = new Transaction(doc, "Room Ref planes");
            t.Start();
            foreach (Line line in listLine)
            {
                XYZ bubbleEnd = line.GetEndPoint(0);
                XYZ freeEnd = line.GetEndPoint(1);
                XYZ cutVec = new XYZ(0, 0, 1);
                ReferencePlane refPlane = doc.Create.NewReferencePlane(bubbleEnd, freeEnd, cutVec, doc.ActiveView);
                listRefPlane.Add(refPlane);
            }
            t.Commit();
            */
            foreach (Line line in listLine)
            {
                XYZ bubbleEnd = line.GetEndPoint(0);
                XYZ freeEnd = line.GetEndPoint(1);
                XYZ cutVec = new XYZ(0, 0, 1);
                ReferencePlane refPlane = doc.Create.NewReferencePlane(bubbleEnd, freeEnd, cutVec, doc.ActiveView);
                listRefPlane.Add(refPlane);
            }
            return listRefPlane;
        }
        public List<List<Grid>> GetParalleGrids(List<Grid> listGrid)
        {
            List<List<Grid>> listlistgrid = new List<List<Grid>>();
            List<List<Grid>> outputListList = new List<List<Grid>>();
            List<Grid> newlistGrid = new List<Grid>();
            List<Grid> listNonParal = new List<Grid>();
            for (int ii = 0; ii < listGrid.Count; ii++)
            {
                Grid gr = listGrid[ii];
                List<Grid> restList = new List<Grid>(listGrid);
                restList.RemoveAt(ii);
                bool flag = DetermineParallel(gr, restList);
                if (flag)
                {
                    newlistGrid.Add(gr);
                    continue;
                }
                else
                    listNonParal.Add(gr);
            }

            List<Grid> restList2 = new List<Grid>();
            while (newlistGrid.Count > 0)
            {
                List<Grid> output = new List<Grid>();
                Grid wa = newlistGrid.First();
                output.Add(wa);
                List<Grid> restList3 = new List<Grid>(newlistGrid);
                restList3.Remove(wa);
                foreach (Grid grr in restList3)
                {
                    bool flag = DetermineParallel(grr, output);
                    if (flag)
                    {
                        output.Add(grr);
                        if (newlistGrid.Count > 1)
                            newlistGrid.Remove(grr);
                        continue;
                    }
                }
                listlistgrid.Add(output);
                newlistGrid.Remove(wa);
            }
            return listlistgrid;
        }
        /// <summary>
        /// Phương thức tìm DS các DS WALL song song nhau
        /// </summary>
        /// <param name="listRefPlane"></param>
        /// <returns></returns>
        public List<List<Wall>> GetParallelWallList(List<Wall> listWall)
        {
            List<List<Wall>> wallListList = new List<List<Wall>>();
            List<List<ReferencePlane>> outputListList = new List<List<ReferencePlane>>();
            List<Wall> newlistWall = new List<Wall>();
            List<Wall> listNonParal = new List<Wall>();
            for (int ii = 0; ii < listWall.Count; ii++)
            {
                Wall ww = listWall[ii];
                List<Wall> restList = new List<Wall>(listWall);
                restList.RemoveAt(ii);
                bool flag = DetermineParallel(ww, restList);
                if (flag)
                {
                    newlistWall.Add(ww);
                    continue;
                }
                else
                    listNonParal.Add(ww);
            }

            List<Wall> restList2 = new List<Wall>();
            while (newlistWall.Count > 0)
            {
                List<Wall> output = new List<Wall>();
                Wall wa = newlistWall.First();
                output.Add(wa);
                List<Wall> restList3 = new List<Wall>(newlistWall);
                restList3.Remove(wa);
                foreach (Wall www in restList3)
                {
                    bool flag = DetermineParallel(www, output);
                    if (flag)
                    {
                        output.Add(www);
                        if (newlistWall.Count > 1)
                            newlistWall.Remove(www);
                        continue;
                    }
                }
                wallListList.Add(output);
                newlistWall.Remove(wa);
            }
            return wallListList;
        }
        /// <summary>
        /// Tìm (List List Ref Plane)Các Ref Plane Song song nhau
        /// </summary>
        /// <param name="listRefPlane"></param>
        /// <returns></returns>
        public List<List<ReferencePlane>> GetParallelRefPlanesList(List<ReferencePlane> listRefPlane)
        {
            List<List<ReferencePlane>> outputListList = new List<List<ReferencePlane>>();
            List<ReferencePlane> newlistRefPlane = new List<ReferencePlane>();
            List<ReferencePlane> listNonParal = new List<ReferencePlane>();
            for (int ii = 0; ii < listRefPlane.Count; ii++)
            {
                ReferencePlane rp = listRefPlane[ii];
                List<ReferencePlane> restList = new List<ReferencePlane>(listRefPlane);
                restList.RemoveAt(ii);
                bool flag = DetermineParallel(rp, restList);
                if (flag)
                {
                    newlistRefPlane.Add(rp);
                    continue;
                }
                else
                    listNonParal.Add(rp);
            }

            List<ReferencePlane> restList2 = new List<ReferencePlane>();
            while (newlistRefPlane.Count > 0)
            {
                List<ReferencePlane> output = new List<ReferencePlane>();
                ReferencePlane rp = newlistRefPlane.First();
                output.Add(rp);
                List<ReferencePlane> restList3 = new List<ReferencePlane>(newlistRefPlane);
                restList3.Remove(rp);
                foreach (ReferencePlane r in restList3)
                {
                    bool flag = DetermineParallel(r, output);
                    if (flag)
                    {
                        output.Add(r);
                        if (newlistRefPlane.Count > 1)
                            newlistRefPlane.Remove(r);
                        continue;
                    }
                }
                outputListList.Add(output);
                newlistRefPlane.Remove(rp);
            }
            return outputListList;
        }
        public bool DetermineParallel(ReferencePlane rp, List<Wall> listWall)
        {
            int i = 0;
            bool flagParal = false;
            foreach (Wall w in listWall)
            {
                Line line = rp.GetCurvesInView(DatumExtentType.ViewSpecific, doc.ActiveView).First() as Line;

                bool pal = IsParallel(line, w);
                if (pal)
                    i++;
            }
            if (i > 0)
            {
                flagParal = true;
                return flagParal;
            }
            return flagParal;
        }

        /// <summary>
        /// Xác định 1 REF PLANE có giao bất kì với DS REF PLANE
        /// </summary>
        /// <param name="rp"></param>
        /// <param name="newListRP"></param>
        /// <returns></returns>
        public bool DetermineParallel(ReferencePlane rp, List<ReferencePlane> newListRP)
        {
            int i = 0;
            bool flagParal = false;
            foreach (ReferencePlane r in newListRP)
            {
                bool pal = IsParallel(rp, r);
                if (pal)
                    i++;
            }
            if (i > 0)
            {
                flagParal = true;
                return flagParal;
            }
            return flagParal;
        }
        /// <summary>
        /// Xác định 1 WALL có giao bất kì với DS WALL
        /// </summary>
        /// <param name="w"></param>
        /// <param name="listWall"></param>
        /// <returns></returns>
        public bool DetermineParallel(Wall w, List<Wall> listWall)
        {
            int i = 0;
            bool flagParal = false;
            foreach (Wall ww in listWall)
            {
                bool pal = IsParallel(w, ww);
                if (pal)
                    i++;
            }
            if (i > 0)
            {
                flagParal = true;
                return flagParal;
            }
            return flagParal;
        }
        /// <summary>
        /// Xác định 1 LINE có giao bất kì với DS WALL
        /// </summary>
        /// <param name="line"></param>
        /// <param name="ListWall"></param>
        /// <returns></returns>
        public bool DetermineParallel(Line line, List<Wall> ListWall)
        {
            int i = 0;
            bool flagParal = false;
            foreach (Wall w in ListWall)
            {

                bool pal = IsParallel(line, w);
                if (pal)
                    i++;
            }
            if (i > 0)
            {
                flagParal = true;
                return flagParal;
            }
            return flagParal;
        }
        public bool DetermineParallel(Line line, List<Grid> LisGrid)
        {
            int i = 0;
            bool flagParal = false;
            foreach (Grid gr in LisGrid)
            {
                bool pal = IsParallel(line, gr);
                if (pal)
                    i++;
            }
            if (i > 0)
            {
                flagParal = true;
                return flagParal;
            }
            return flagParal;
        }
        /// <summary>
        /// Xác định 1 LINE có giao bất kì với DS REF PLANE
        /// </summary>
        /// <param name="line"></param>
        /// <param name="newListRP"></param>
        /// <returns></returns>
        public bool DetermineParallel(Line line, List<ReferencePlane> newListRP)//, ref List<ReferencePlane> restListRP
        {
            int i = 0;
            bool flagParal = false;
            foreach (ReferencePlane r in newListRP)
            {
                bool pal = IsParallel(line, r);
                if (pal)
                    i++;
            }
            if (i > 0)
            {
                flagParal = true;
                return flagParal;
            }
            return flagParal;
        }
        public bool DetermineParallel(Grid grid, List<Grid> listgrid)
        {
            bool flagParal = false;
            int i = 0;
            foreach (Grid gr in listgrid)
            {
                bool pal = IsParallel(grid, gr);
                if (pal)
                    i++;
            }
            if (i > 0)
            {
                flagParal = true;
                return flagParal;
            }
            return flagParal;
        }

        public bool IsParallel(Grid gr1, Grid gr2)
        {
            XYZ pt1 = (gr1.Curve as Line).Direction.Normalize();
            XYZ pt2 = (gr2.Curve as Line).Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == x2 || y1 == y2)//|| pt1.Z == pt2.Z
                return true;
            return false;
        }
        public bool IsParallel(SketchPlane skp, Grid gr)
        {

            XYZ pt1 = skp.GetPlane().Normal.Normalize();
            XYZ pt2 = (gr.Curve as Line).Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == y2 || y1 == x2)
                return true;
            return false;
        }
        /// <summary>
        /// Xác định 2 REF PLANE có song song nhau
        /// </summary>
        /// <param name="rp1"></param>
        /// <param name="rp2"></param>
        /// <returns></returns>
        public bool IsParallel(ReferencePlane rp1, ReferencePlane rp2)
        {
            XYZ pt1 = rp1.Direction.Normalize();
            XYZ pt2 = rp2.Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == x2 || y1 == y2)//|| pt1.Z == pt2.Z
                return true;
            return false;
        }
        /// <summary>
        /// Xác định LINE và REF PLANE có song song nhau
        /// </summary>
        /// <param name="line"></param>
        /// <param name="rp2"></param>
        /// <returns></returns>
        public bool IsParallel(Line line, ReferencePlane rp2)
        {
            XYZ pt1 = line.Direction.Normalize();
            XYZ pt2 = rp2.Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == x2 || y1 == y2)//|| pt1.Z == pt2.Z
                return true;
            return false;
        }
        /// <summary>
        /// Xác định LINE và WALL có song song nhau
        /// </summary>
        /// <param name="line"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool IsParallel(Line line, Wall w)
        {
            Curve c = (w.Location as LocationCurve).Curve;

            XYZ pt1 = line.Direction.Normalize();
            XYZ pt2 = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1)).Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == x2 || y1 == y2)//|| pt1.Z == pt2.Z
                return true;
            return false;
        }
        public bool IsParallel(Line line, Grid gr)
        {
            XYZ pt1 = line.Direction.Normalize();
            XYZ pt2 = (gr.Curve as Line).Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == x2 || y1 == y2)//|| pt1.Z == pt2.Z
                return true;
            return false;
        }
        /// <summary>
        /// Xác định 2 WALL có song song nhau
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns></returns>
        public bool IsParallel(Wall w1, Wall w2)
        {
            Curve c1 = (w1.Location as LocationCurve).Curve;
            Curve c2 = (w2.Location as LocationCurve).Curve;

            XYZ pt1 = Line.CreateBound(c1.GetEndPoint(0), c1.GetEndPoint(1)).Direction.Normalize();
            XYZ pt2 = Line.CreateBound(c2.GetEndPoint(0), c2.GetEndPoint(1)).Direction.Normalize();

            double x1 = Math.Round(Math.Abs(pt1.X), 3);
            double x2 = Math.Round(Math.Abs(pt2.X), 3);
            double y1 = Math.Round(Math.Abs(pt1.Y), 3);
            double y2 = Math.Round(Math.Abs(pt2.Y), 3);

            if (x1 == x2 || y1 == y2)//|| pt1.Z == pt2.Z
                return true;
            return false;
        }
        /// <summary>
        /// Phương thức tạo DIMENSION từ DS REF PLANE
        /// </summary>
        /// <param name="listRP"></param>
        /// <param name="dimLine"></param>
        /// <returns></returns>
        public Dimension DimRefPlanes(List<ReferencePlane> listRP, Line dimLine)
        {
            Dimension dim;
            Autodesk.Revit.DB.View actView = doc.ActiveView;
            ReferenceArray refs = new ReferenceArray();
            foreach (ReferencePlane rp in listRP)
            {
                Reference rf = rp.GetReference();
                refs.Append(rf);
            }
            /*
            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                dim = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return null;
            }
            t.Commit();
            */
            dim = doc.Create.NewDimension(actView, dimLine, refs);
            return dim;
        }
        /// <summary>
        /// Phương thức tạo Dimension cho Các reference plane của Room
        /// </summary>
        /// <param name="r"></param>
        /// <param name="lineV"></param>
        /// <param name="lineH"></param>
        /// <returns></returns>
        public List<Dimension> DimensionForReferencePlanesOfRoom(Room r, Line lineV, Line lineH)
        {
            List<Dimension> listDim = new List<Dimension>();
            // TẠO CÁC REF PLANE THEO BOUNDARY CỦA ROOM
            List<ReferencePlane> listRoomRefPlanes = CreateListRefPlaneFromRoom(r);

            // TÌM CÁC BỘ REF PLANE SONG SONG NHAU
            List<List<ReferencePlane>> listListRefPlanes = GetParallelRefPlanesList(listRoomRefPlanes); //MessageBox.Show(llRP.Count.ToString(), "");
            foreach (List<ReferencePlane> listRP in listListRefPlanes)
            {
                bool flagParallel = DetermineParallel(lineV, listRP); // nếu Line Dọc song song với bất 1 DS Ref plane ...
                if (flagParallel)
                { Dimension d = DimRefPlanes(listRP, lineH); listDim.Add(d); } // ... thì TẠO DIMENSION bằng Line Ngang
                //List<TextNote> listTx = PlaceTextNoteToRefPlane(listRP, listListRefPlanes.IndexOf(listRP)); // TẠO TEXT NOTE (THỨ TỰ) TẠI GIỮA REF PLANE
            }
            foreach (List<ReferencePlane> listRP in listListRefPlanes)// nếu Line Ngang song song với bất 1 DS Ref plane ...
            {
                bool flagParallel = DetermineParallel(lineH, listRP);
                if (flagParallel)
                { Dimension d = DimRefPlanes(listRP, lineV); listDim.Add(d); }// TẠO DIMENSION// ... thì TẠO DIMENSION bằng Line Dọc
                //List<TextNote> listTx = PlaceTextNoteToRefPlane(listRP, listListRefPlanes.IndexOf(listRP)); // TẠO TEXT NOTE (THỨ TỰ) TẠI GIỮA REF PLANE
            }
            return listDim;
        }

        public List<Dimension> DimensionElementLoctionInRoom(XYZ pt, Room r, ref List<ReferencePlane> listRP)
        {
            Line lineV = this._YdirectLineFromPoint(pt, 1);
            Line lineH = this._HorizontalLineFromPoint(pt, 1);
            List<Line> listLine = new List<Line>();
            listLine.Add(lineV);
            listLine.Add(lineH);

            listRP = this.CreateListRefPlaneFromListLines(listLine);// 2 REF PLANE TAI LOCKPOINT ĐỂ DIM
            ReferencePlane rpV = listRP.First();
            ReferencePlane rpH = listRP.Last();

            List<Dimension> listDim = new List<Dimension>();

            List<Wall> ListWall = FindWallsOfRoom(r);// TÌM CÁC TƯỜNG CỦA PHÒNG
            if (ListWall == null)
            {
                MessageBox.Show("Không tìm ra Tường của Room", "");
                return null;
            }
            //TÌM DS DS TƯỜNG và SONG SONG
            List<List<Wall>> ListListWall = GetParallelWallList(ListWall);
            if (ListListWall == null)
            {
                MessageBox.Show("Không có Các nhóm Tường song song ", "");
                return null;
            }
            foreach (List<Wall> listW in ListListWall)
            {
                bool flagParallel = DetermineParallel(rpV, listW); // nếu Line Dọc song song với bất 1 DS Ref plane ...
                List<Reference> lisRef = new List<Reference>();
                lisRef.Add(rpV.GetReference());
                if (flagParallel)
                {
                    List<Reference> lisWallRef = GetWallReferenceForDim(listW, lineV);
                    lisRef.AddRange(lisWallRef);
                    Dimension d = DimReferences(lisRef, lineH); listDim.Add(d);
                } // ... thì TẠO DIMENSION bằng Line Ngang(H)
            }
            foreach (List<Wall> listW in ListListWall)// nếu Line Ngang song song với bất 1 DS Ref plane ...
            {
                bool flagParallel = DetermineParallel(rpH, listW);
                List<Reference> lisRef = new List<Reference>();
                lisRef.Add(rpH.GetReference());
                if (flagParallel)
                {
                    List<Reference> lisWallRef = GetWallReferenceForDim(listW, lineH);
                    lisRef.AddRange(lisWallRef);
                    Dimension d = DimReferences(lisRef, lineV); listDim.Add(d);
                }// TẠO DIMENSION// ... thì TẠO DIMENSION bằng Line Dọc (V)
            }
            return listDim;
        }
        /// <summary>
        /// Phương thức tạo DIM cho Tường của Room
        /// </summary>
        /// <param name="r"></param>
        /// <param name="lineV"></param>
        /// <param name="lineH"></param>
        /// <returns></returns>
        public List<Dimension> DimensionForWallsOfRoom(Room r, Line lineV, Line lineH)
        {
            List<Dimension> listDim = new List<Dimension>();
            // TÌM CÁC TƯỜNG CỦA PHÒNG
            List<Wall> ListWall = FindWallsOfRoom(r);
            if (ListWall == null)
            {
                MessageBox.Show("Không tìm ra Tường của Room", "");
                return null;
            }
            //TÌM DS DS TƯỜNG SONG SONG
            List<List<Wall>> ListListWall = GetParallelWallList(ListWall);
            if (ListListWall == null)
            {
                MessageBox.Show("Không có Các nhóm Tường song song ", "");
                return null;
            }

            foreach (List<Wall> listW in ListListWall)
            {
                bool flagParallel = DetermineParallel(lineV, listW); // nếu Line Dọc song song với bất 1 DS Ref plane ...
                if (flagParallel)
                {
                    Transaction tt = new Transaction(doc, "Dimension");
                    tt.Start();
                    try { Dimension d = DimWalls(listW, lineH); listDim.Add(d); }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "");
                        return null;
                    }
                    tt.Commit();

                } // ... thì TẠO DIMENSION bằng Line Ngang
            }
            foreach (List<Wall> listW in ListListWall)// nếu Line Ngang song song với bất 1 DS Ref plane ...
            {
                bool flagParallel = DetermineParallel(lineH, listW);
                if (flagParallel)
                {
                    Transaction tt = new Transaction(doc, "Dimension");
                    tt.Start();
                    try
                    {
                        Dimension d = DimWalls(listW, lineV); listDim.Add(d);
                    }// TẠO DIMENSION// ... thì TẠO DIMENSION bằng Line Dọc
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "");
                        return null;
                    }
                    tt.Commit();
                }
            }

            return listDim;
        }
        /// <summary>
        /// Phương thức tìm SEPERATE LINES của ROOM
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public List<ReferencePlane> ReferencePlanesFromRoomSeperateLines(Room r)
        {
            List<ReferencePlane> listRP = new List<ReferencePlane>();

            IList<IList<BoundarySegment>> ListListBSegs = r.GetBoundarySegments(new SpatialElementBoundaryOptions()); //MessageBox.Show(ListListBSegs.Count.ToString());

            IList<BoundarySegment> ListBSegs = ListListBSegs.First(); //MessageBox.Show(ListBSegs.Count.ToString());

            List<Line> ll = new List<Line>();
            foreach (BoundarySegment bs in ListBSegs)
            {
                Element e = doc.GetElement(bs.ElementId);
                if (e.Category.Name == "<Room Separation>")
                {
                    ModelLine ml = e as ModelLine;
                    Curve c = ml.GeometryCurve;
                    ll.Add(c as Line);
                    listRP = CreateListRefPlaneFromListLines(ll);
                }
            }
            return listRP;
        }
        /// <summary>
        /// Phương thức tìm WALL của ROOM
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public List<Wall> FindWallsOfRoom(Room r)
        {
            List<Wall> listWalls = new List<Wall>();

            IList<IList<BoundarySegment>> ListListBSegs = r.GetBoundarySegments(new SpatialElementBoundaryOptions()); //MessageBox.Show(ListListBSegs.Count.ToString());

            IList<BoundarySegment> ListBSegs = ListListBSegs.First(); //MessageBox.Show(ListBSegs.Count.ToString());
            foreach (BoundarySegment bs in ListBSegs)
            {
                Element e = doc.GetElement(bs.ElementId);
                if (e.Category.Name == "Walls")
                {
                    Wall w = e as Wall;
                    listWalls.Add(w);// Nếu là tường thì Add vô List
                }

            }
            return listWalls;
        }

        /// <summary>
        /// Phương thức tạo DIMESION cho DS WALL
        /// </summary>
        /// <param name="listWall"></param>
        /// <param name="dimLine"></param>
        /// <returns></returns>
        public Dimension DimWalls(List<Wall> listWall, Line dimLine)
        {
            Dimension d;
            ReferenceArray refs = new ReferenceArray();
            foreach (Wall ww in listWall)
            {
                IList<Reference> ilistFacesExt = HostObjectUtils.GetSideFaces(ww as HostObject, ShellLayerType.Exterior);//MessageBox.Show(ilistFacesExt.Count.ToString());
                IList<Reference> ilistFacesInt = HostObjectUtils.GetSideFaces(ww as HostObject, ShellLayerType.Interior);//MessageBox.Show(ilistFacesExt.Count.ToString());
                refs.Append(ilistFacesExt[0]);
                refs.Append(ilistFacesInt[0]);
            }
            Autodesk.Revit.DB.View actView = doc.ActiveView;
            /*
            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                d = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return null;
            }
            t.Commit();
            */
            d = doc.Create.NewDimension(actView, dimLine, refs);
            return d;
        }
        public Dimension DimGrids(List<Grid> listGrid, Line dimLine)
        {
            Dimension d;
            ReferenceArray refs = new ReferenceArray();
            foreach (Grid gr in listGrid)
            {
                //refs.Append(gr.GetCurvesInView(DatumExtentType.ViewSpecific,doc.ActiveView)[0].Reference);
                Reference rf = Reference.ParseFromStableRepresentation(doc, gr.UniqueId.ToString());
                refs.Append(rf);
            }
            Autodesk.Revit.DB.View actView = doc.ActiveView;

            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                d = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return null;
            }
            t.Commit();
            return d;
        }
        public Dimension DimLevels(List<Level> listLevel, Line dimLine)
        {
            Dimension d;
            ReferenceArray refs = new ReferenceArray();
            foreach (Level lv in listLevel)
            {
                Reference rf = Reference.ParseFromStableRepresentation(doc, lv.UniqueId.ToString());
                refs.Append(rf);
            }
            Autodesk.Revit.DB.View actView = doc.ActiveView;
            Transaction t = new Transaction(doc, "Dimension");
            t.Start();
            try
            {
                d = doc.Create.NewDimension(actView, dimLine, refs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "");
                return null;
            }
            t.Commit();
            return d;
        }
        public Dimension DimReferences(List<Reference> listRef, Line dimLine)
        {
            Dimension d;
            ReferenceArray refs = new ReferenceArray();
            foreach (Reference rf in listRef)
            {
                refs.Append(rf);
            }
            Autodesk.Revit.DB.View actView = doc.ActiveView;

            //Transaction t = new Transaction(doc, "Dimension");
            //t.Start();
            //try
            //{
            //    d = doc.Create.NewDimension(actView, dimLine, refs);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString(), "");
            //    return null;
            //}
            //t.Commit();
            d = doc.Create.NewDimension(actView, dimLine, refs);
            return d;
        }
        public List<Reference> GetWallReferenceForDim(List<Wall> listWall, Line dimLine)
        {
            List<Reference> listRef = new List<Reference>();
            foreach (Wall ww in listWall)
            {
                IList<Reference> ilistFacesExt = HostObjectUtils.GetSideFaces(ww as HostObject, ShellLayerType.Exterior);//MessageBox.Show(ilistFacesExt.Count.ToString());
                IList<Reference> ilistFacesInt = HostObjectUtils.GetSideFaces(ww as HostObject, ShellLayerType.Interior);//MessageBox.Show(ilistFacesExt.Count.ToString());
                listRef.Add(ilistFacesExt[0]);
                listRef.Add(ilistFacesInt[0]);
            }

            return listRef;
        }
        /// <summary>
        /// Tạo các DS REF PLANE từ ROOM
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public List<ReferencePlane> CreateListRefPlaneFromRoom(Room rm)
        {
            IList<IList<BoundarySegment>> IIlistBoundSeg = rm.GetBoundarySegments(new SpatialElementBoundaryOptions());

            Solid solid = getSolidFromRoom(rm);
            //double d = sl.Volume * 0.3048* 0.3048* 0.3048;
            //MessageBox.Show(d.ToString(),"");

            FaceArray faceAr = solid.Faces;
            Face bottomFace = getBottomFaceOfRoom(rm);
            IList<CurveLoop> listCL = bottomFace.GetEdgesAsCurveLoops();
            //EdgeArrayArray edgeArAr = bottomFace.EdgeLoops;

            List<Line> listLine = GetLinesOfListCurveLoop(listCL);
            List<ReferencePlane> listRefPlane = CreateListRefPlaneFromListLines(listLine);

            return listRefPlane;
        }
        /// <summary>
        /// Lấy SOLID từ ROOM
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public Solid getSolidFromRoom(Room rm)
        {
            GeometryElement roomGeoElem = rm.get_Geometry(new Options());
            Solid roomSolid = null;
            foreach (GeometryObject roomGeoObj in roomGeoElem)
            {
                roomSolid = roomGeoObj as Solid;
                if (roomSolid != null) break;
            }
            return roomSolid;
        }
        /// <summary>
        /// Lấy SOLID từ WALL
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        private Solid getSolidFromWall(Wall rm)
        {
            GeometryElement roomGeoElem = rm.get_Geometry(new Options());
            Solid roomSolid = null;
            foreach (GeometryObject roomGeoObj in roomGeoElem)
            {
                roomSolid = roomGeoObj as Solid;
                if (roomSolid != null) break;
            }
            return roomSolid;
        }
        /// <summary>
        /// Lấy Mặt dưới Bottom Face của Room
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public Face getBottomFaceOfRoom(Room rm)
        {
            Solid solid = getSolidFromRoom(rm);
            //double d = sl.Volume * 0.3048* 0.3048* 0.3048;
            //MessageBox.Show(d.ToString(),"");

            FaceArray faceAr = solid.Faces;
            Face face = null;
            foreach (Face f in faceAr)
            {
                UV uv = new UV();
                XYZ nPt = f.ComputeNormal(uv).Normalize();
                if (nPt.Z == -1)//|| nPt.Z==1
                {
                    face = f;
                    break;
                }
            }
            return face;
        }
        /// <summary>
        /// Lấy DS LINE từ DS CURVE LOOP
        /// </summary>
        /// <param name="listCL"></param>
        /// <returns></returns>
        public List<Line> GetLinesOfListCurveLoop(IList<CurveLoop> listCL)
        {
            List<Line> listLine = new List<Line>();
            foreach (CurveLoop cl in listCL)
            {
                foreach (Curve c in cl)
                {
                    XYZ pt0 = c.GetEndPoint(0);
                    XYZ pt1 = c.GetEndPoint(1);
                    Line line = Line.CreateBound(pt0, pt1);
                    listLine.Add(line);
                }
            }
            return listLine;
        }
        /// <summary>
        /// Tìm Room chứa điểm
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="listRoom"></param>
        /// <returns></returns>
        public Room findRoomContainPoint(XYZ pt, List<Room> listRoom)
        {
            foreach (Room r in listRoom)
            {
                if (r.IsPointInRoom(pt)) return r;
            }
            return null;
        }
        /// <summary>
        /// Lấy tất cả Room , trừ room Not in placed
        /// </summary>
        /// <returns></returns>
        public List<Room> getAllRooms()
        {
            List<Room> rooms = new List<Room>();
            FilteredElementCollector filter = new FilteredElementCollector(doc);
            //IList<Element> elems = filter.OfClass(typeof(Room)).WhereElementIsNotElementType().ToElements() as IList<Element>;
            IList<Element> elems = filter.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements() as IList<Element>;

            foreach (Element e in elems) rooms.Add(e as Room);

            //StringBuilder s = new StringBuilder();
            //foreach (Room r in rooms) s.Append(r.Area.ToString() + "\n");

            List<Room> newrooms = new List<Room>();
            foreach (Room r in rooms)
            {
                if (r.Area != 0) newrooms.Add(r);
            }
            //showDialog(newrooms.Count.ToString());
            return rooms;
        }
        /*
        public List<Element> NonDimElements(BuiltInCategory bcate)
        {
            string elementTypeName = bcate.ToString();//.Substring(bcate.ToString().IndexOf("_")+1);
            ViewUtils vU = new ViewUtils(uiApp, doc, app);
            //TÌM CÁC elemnent VISIBLE TRONG VIEW PLAN
            FilteredElementCollector collector1 = new FilteredElementCollector(doc, doc.ActiveView.Id);
            ViewPlan v = doc.ActiveView as ViewPlan;
            List<Element> listElement = new List<Element>();
            foreach (Element eee in collector1.OfCategory(bcate).WhereElementIsNotElementType().ToElements())
            {
                listElement.Add(eee);
            }
            //TÌM CÁC DIM THUOC LEVEL
            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Dimension> listDim = new List<Dimension>();
            IList<Element> ilistElem = collector.OfClass(typeof(Dimension)).WhereElementIsNotElementType().ToElements();
            foreach (Element ee in collector.OfClass(typeof(Dimension)).WhereElementIsNotElementType().ToElements())
            {
                //string dimViewName = doc.GetElement(ee.OwnerViewId).Name;
                listDim.Add(ee as Dimension);
            }
            //LỌC CÁC element KHÔNG CÓ DIM
            List<Element> listNonDim = new List<Element>();
            foreach (Element eeee in listElement)
            {
                bool flag = HasDimension(eeee, listDim);
                if (!flag) { listNonDim.Add(eeee); }
            }
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ElementId fpatId = null;
            vU.GetOGS_PatternID(ViewUtils.RED, ref ogs, ref fpatId);
            List<Element> listE = vU.OverrideMultiElementsColor(listNonDim, ViewUtils.RED, v, ogs, fpatId);
            MessageBox.Show(string.Format("Loại cấu kiện: {0}\n Số lượng cấu kiện không có Dimension : {1}", elementTypeName, listE.Count), "Thông báo kiểm tra");
            return listE;
        }
        public List<Element> NonDimElements(BuiltInCategory bcate, ref List<Element> listHasDim)
        {
            string elementTypeName = bcate.ToString();//.Substring(bcate.ToString().IndexOf("_")+1);

            //TÌM CÁC elemnent VISIBLE TRONG VIEW PLAN
            FilteredElementCollector collector1 = new FilteredElementCollector(doc, doc.ActiveView.Id);
            ViewPlan v = doc.ActiveView as ViewPlan;
            List<Element> listElement = new List<Element>();
            foreach (Element eee in collector1.OfCategory(bcate).WhereElementIsNotElementType().ToElements())
            {
                listElement.Add(eee);
            }
            //TÌM CÁC DIM THUOC LEVEL
            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Dimension> listDim = new List<Dimension>();
            IList<Element> ilistElem = collector.OfClass(typeof(Dimension)).WhereElementIsNotElementType().ToElements();
            foreach (Element ee in collector.OfClass(typeof(Dimension)).WhereElementIsNotElementType().ToElements())
            {
                //string dimViewName = doc.GetElement(ee.OwnerViewId).Name;
                listDim.Add(ee as Dimension);
            }
            //LỌC CÁC element KHÔNG CÓ DIM
            List<Element> listNonDim = new List<Element>();
            foreach (Element eeee in listElement)
            {
                bool flag = HasDimension(eeee, listDim);
                if (!flag) { listNonDim.Add(eeee); }
                else listHasDim.Add(eeee);
            }

            ViewUtils vU = new ViewUtils(uiApp, doc, app);
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ElementId fpatId = null;
            vU.GetOGS_PatternID(ViewUtils.RED, ref ogs, ref fpatId);
            List<Element> listE = vU.OverrideMultiElementsColor(listNonDim, ViewUtils.RED, v, ogs, fpatId);

            OverrideGraphicSettings ogs2 = new OverrideGraphicSettings();
            ElementId fpatId2 = null;
            vU.GetOGS_PatternID(ViewUtils.YELLOW, ref ogs2, ref fpatId2);
            List<Element> listE2 = vU.OverrideMultiElementsColor(listHasDim, ViewUtils.YELLOW, v, ogs2, fpatId2);

            MessageBox.Show(string.Format("Loại cấu kiện: {0}\n Số lượng cấu kiện không có Dimension : {1}\nSố lượng cấu kiện có Dimension : {2}", elementTypeName, listE.Count, listHasDim.Count), "Thông báo kiểm tra");

            return listE;
        }*/
        public List<Dimension> AllActiveViewDimensions()
        {
            FilteredElementCollector dimCollector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Element> listDimElem = dimCollector.OfCategory(BuiltInCategory.OST_Dimensions).WhereElementIsNotElementType().ToElements().ToList();
            List<Dimension> listDim = new List<Dimension>();
            foreach (Element e in listDimElem)
            {
                Dimension d = e as Dimension;
                listDim.Add(d);
            }
            return listDim;
        }
        public List<Dimension> SelectDimensions()
        {
            FilteredElementCollector dimCollector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            List<Element> listDimElem = dimCollector.OfCategory(BuiltInCategory.OST_Dimensions).WhereElementIsNotElementType().ToElements().ToList();
            List<Dimension> listDim = new List<Dimension>();
            foreach (Element e in listDimElem)
            {
                Dimension d = e as Dimension;
                listDim.Add(d);
            }
            return listDim;
        }
        public void MoveDim(Dimension dim, double doubleMM)
        {
            double viewScale = double.Parse(doc.ActiveView.Scale.ToString());//MessageBox.Show(string.Format("View Scale = {0}",viewScale));
            Curve dimCurve = dim.Curve;
            Line dimLine = dimCurve as Line;

            XYZ dimDirect = dimLine.Direction;//.Normalize();
            double textSize = dim.DimensionType.LookupParameter("Text Size").AsDouble();
            double textoffset = dim.DimensionType.LookupParameter("Text Offset").AsDouble();
            double textBoxSize = textSize * (397.0 / 250.0);

            int dimSegmentNo = dim.NumberOfSegments;
            bool IsMultiDim = dimSegmentNo == 0;
            XYZ normal = geO.normalDirect(dimDirect);
            XYZ moveXYZ = normal.Multiply(doubleMM / 304.8);

            Transaction t = new Transaction(doc);
            t.Start("Move DIM");
            ElementTransformUtils.MoveElement(doc, dim.Id, moveXYZ);
            t.Commit();
        }

        public List<Line> BorderingDimText(Dimension dim)
        {
            List<Line> listLine = new List<Line>();
            double viewScale = double.Parse(doc.ActiveView.Scale.ToString());//MessageBox.Show(string.Format("View Scale = {0}",viewScale));
            DimIselectionFilter dimfilter = new DimIselectionFilter();
            Curve dimCurve = dim.Curve;
            Line dimLine = dimCurve as Line;

            XYZ dimDirect = dimLine.Direction;//.Normalize();
            double textSize = dim.DimensionType.LookupParameter("Text Size").AsDouble();
            double textoffset = dim.DimensionType.LookupParameter("Text Offset").AsDouble();
            double textBoxSize = textSize * (397.0 / 250.0);
            double angle = dimDirect.AngleTo(new XYZ(1, 0, 0));

            int dimSegmentNo = dim.NumberOfSegments;
            bool IsMultiDim = dimSegmentNo == 0;
            switch (IsMultiDim)
            {
                case (true):
                    {
                        string dimStringValue = dim.ValueString;
                        int dimStringCount = dimStringValue.Length;
                        XYZ dimTextPost = dim.TextPosition;
                        double textWidth = (dimStringCount / 4) * 857.7857 / (viewScale * 304.8);
                        //Line lineVector = Line.CreateBound(new XYZ(),dimDirect);                        
                        //bool isVH = Math.Abs(angle) == 0;   //%(Math.PI/2)
                        listLine.AddRange(geO.CrossLinesFromPoint(textoffset * viewScale, textWidth * viewScale, textBoxSize * viewScale, dimTextPost, dimDirect, angle));
                        //Transaction t = new Transaction(doc);
                        //t.Start("Detail Line");
                        //foreach (Line line in listLine)
                        //{
                        //    DetailCurve dtLine = doc.Create.NewDetailCurve(doc.ActiveView, line as Curve);
                        //}
                        //t.Commit();
                        break;
                    }
                case (false):
                    {

                        DimensionSegmentArray dimSegAr = dim.Segments;
                        foreach (DimensionSegment ds in dimSegAr)
                        {
                            string dimStringValue = ds.ValueString;
                            int dimStringCount = dimStringValue.Length;
                            XYZ dimTextPost = ds.TextPosition;
                            double textWidth = (dimStringCount / 4) * 857.7857 / (viewScale * 304.8);
                            listLine.AddRange(geO.CrossLinesFromPoint(textoffset * viewScale, textWidth * viewScale, textBoxSize * viewScale, dimTextPost, dimDirect, angle));
                            //Transaction t = new Transaction(doc);
                            //t.Start("Detail Line");
                            //foreach (Line line in listLine)
                            //{
                            //    DetailCurve dtLine = doc.Create.NewDetailCurve(doc.ActiveView, line as Curve);
                            //}
                            //t.Commit();
                        }
                        break;
                    }
            }
            return listLine;
        }
        public List<string> GetAllDimensionTypes(ref List<DimensionType> listDimType)
        {
            List<string> ls = new List<string>();
            FilteredElementCollector collect = new FilteredElementCollector(doc);
            IList<Element> elems = collect.OfClass(typeof(DimensionType)).ToElements();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            foreach (Element e in elems)
            {
                DimensionType dimType = e as DimensionType;

                if (dimType.StyleType == DimensionStyleType.Linear)// || dimType.StyleType == DimensionStyleType.LinearFixed)
                {
                    if (dimType.LookupParameter("Dimension String Type") != null)
                    {
                        listDimType.Add(dimType);
                        ls.Add(dimType.Name);
                        sb.AppendLine(dimType.Name);
                    }
                }
            }
            MessageBox.Show(sb.ToString());
            return ls;
        }

    }//class

    /// <summary>
    /// Bộ lọc Iselection cho Dimension
    /// </summary>
    public class DimIselectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category.Name == "Dimensions")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    /// <summary>
    /// Tạo Class Thiết lập Dimension
    /// </summary>
    [Serializable]
    public class DimSetting
    {
        public DimSetting() { }
        public DimSetting(DimensionType _dimensionType, XYZ _dimensionDirection)
        {
            dimType = _dimensionType;
            dimDirection = _dimensionDirection;
        }
        public DimensionType dimType
        {
            get; set;
        }
        public XYZ dimDirection
        {
            get; set;
        }
        /*
        internal Stream SerializeToMS(DimConfigureSetting dcfg)
        {
            DimConfigureSetting objDcfg = new DimConfigureSetting();
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, objDcfg);
            return ms;
        }
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            // Do some work while serializing object
        }
        [OnSerialized]
        private void OnSerialized(StreamingContext context)
        {
            // Do some work after serialized object
        }
        */
    }
    /// <summary>
    /// Tạo class Quản lí DIM Setting
    /// </summary>
    public class DimSettingManager
    {
        private static string programName = Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
        public UIApplication uiApp;
        public Document doc;
        public Autodesk.Revit.Creation.Application app;

        public static DimSettingManager dimSettingManager;
        public static DimSetting dimSetting;
        private static BinaryFormatter formatter;

        public string projectNamePath;
        public string projectName;

        public const string DatFileName = "DimSettings.dat";
        public static string dimSettingNamePath;

        public DimSettingManager() { }
        public DimSettingManager(Document _doc)
        {
            doc = _doc;
            dimSetting = new DimSetting();
            dimSetting.dimType = doc.GetElement(doc.GetDefaultElementTypeId(ElementTypeGroup.LinearDimensionType)) as DimensionType;
            dimSetting.dimDirection = XYZ.BasisX;
            formatter = new BinaryFormatter();
            projectNamePath = doc.PathName;
            int lastIdx = projectNamePath.LastIndexOf(@"\");
            string directory = projectNamePath.Substring(0, lastIdx + 1);
            projectName = projectNamePath.Substring(directory.Length, projectNamePath.LastIndexOf(@".rvt") - lastIdx - 1);
            dimSettingNamePath = directory + projectName + "_" + DatFileName;
        }

        public static DimSettingManager Instance()
        {
            if (dimSettingManager == null) { dimSettingManager = new DimSettingManager(); }
            return dimSettingManager;
        }
        public void ChangeDimSetting(DimensionType newDimType, XYZ newDirection)
        {
            dimSetting.dimType = newDimType;
            dimSetting.dimDirection = newDirection;
        }
        public static void Save()
        {
            // Gain code access to the file that we are going to write to
            try
            {// Create a FileStream that will write data to file.
                FileStream writerFileStream = new FileStream(dimSettingNamePath, FileMode.Create, FileAccess.Write);// Save our dictionary of friends to file
                formatter.Serialize(writerFileStream, dimSetting);// Close the writerFileStream when we are done.
                writerFileStream.Close();
            }
            catch (Exception) { MessageBox.Show(string.Format("{0}"), "Không thể Load Dimension setting"); }
        }

        public static void Load()
        {// Check if we had previously Save information of our friends previously
            if (File.Exists(dimSettingNamePath))
            {
                try
                {
                    // Create a FileStream will gain read access to the data file.
                    FileStream readerFileStream = new FileStream(dimSettingNamePath, FileMode.Open, FileAccess.Read);
                    // Reconstruct information of our friends from file.
                    dimSetting = (DimSetting)formatter.Deserialize(readerFileStream);
                    // Close the readerFileStream when we are done
                    readerFileStream.Close();
                }
                catch (Exception) { MessageBox.Show(string.Format("{0}"), "Không thể Load Dimension setting"); }
            }
        }
    }
    public class ExtractGeometry4QTO
    {
        Element strucElem;
        public ExtractGeometry4QTO (Element e)
        {
            strucElem = e;
        }
        public string GetSymbolGeometry(Element e)
        {
            Options op = new Options();
            GeometryElement geoElem = e.get_Geometry(op);
            GeometryInstance geoInstance = null;
            foreach (GeometryObject geoInst in geoElem)
            {
                if(geoInst is GeometryInstance)
                {
                    geoInstance = geoInst as GeometryInstance;
                    break;
                }
                
            }
            FamilySymbol FamSym = geoInstance.Symbol as FamilySymbol;

            //lấy Hình học ban đầu của FamilySymbol chưa bị join
            GeometryElement FamSymGeoElem= FamSym.get_Geometry(op);
            List<Solid> listSolid = new List<Solid>();

            StringBuilder sbRes = new StringBuilder();
            sbRes.AppendLine("Element\tVolume\tFacesSize\tEdgesSize");
            foreach (GeometryObject sol in FamSymGeoElem)
            {
                if (sol is Solid)
                {
                    Solid sl = sol as Solid;
                    listSolid.Add(sl);
                    string s = string.Format("{0}\t{1}\t{2}\t{3}", sl, sl.Volume, sl.Faces.Size, sl.Edges.Size);
                    sbRes.AppendLine(s);
                }                
            }
            return sbRes.ToString();
        }

        public List<Solid> GetSymbolGeometrySolids(Element e)
        {
            Options op = new Options();
            GeometryElement geoElem = e.get_Geometry(op);
            GeometryInstance geoInstance = null;
            foreach (GeometryObject geoInst in geoElem)
            {
                if (geoInst is GeometryInstance)
                {
                    geoInstance = geoInst as GeometryInstance;
                    break;
                }

            }
            FamilySymbol FamSym = geoInstance.Symbol as FamilySymbol;

            //lấy Hình học ban đầu của FamilySymbol chưa bị join
            GeometryElement FamSymGeoElem = FamSym.get_Geometry(op);
            List<Solid> listSolid = new List<Solid>();

            StringBuilder sbRes = new StringBuilder();
            sbRes.AppendLine("Element\tVolume\tFacesSize\tEdgesSize");

            List<Solid> listSol = new List<Solid>();
            foreach (GeometryObject sol in FamSymGeoElem)
            {
                if (sol is Solid)
                {
                    Solid sl = sol as Solid;
                    listSolid.Add(sl);
                    string s = string.Format("{0}\t{1}\t{2}\t{3}", sl, sl.Volume, sl.Faces.Size, sl.Edges.Size);
                    sbRes.AppendLine(s);

                    listSol.Add(sl);
                }
            }
            return listSol;
        }
        public void ExportSAT(Element e)
        {
            List<Solid> listSol = GetSymbolGeometrySolids(e);
            int order = 0;
            foreach(Solid sl in listSol)
            {
                
                order++;
            }
        }

    }
}
