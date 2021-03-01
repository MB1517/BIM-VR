res = []

flag = elems.__class__.__name__ =="List[object]"

#.__class__.__name__ = "str" //string
#.__class__.__name__ = "int" // interger
#.__class__.__name__ = "float" // double
#.__class__.__name__ = "bool" // boolean

if not flag:
	if elems.Symbol.LookupParameter("OmniClass Number").AsString() == omni:
		res.append(elems)	
	OUT = "not list"

else:
	if len(elems)> 1:
			
		OUT = "list"