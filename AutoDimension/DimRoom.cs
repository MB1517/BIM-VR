using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;

using COFRevitAPILibs;

namespace AutoDimension
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class DimRoom : IExternalCommand
    {
        UIApplication uiApp = null;
        Document doc = null;
        Autodesk.Revit.Creation.Application app = null;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("ĐANG TRONG QUÁ TRÌNH XÂY DỰNG\nUNDER CONSTRUCTION");
            uiApp = new UIApplication(commandData.Application.Application);
            doc = commandData.Application.ActiveUIDocument.Document;
            app = commandData.Application.Application.Create;
            DimUtil dU = new DimUtil(uiApp, doc, app);

            //LOAD SETTING DIM OPTIONS ** ON GOING
            //string path = doc.PathName;
            //string title = doc.Title;
            //string directory = path.Substring(0, path.Length - title.Length);


            List<Room> rooms = dU.getAllRooms();// BẮT TẤT CẢ ROOM TRONG DỰ ÁN
            if (rooms.Count == 0)
            {
                return Result.Cancelled;
            }
            XYZ pt = dU.pickForTestPoint();// CHỌN 1 POINT Ở GIỮA ROOM
            Room r = dU.findRoomContainPoint(pt, rooms);// TÌM RA ROOM CHỨA POINT
            if (r == null)
            {
                MessageBox.Show("Không tìm ra Room chứa Picked Point", "");
                return Result.Failed;
            }
            Line lineV = dU._YdirectLineFromPoint(pt);//TẠO 2 LINE DỌC VÀ NGANG
            Line lineH = dU._HorizontalLineFromPoint(pt);
            //TẠO DIMENSION
            List<Dimension> listDim = new List<Dimension>();
            listDim = dU.DimensionForWallsOfRoom(r, lineV, lineH); // đã fix Transaction cho multy Tasks
            // XÉT CÁC TRƯỜNG HỢP THEO OPTION ** ON GOING
            /*
            int StringMode = int.Parse(Ribbon.tb.Value.ToString()); //MessageBox.Show(StringMode.ToString());
            switch (Ribbon.DimRoomMode)
            {
                case "Dim lọt lòng và phủ bì tường":
                    {
                        listDim = dU.DimensionForWallsOfRoom(r, lineV, lineH); break;
                    }
                case "Dim phủ bì tường bằng Reference Plane":
                    {
                        listDim = dU.DimensionForReferencePlanesOfRoom(r, lineV, lineH); break;
                    }                    
            }*/
            return Result.Succeeded;

        }
    }
}
