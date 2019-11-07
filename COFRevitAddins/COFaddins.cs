using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

//using external .dll
using AutoDimension;

namespace COFRevitAddins
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class AddinsPanel : IExternalCommand
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


            return Result.Succeeded;
        }
    }



    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class COFaddins : IExternalApplication
    {
        public static UIApplication uiapp = null;
        public static UIDocument uidoc = null;
        public static Document doc = null;
        public Autodesk.Revit.Creation.Application app = null;

        public static string AddInP = typeof(COFaddins).Assembly.Location;
        public static string AddInPath = Path.GetDirectoryName(AddInP);
        public static string ButtonIconsFolder = Path.GetDirectoryName(AddInP) + "\\Icons\\";

        public string TabName = "DxD_addins";

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
            
            PushButtonData pvdAddinsPanel = new PushButtonData("AddinsPanel", "AddinsPanel", AddInPath + "\\COFaddins.dll", "COFaddins.AddinsPanel");
            pvdAddinsPanel.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Window.png"), UriKind.Absolute));
            pvdAddinsPanel.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Window.png"), UriKind.Absolute));
            pvdAddinsPanel.ToolTip = "Some Tip Here";


            rb.AddItem(pvdAddinsPanel);
        }
    }
}
