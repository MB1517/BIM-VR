import clr

clr.AddReference("RevitServices")
import RevitServices
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager
doc = DocumentManager.Instance.CurrentDBDocument

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import WallUtils

walls = UnwrapElement(IN[0])

TransactionManager.Instance.EnsureInTransaction(doc)

for i in range(0,len(walls)):
	WallUtils.DisallowWallJoinAtEnd(walls[i],0)
	WallUtils.DisallowWallJoinAtEnd(walls[i],1)
	
TransactionManager.Instance.TransactionTaskDone()
OUT = "Successfull"