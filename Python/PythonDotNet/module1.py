def sayHello():
	print('Hello')

def allElemsOfCat (catName,doc):
	res = []
	cat = []
	try:
		for e in doc.Settings.Categories:			
			if e.Name == catName.ToString():
				cat = e	
		elems = FilteredElementCollector(doc).OfCategoryId(cat.Id).WhereElementIsNotElementType()	
		for e in elems:
			res.append(e)
	except:
		pass
	return res

def allElemsIntersectedOfCat(ee,catName,doc):
	res = []
	cat = []
	for e in doc.Settings.Categories:
	#res.append(e.Name)
		if e.Name == catName.ToString():
			cat = e	
	elems = FilteredElementCollector(doc).OfCategoryId(cat.Id).WherePasses(ElementIntersectsElementFilter(ee))#.WhereElementIsNotElementType()
	for eee in elems:
		res.append(eee)
	return res   

def allElemsNOTIntersectedOfCat(ee,catName,doc):
	res = []
	cat = []
	for e in doc.Settings.Categories:
	#res.append(e.Name)
		if e.Name == catName.ToString():
			cat = e	
	elems = FilteredElementCollector(doc).OfCategoryId(cat.Id).WherePasses(ElementIntersectsElementFilter(ee),False)#.WhereElementIsNotElementType()
	for eee in elems:
		res.append(eee)
	return res

def joinTwoElement(a,b):	
	boolCut = JGU.AreElementsJoined(doc,a,b)
	if not boolCut:
		JGU.JoinGeometry(doc,a,b)
		if not JGU.IsCuttingElementInJoin(doc,a,b):
			JGU.SwitchJoinOrder(doc,a,b)
def UNjoinTwoElement(a,b):
	JGU.UnjoinGeometry(doc,a,b)

def joinAllEnableJoinedElemsOfCat(e,cat,doc):
	elems = allElemsIntersectedOfCat(e,cat,doc)
	for el in elems:
		joinTwoElement(e,el)
def UNjoinAllNOTJoinedElemsOfCat(e,cat,doc):
	elems = allElemsNOTIntersectedOfCat(e,cat,doc)
	for el in elems:
		UNjoinTwoElement(e,el)
####
def getSolids(e):
	solid = []
	geoE1 = e.get_Geometry(opt)
	geoE2 = []
	try:
		enum = geoE1.GetEnumerator()	
		while enum.MoveNext():
			geoE2 = enum.Current
		if isinstance(geoE2,Solid):
			solid.Add(geoE2)
		
		else:
			if isinstance(geoE2,GeometryInstance):
				geoObj = geoE2.GetInstanceGeometry()
				for s in geoObj:
					if isinstance(s,Solid) and s.Volume > 0:
						solid.Add(s)	
	except Exceptyion, ex:
		mergedSolid.append(ex)
		pass	
	return solid
####
def UnionSolid (solids):
	mergedSolid = []
	try:
		if len(solids) == 0:
			return null
		if len(solids) == 1:
			return solids[0]
		else:			
			first = solids[0]
			res = solids[1:]
			second = UnionSolid(res)			
			mergedSolid = BooleanOperationsUtils.ExecuteBooleanOperation(first,second,BooleanOperationsType.Union)
	except Exceptyion, ex:
		mergedSolid.append(ex)
		pass
	return mergedSolid

def getRawVolume(solids):
	vol = 0
	try:
		for s in solids:
			vol += s.Volume*0.0283168
	except:	
		pass
	return vol

def getSeftVolume(solids): #Intersect SOlids togeget
	mergedSolid = []
	vol = []
	try:
		mergedSolid = UnionSolid(solids)
		vol = mergedSolid.Volume*0.0283168
	except:	
		pass
	return vol
def exportSolidToSAT(app,msolid,path):
	res = []
	#FAMILY CREATION
	famPath = app.FamilyTemplatePath + "\\Conceptual Mass\\Metric Mass.rft"
	famdoc = app.NewFamilyDocument(famPath)
	#create Freeform by solid
	with Transaction(famdoc,"Create Free Form") as t:
		t.Start()
		try:	
			freeFormElement = FreeFormElement.Create(famdoc,msolid)
		except Exception, ex:
			res.append(ex)
			pass
		t.Commit()
	tempDir = tempfile.gettempdir()
	tempFamName = tempDir + "\\temFamily"+".rfa"
	#if os.path.isfile(tempFamName):
		#tempFamName = tempDir + "\\temFamily"+"-"+ msolid.UniqueId.ToString() +".rfa"
	
	sao = SaveAsOptions()
	sao.OverwriteExistingFile = True
	famdoc.SaveAs(tempFamName,sao)
	"""
	sav = SaveOptions()
	famdoc.Save(sav)
	"""
	viewFamTypes = FilteredElementCollector(famdoc).OfClass(ViewFamilyType)
	viewTypeID = 0
	
	for vft in viewFamTypes:
		if vft.ViewFamily == ViewFamily.ThreeDimensional:
			viewTypeID = vft.Id
	
	#export SAT
	with Transaction(famdoc,"Export SAT") as tt:
		tt.Start()
		view3D = View3D.CreateIsometric(famdoc,viewTypeID)
		tt.Commit()
	
	viewSet = List[ElementId]()
	viewSet.Add(view3D.Id)
	exportOptions=SATExportOptions()
	famdoc.Export (path,"SolidFile.sat", viewSet, exportOptions)
	return res


def readLinesDYNString(str):
	res = []
	for s in catNames.split("\n"):
		res.append(s[:-1])
	return res

def exportSolidToSATMulti(app,listmsolid,path):
	
	res = []
	#FAMILY CREATION
	famPath = app.FamilyTemplatePath + "\\Conceptual Mass\\Metric Mass.rft"
	famdoc = app.NewFamilyDocument(famPath)	
	tempDir = tempfile.gettempdir()
	tempFamName = tempDir + "\\temFamily"+".rfa"
	#if os.path.isfile(tempFamName):
		#tempFamName = tempDir + "\\temFamily"+"-"+ msolid.UniqueId.ToString() +".rfa"	
	sao = SaveAsOptions()
	sao.OverwriteExistingFile = True
	famdoc.SaveAs(tempFamName,sao)
	"""
	sav = SaveOptions()
	famdoc.Save(sav)
	"""
	viewFamTypes = FilteredElementCollector(famdoc).OfClass(ViewFamilyType)
	viewTypeID = 0	
	for vft in viewFamTypes:
		if vft.ViewFamily == ViewFamily.ThreeDimensional:
			viewTypeID = vft.Id
	
	with Transaction(famdoc,"CreateView") as tt:
		tt.Start()
		view3D = View3D.CreateIsometric(famdoc,viewTypeID)
		tt.Commit()
	viewSet = List[ElementId]()
	viewSet.Add(view3D.Id)	
	
	#export SAT
	#create Freeform by solid
	for i in range(len(listmsolid)):
		msolid = listmsolid[i]
		with Transaction(famdoc,"Create Free Form") as t:
			t.Start()
			try:	
				freeFormElement = FreeFormElement.Create(famdoc,msolid)
			except Exception, ex:
				res.append(ex)
				pass
			t.Commit()
		exportOptions=SATExportOptions()
		satFN = "Solid" + "-" + str(i) +".sat"
		famdoc.Export (path,satFN, viewSet, exportOptions)
		with Transaction(famdoc,"Delete") as ttt:
			ttt.Start()
			if not os.path.isfile(path+"\\"+satFN):
				famdoc.Delete(freeFormElement.Id)	
			ttt.Commit()			
		
		#sav = SaveOptions()
		#famdoc.Save(sav)
		res.append(i)	
	
	return res
