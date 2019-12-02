using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using SWF = System.Windows.Forms;
using System.Text;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Exceptions;


using COFRevitAPILibs;

namespace InformationInput
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class InputInfoByExcelData : IExternalCommand
    {
        UIApplication uiApp = null;
        Document doc = null;
        Autodesk.Revit.Creation.Application app = null;

        List<DataList> listdatalist = new List<DataList>();
        public List<PairWallXYZs> listPairWallsXYZs = new List<PairWallXYZs>();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uiApp = new UIApplication(commandData.Application.Application);
            doc = commandData.Application.ActiveUIDocument.Document;
            app = commandData.Application.Application.Create;

            FormReadExcel form = new FormReadExcel(uiApp,doc,app);
            //form.TopMost = true;
            form.ShowDialog();

            listdatalist.Clear();
            listdatalist.AddRange(form.listdatalist);

            listPairWallsXYZs.AddRange(form.listPairWallsXYZs);
            WriteInfoComment();

            return Result.Succeeded;
        }

        private void WriteInfoComment()
        {
            foreach (PairWallXYZs pair in listPairWallsXYZs)
            {
                Wall w = pair.Wall;
                Element e = w as Element;
                using (Transaction t = new Transaction(doc, "Write Comments"))
                {
                    t.Start();
                    w.LookupParameter("Comments").Set(pair.Combinetextvalue());
                    t.Commit();
                }
            }
        }
    }
}
