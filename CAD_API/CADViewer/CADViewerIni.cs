using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;


using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Interop;

using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Microsoft.Win32;


namespace CADViewer
{
    public partial class CADVierIni : Form
    {
        public CADVierIni()
        {
            InitializeComponent();
        }

        private void bOpenCADApp_Click(object sender, EventArgs e)
        {
            ConnectToAcad();
        }
        public static void ConnectToAcad()
        {

            AcadApplication acAppComObj = null;
            const string strProgId = "AutoCAD.Application.21"; // 21 là 2017, 23 là 2019
            // Get a running instance of AutoCAD
            try
            {
                acAppComObj = (AcadApplication)Marshal.GetActiveObject(strProgId);
            }
            catch // An error occurs if no instance is running
            {
                try
                {
                    // Create a new instance of AutoCAD
                    acAppComObj = (AcadApplication)Activator.CreateInstance(Type.GetTypeFromProgID(strProgId), true);
                }
                catch
                {
                    // If an instance of AutoCAD is not created then message and exit
                    System.Windows.Forms.MessageBox.Show("Instance of 'AutoCAD.Application'" +
                                                         " could not be created.");
                    return;
                }
            }

            // Display the application and return the name and version
            acAppComObj.Visible = true;

            MessageBox.Show("Now running " + acAppComObj.Name +
                                                 " version " + acAppComObj.Version);            
        }
    }
}
