import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import *
clr.AddReference('RevitAPI')
from Autodesk.Revit.DB import *
clr.AddReference('RevitAPIUI')
from Autodesk.Revit.UI import TaskDialog
clr.AddReference("RevitNodes")
import Revit
clr.ImportExtensions(Revit.Elements)
clr.AddReference("RevitServices")
import RevitServices
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager
from System.Collections.Generic import *
clr.ImportExtensions(Revit.GeometryConversion)
 
doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application
 
import sys
import System

import Autodesk.Revit.DB.JoinGeometryUtils as JGU
#The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN

elems = UnwrapElement(IN[0])
res = []

finRes = []

if not len(elems) == 0:
	for e in elems:
		je = JGU.GetJoinedElements(doc,e)
		ees = []
		for id in je:
			ee = doc.GetElement(id)
			ees.append(ee)			
		res.append(ees)
	finRes.append(elems)
	finRes.append(res)
else:
	finRes = 0

#Assign your output to the OUT variable.
OUT = finRes