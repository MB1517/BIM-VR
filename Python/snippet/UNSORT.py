#Split String to List
import clr
clr.AddReference('ProtoGeometry')
from Autodesk.DesignScript.Geometry import *
#The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN

splitData = IN[0]
res = []

if len(splitData)>0:
	for d in splitData:
		try:
			res.append(d.split(" ",1))
		except:
			res.append(d)
			pass
else:
	try:
		res.append(splitData.split(" ",1))
	except:
		res.append(splitData)
		pass
#Assign your output to the OUT variable.
OUT = res


# Load the Python Standard and DesignScript Libraries
import sys
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

##
doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application

opt = Options()
# The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN


##

res = []

res.append(app.VersionName)
res.append(app.SharedParametersFilename)
res.append(app.Username)
res.append(doc.PathName)
res.append(list(enumerate(doc.ProjectLocations)))
res.append(doc.SiteLocation)
# Place your code below this line

# Assign your output to the OUT variable.
OUT = res


# The inputs to this node will be stored as a list in the IN variables.
dataEnteringNode = IN
L=IN[0]

################################## TRANSPOSE
transposeList = map(list, zip(*L))
################################# FIND INDEX OF LIST ELEMENT
indx = transposeList[0].index("*PARAM")
newlist = L[indx+1:]
################################## get Keynote table & Assemly Code Table
extFileIds = GetAllExternalFileReferences(doc)
for ei in extFileIds:
	res2.append(doc.GetElement(ei))

##################################check CLASS NAMEdoc.GetElement(ei).__class__.__name__
################################### try except logging
try:
#code here
except Exception, e:
	definition = e.ToString()

############################ python enum
	defGSenum = enumerate(definition.Groups.GetEnumerator())
	for defG in defGSenum:
		res3.append(defG)
# NEAREST GRID
griddata = IN[0]
elemPoints = IN[1]

res1 = []

for i in range(len(elemPoints)):
	ep = elemPoints[i]
	try:
		x = float(ep.split(" ")[0])
		y = float(ep.split(" ")[1])		
		r = []
		
		for gd in griddata:
			xg1 = float(gd[1].split(" ")[0])
			yg1 = float(gd[1].split(" ")[1])
			
			xg2 = float(gd[2].split(" ")[0])
			yg2 = float(gd[2].split(" ")[1])
			
			l1 = ((xg1-x)**2+(yg1-y)**2)**0.5
			l2 = ((xg2-x)**2+(yg2-y)**2)**0.5
			l3 = ((xg2-xg1)**2+(yg2-yg1)**2)**0.5
			
			p=(l1+l2+l3)/2
			
			h = 2*((p*(p-l1)*(p-l2)*(p-l3))**0.5)/l3
			
			r.append(h)		
			
		
		idx = [i for i, x in enumerate(r) if x == min(r)]	
		
		str = griddata[idx[0]][0].ToString()+"("+round(r[idx[0]],3).ToString()+")"
		
		#res1.append(str)
		res1.append(griddata[idx[0]][0].ToString())
		
	except:
		res1.append([])
		pass
		
		
#Assign your output to the OUT variable.
OUT = res1

	
