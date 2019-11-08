using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using swf = System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Exceptions;

namespace RevitExportCollada
{
    public partial class ColladaExporter_Form : swf.Form
    {
        UIApplication uiApp = null;
        Document doc = null;
        Autodesk.Revit.Creation.Application app = null;
        private string suffix;
        private string directoryPath;
        private string fullNamePath;
        private string prefix;
        private string projectFileName;

        string PathName,ViewName,BuildingName,PName,Number,OrganizationName,Author;
        string rawName;


        StringBuilder sbRes = new StringBuilder();

        public string DirectoryPath { get => directoryPath; set => directoryPath = value; }
        public string FullNamePath { get => fullNamePath; set => fullNamePath = value; }
        public string Prefix { get => prefix; set => prefix = value; }
        public string Suffix { get => suffix; set => suffix = value; }
        public string ProjectFileName { get => projectFileName; set => projectFileName = value; }

        public ColladaExporter_Form(UIApplication _uiApp ,Document _doc,Autodesk.Revit.Creation.Application _app )
        {
            uiApp = _uiApp;
            doc = _doc;
            app = _app;

            PathName= doc.PathName;
            ViewName = doc.ActiveView.ViewName;
            BuildingName = doc.ProjectInformation.BuildingName;
            PName = doc.ProjectInformation.Name;
            Number = doc.ProjectInformation.Number;
            OrganizationName = doc.ProjectInformation.OrganizationName;
            Author = doc.ProjectInformation.Author;

            rawName = string.Format(@"{0}-{1}-{2}-{3}-{4}-{5}", PName, BuildingName, Number, OrganizationName, Author, ViewName);


            InitializeComponent();
        }

        private void bBrowser_Click(object sender, EventArgs e)
        {
            ChooseFolder();
            updateSbRes();
        }

        public void ChooseFolder()
        {
            if (folderBrowserDialog1.ShowDialog() == swf.DialogResult.OK)
            {
                DirectoryPath = tDirectoryPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void tPrefix_TextChanged(object sender, EventArgs e)
        {
            DirectoryPath = tDirectoryPath.Text;
            rtbFullName.Text = string.Format(@"{0}\{1}.dae", DirectoryPath, rawName);

            Prefix = tPrefix.Text;
            Suffix = tSuffix.Text;
            FullNamePath = rtbFullName.Text;

            string path = doc.PathName.ToString();
        }

        private void ColladaExporter_Form_Load(object sender, EventArgs e)
        {
            updateSbRes();
            
            DirectoryPath = "";
            Prefix = "";
            Suffix = "";

        }
        void updateSbRes()
        {
            sbRes.Clear();
            sbRes.AppendLine(string.Format(@"doc.PathName:{0}", doc.PathName));
            sbRes.AppendLine(string.Format(@"doc.ActiveView:{0}", doc.ActiveView.ViewName));
            sbRes.AppendLine(string.Format(@"doc.ProjectInformation.BuildingName:{0}", doc.ProjectInformation.BuildingName));
            sbRes.AppendLine(string.Format(@"doc.ProjectInformation.Name:{0}", doc.ProjectInformation.Name));
            sbRes.AppendLine(string.Format(@"doc.ProjectInformation.Number:{0}", doc.ProjectInformation.Number));
            sbRes.AppendLine(string.Format(@"doc.ProjectInformation.OrganizationName:{0}", doc.ProjectInformation.OrganizationName));
            sbRes.AppendLine(string.Format(@"doc.ProjectInformation.Author:{0}", doc.ProjectInformation.Author));

            rtbRes.Text = sbRes.ToString();
        }

        private void bCreateName_Click(object sender, EventArgs e)
        {
            /*
            DirectoryPath = tDirectoryPath.Text;
            rtbFullName.Text = string.Format(@"{0}\{1}-{2}-{3}-{4}.dae", DirectoryPath, Prefix, lPFName.Text, lVName.Text, Suffix);
            Prefix = tPrefix.Text;
            Suffix = tSuffix.Text;
            FullNamePath = rtbFullName.Text;
            string path = doc.PathName.ToString();
            lPFName.Text = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 5);*/
            string newName = Prefix + rawName + Suffix;
            rtbFullName.Text = string.Format(@"{0}\{1}.dae", DirectoryPath, newName);
        }
    }
}
