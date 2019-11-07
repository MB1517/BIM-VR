using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

//using external .dll
using AutoDimension;

namespace AutoDimension
{
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class AutoDim : IExternalApplication
    {
        public static UIApplication uiapp = null;
        public static UIDocument uidoc = null;
        public static Document doc = null;
        public Autodesk.Revit.Creation.Application app = null;

        public static string AddInP = typeof(AutoDim).Assembly.Location;
        public static string AddInPath = Path.GetDirectoryName(AddInP);
        public static string ButtonIconsFolder = Path.GetDirectoryName(AddInP) + "\\Icons\\";

        public string TabName = "Auto_Dimension";

        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();

        }

        public Result OnStartup(UIControlledApplication application)
        {            
            // TAB: Drawing Utilities
            string tabName = TabName;
            application.CreateRibbonTab(tabName);

            // PANEL: DIMENSION TOOLS
            RibbonPanel rp = application.CreateRibbonPanel(tabName, TabName);
            try
            {
                RP_Dimension(application, rp);
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show(TabName, ex.Message.ToString());
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
        private void RP_Dimension(UIControlledApplication app, RibbonPanel rb)
        {
            
            PushButtonData pbdDimRoom = new PushButtonData(TabName, TabName, AddInPath + "\\AutoDimension.dll", "AutoDimension.DimRoom");
            pbdDimRoom.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Window.png"), UriKind.Absolute));
            pbdDimRoom.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Window.png"), UriKind.Absolute));
            pbdDimRoom.ToolTip = "Some Tip Here";


            rb.AddItem(pbdDimRoom);
        }
    }
}
