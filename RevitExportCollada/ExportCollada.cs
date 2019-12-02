using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using swf=System.Windows.Forms;
using System.Text;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Exceptions;


using COFRevitAPILibs;


namespace RevitExportCollada
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class ExportCollada : IExternalCommand
    {
        UIApplication uiApp = null;
        Document doc = null;
        Autodesk.Revit.Creation.Application app = null;
        public static string namePath;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            
            //
            //
            //swf.MessageBox.Show("ĐANG TRONG QUÁ TRÌNH XÂY DỰNG\nUNDER CONSTRUCTION");
            uiApp = new UIApplication(commandData.Application.Application);
            doc = commandData.Application.ActiveUIDocument.Document;
            app = uiApp.Application.Create;
            //
            //
            //
            ColladaExporter_Form form = new ColladaExporter_Form(uiApp, doc, app);
            form.ShowDialog();
            namePath = form.FullNamePath;
            //
            //
            View view = doc.ActiveView;
            //
            //
            //
            IList<ElementId> listIDs = new FilteredElementCollector(doc,view.Id).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElementIds().ToList();
            //view.SetVisibility(catWall, false);
            //
            //
            //
            if (view is View3D) ExportView3D(doc, view as View3D);
            else swf.MessageBox.Show("Collada Export","You must be in 3D view to export.");
            //
            //
            //
            return Result.Succeeded;
        }
        
        static void ExportView3D(Document document,View3D view3D)
        {

            MyExportContext context = new MyExportContext(document, namePath);
            CustomExporter exporter = new CustomExporter(document, context);
            // Note: Excluding faces just excludes the calls, 
            // not the actual processing of face tessellation. 
            // Meshes of the faces will still be received by 
            // the context.
            //exporter.IncludeFaces = false;
            exporter.ShouldStopOnError = false;
            try
            {
                exporter.Export(view3D);
            }
            catch (ExternalApplicationException ex)
            {
                string res = "ExternalApplicationException " + ex.Message;
                Debug.Print(res);
                swf.MessageBox.Show(res);
            }
        }
    }    
}
