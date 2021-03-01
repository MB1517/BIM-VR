import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import *

clr.AddReference('RevitAPI')
from Autodesk.Revit.DB import *

#The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN

elems = UnwrapElement(IN[0])
res = []
for e in elems:
	className = e.Location.__class__.__name__
	if className == "LocationCurve":
		res.append(e)	

#Assign your output to the OUT variable.
OUT = res