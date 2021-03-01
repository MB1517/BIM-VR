import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

clr.AddReference("RevitServices")
import RevitServices
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager
 
doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application


#The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN

catName = IN[0]
res = []
cat = []

for e in doc.Settings.Categories:
	#res.append(e.Name)
	if e.Name == catName.ToString():
		cat = e

elems = FilteredElementCollector(doc).OfCategoryId(cat.Id).WhereElementIsNotElementType()

for e in elems:
	res.append(e)
#Assign your output to the OUT variable.
OUT = res