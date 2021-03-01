clr.AddReference('System')
from System.Collections.Generic import List

eList = List[LinkElementId]()
for e in elements:
	eList.Add(LinkElementId(link.Id,e.Id))


OUT = eList