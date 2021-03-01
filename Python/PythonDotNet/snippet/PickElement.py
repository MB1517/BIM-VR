# Load the Python Standard and DesignScript Libraries
import sys
import clr
#clr.AddReference('ProtoGeometry')
#from Autodesk.DesignScript.Geometry import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *


clr.AddReference("RevitAPIUI")
from Autodesk.Revit.UI.Selection import *

clr.AddReference("RevitServices")
import RevitServices
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager
 
doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application


# The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN

# Place your code below this line

object = doc.GetElement(uiapp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Chọn Wall").ElementId)



# Assign your output to the OUT variable.
OUT = object