"""
https://view.ifcopenshell.org/v/CJqGJhVVVEpqFRZnNGrcvVTGQMwFrFxs
"""

import ifcopenshell
import os


def writeTxtFromTxtString(PATH, exDat): #from txt string
	with open(PATH,"w") as f:
		f.write(exDat)	
	return exDat


path = "C:\\Users\\USER\\Documents\\GitHub\\cofico\\BIM_master\\Python\\ifcopenshell\\ifcopenshell\\NHA XUONG 27_2021.03.03 (1).ifc"#input("Enter IFC File Path:")#"R:\\BimESC\\00-BIM STANDARD\\PYTHON\\ifcopenshell\\NHA XUONG 27_2021.03.03 (1).ifc"

ifc_file = ifcopenshell.open(path)
products = ifc_file.by_type('IfcProduct')
CTO = ifc_file.by_type("IfcCartesianTransformationOperator")

pds = ifc_file.by_type("IfcProductDefinitionShape")

res = []
ssss = ""

resCTO = []
allRes = []
allResStr = ""
for product in products:
    # allRes.append(product)
    # allResStr += product.GlobalId + "\t" + product.Name + "\n"
    if product.is_a() == "IfcElementAssembly" and "CO".lower() in product.Tag.lower() : # IfcColumn = 2373 IfcElementAssembly = 11932 IfcGirt = 0 COlumn chinh = 137
        res.append(product)
        ssss += product.GlobalId + "\t" + product.Name + "\t" + product.Tag+ "\n"

print("IfcProductRepresentation = ",len(pds))
print(pds[100].get_info())
"""
IfcProductRepresentation =  48834
{'id': 4322, 'type': 'IfcProductRepresentation =  48834
{'id': 4322, 'type': 'IfcProductDefinitionShape', 'Name': None, 'Description': None, 'Representations': (#4321=IfcShapeRepresentation(#12,'Body','Brep',(#4235)),)}', 'Name': None, 'Description': None, 'Representations': (#4321=IfcShapeRepresentation(#12,'Body','Brep',(#4235)),)}
"""


# print("Count Product Column CO=",len(res))
# print("Count All Product=",len(allRes))
# print("Count IfcCartesianTransformationOperator=",len(CTO))
"""
Count Product Column CO= 137
Count All Product= 60769
Count IfcCartesianTransformationOperator= 242

dir(CTO[0])
['Axis1', 'Axis2', 'Axis3', 'LayerAssignments', 'LocalOrigin', 'Scale', 'StyledByItem', '__class__', '__delattr__', '__dict__', '__dir__', '__doc__', '__eq__', '__format__', '__ge__', '__getattr__', '__getattribute__', '__getitem__', '__gt__', '__hash__', '__init__', '__init_subclass__', '__le__', '__len__', '__lt__', '__module__', '__ne__', '__new__', '__reduce__', '__reduce_ex__', '__repr__', '__setattr__', '__setitem__', '__sizeof__', '__str__', '__subclasshook__', '__weakref__', 'attribute_name', 'attribute_type', 'get_info', 'get_info_2', 'id', 'is_a', 'unwrap_value', 'walk', 'wrap_value']
CTO[0]Axist1 = None
CTO[0]Axist2 = None
CTO[0]Axist3 = None
CTO[0]LocalOrigin = #6=IfcCartesianPoint((0.,0.,0.))
CTO[0]get_info = {'id': 151, 'type': 'IfcCartesianTransformationOperator3D', 'Axis1': None, 'Axis2': None, 'LocalOrigin': #6=IfcCartesianPoint((0.,0.,0.)), 'Scale': None, 'Axis3': None}
"""
# print("CTO[0]Axist1 =",CTO[0].Axis1)
# print("CTO[0]Axist2 =",CTO[0].Axis2)
# print("CTO[0]Axist3 =",CTO[0].Axis3)
# print("CTO[0]LocalOrigin =",CTO[0].LocalOrigin)
# print("CTO[0]get_info =",CTO[0].get_info())

col = res[100]

# pathWrite = "C:\\Users\\USER\\Documents\\GitHub\\cofico\\BIM_master\\Python\\ifcopenshell\\ifcopenshell\\test.txt"#input("Enter Exported TXT File Path:")#"R:\\BimESC\\00-BIM STANDARD\\PYTHON\\ifcopenshell\\test.txt"
# fn = writeTxtFromTxtString(pathWrite,allResStr)
res2 = []
res3 = []
res4 = []


# for co in products:
#     if co.is_a() == "IfcColumn": 
#         res2.append(co)
# for rh in products:
#     if rh.is_a() == "LcIfcRepresentationHolder":
#         res3.append(rh)
# for go in products:
#     if go.is_a() == "Geometry": # IfcColumn = 2373 IfcElementAssembly = 11932 IfcGirt = 0 COlumn chinh = 137
#         res4.append(go)

# print(res)
# print(res2)
# print(res3)
# print(res4)
print("GET INFO PlacementRelTo = ",col.ObjectPlacement.PlacementRelTo.PlacementRelTo.PlacementRelTo.get_info())#.ObjectPlacement)#ObjectPlacement.RelativePlacement.Location)
"""
GET INFO PlacementRelTo =  #30=IfcLocalPlacement(#28,#10)
{'id': 30, 'type': 'IfcLocalPlacement', 'PlacementRelTo': #28=IfcLocalPlacement(#25,#10), 'RelativePlacement': #10=IfcAxis2Placement3D(#6,#9,#7)}
{'id': 28, 'type': 'IfcLocalPlacement', 'PlacementRelTo': #25=IfcLocalPlacement($,#10), 'RelativePlacement': #10=IfcAxis2Placement3D(#6,#9,#7)}
{'id': 25, 'type': 'IfcLocalPlacement', 'PlacementRelTo': None, 'RelativePlacement': #10=IfcAxis2Placement3D(#6,#9,#7)}
"""



# print(col.ObjectPlacement.ReferencedByPlacements)
# print(dir(col.ObjectPlacement))#AssemblyPlace)
# print(dir(col))


property_set=[]

# for definition in col.IsDefinedBy:
#     # To support IFC2X3, we need to filter our results.
#     if definition.is_a('IfcRelDefinesByProperties'):
#         # print(dir(definition))
#         property_set = definition.RelatingPropertyDefinition
#         # print(dir(property_set))
#         print(property_set.Name) # Might return Pset_WallCommon
#         print(property_set.PropertyDefinitionOf)
#         for defi in property_set.PropertyDefinitionOf:
#             if defi.is_a('IfcRelDefinesByProperties'):
#                 proSet = defi.RelatingPropertyDefinition
#                 print(proSet.Name)



# try:
#     for property in property_set.HasProperties:
#         if property.is_a('IfcPropertySingleValue'):
#             print(property.Name)
#             print(property.NominalValue.wrappedValue)
# except:
#     pass

#print(dir(res[0]))



# if col.ObjectPlacement.PlacementRelTo:
#     # Inherit the coordinates of its parents
#     pass
# local_coordinates = col.ObjectPlacement.RelativePlacement.Location[0]
# print(local_coordinates)


# geo = col.Position
# print(geo)

# print(col.get_info())
"""
{'id': 31390, 'type': 'IfcElementAssembly', 'GlobalId': '1W1JDn001Bvp4sCJ0vDJOo', 'OwnerHistory': #5=IfcOwnerHistory(#3,#4,$,.NOCHANGE.,$,$,$,1614765940), 'Name': 'Steel Assembly', 'Description': None, 'ObjectType': None, 'ObjectPlacement': #31389=IfcLocalPlacement(#30,#10), 'Representation': None, 'Tag': 'CO54(?)', 'AssemblyPlace': 'NOTDEFINED', 'PredefinedType': 'RIGID_FRAME'}
"""
# def getAllgeometry(elem):
#     res = []
#     # for el in elem:
#     #     res.append(el)

#     res = elem.ReferenceTo
#     return res

# print(getAllgeometry(col))
