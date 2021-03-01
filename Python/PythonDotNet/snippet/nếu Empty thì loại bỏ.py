import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import *
#The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN
list = IN[0]
res = []
for l in list:
	e = l[0]
	je = l[1]	
	if len(je) == 0:
		res.append(l)
		list.remove(l)
#Assign your output to the OUT variable.
OUT = list#,res