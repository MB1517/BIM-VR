# NOTE should using ipy64 to interpreter

import sys
import clr
#clr.AddReference('ProtoGeometry')
#from Autodesk.DesignScript.Geometry import *
import sys
sys.path.append("C:\Program Files\Autodesk\Revit 2020")

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *
clr.AddReference("RevitAPIUI")
#from Autodesk.Revit.UI.Selection import *

print("Loaded Revit API: ","RevitAPI" in [assembly.GetName().Name for assembly in clr.References])
#print (dir(clr))