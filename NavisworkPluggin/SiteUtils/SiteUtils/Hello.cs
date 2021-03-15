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

namespace SiteUtils
{
    [PluginAttribute("SiteUtils.HelloNaviswork",                   //Plugin name
                    "COFN",                                       //4 character Developer ID or GUID
                    ToolTip = "HelloNaviswork tool tip",            //The tooltip for the item in the ribbon
                    DisplayName = "Hello World Plugin")]          //Display name for the Plugin in the Ribbon
    //C:\Program Files\Autodesk\Navisworks Manage 2017\Plugins\SiteUtils
    public class HelloNaviswork : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            MessageBox.Show(string.Format("Hello {0}", "Naviswork"), "Thông báo");
            return 0;
        }
    }
}
