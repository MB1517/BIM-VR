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

list = IN[0]
res = []
flag = list.__class__.__name__ == "List[object]"
if flag:
	OUT = 0
else:
	for e in list:
		if not e:
			continue
		else:
			res.append(e)
	OUT = res
			
			
	