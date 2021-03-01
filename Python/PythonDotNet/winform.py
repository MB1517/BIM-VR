import clr

clr.AddReference('System.IO')
clr.AddReference('System.Drawing')
clr.AddReference('System.Reflection')
clr.AddReference('System.Threading')
clr.AddReference("System.Windows.Forms")

import System
import System.IO
import System.Drawing
import System.Reflection
import System.Windows.Forms

from System.Threading import ApartmentState, Thread, ThreadStart
from System.Windows.Forms import *

# class InteropExplorer(System.Windows.Forms.Form):
#     def _init_(self):
#         self.Text = "Interop Explorer"


#print("System.Windows.Forms" in [assembly.GetName().Name for assembly in clr.References])
MessageBox.Show("Hello work")