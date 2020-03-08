using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace DataManager
{
    [PluginAttribute("DataManager.HelloNaviswork",                   //Plugin name
                    "NDMNG",                                       //4 character Developer ID or GUID
                    ToolTip = "Data Seeking tool tip",            //The tooltip for the item in the ribbon
                    DisplayName = "Data Seeking Plugin")]          //Display name for the Plugin in the Ribbon
    //C:\Program Files\Autodesk\Navisworks Manage 2017\Plugins\SiteUtils
    public class DataSeeker : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            //MessageBox.Show(string.Format("Hello {0}", "Naviswork"), "Thông báo");
            DataSeekerForm form = new DataSeekerForm();
            form.ShowDialog();


            return 0;
        }
    }
}
