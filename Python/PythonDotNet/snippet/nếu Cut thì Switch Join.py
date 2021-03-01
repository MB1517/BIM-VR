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
import Autodesk.Revit.DB.JoinGeometryUtils as JGU # de Join 

#The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN

list = IN[0]

res = []

# Start Transaction
TransactionManager.Instance.EnsureInTransaction(doc)
try:
	for l in list:
		A = UnwrapElement(l[0])
		jes = l[1]
		
		for j in jes:
			B = UnwrapElement(j)
			boolCut = JGU.IsCuttingElementInJoin(doc,A,B)
			if boolCut == True:
				JGU.SwitchJoinOrder(doc,B,A)
except:
    pass
 
# End Transaction
TransactionManager.Instance.TransactionTaskDone()
doc.Regenerate()

#Assign your output to the OUT variable.
OUT = list