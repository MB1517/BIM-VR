import re
str = "1P5-3 (3-4/E), 2P5-3 (3/E), 4P5-3A (3-4/E-D), 5P5-3 (3/E-D), 5P5-3 (3-4/D), 4P5-1A (4+4,7m/C), 11P5-1 (5/F-G), 2P5-1F (6/H), 2P5-1F (6-7/H), 3P5-1 (8/H), 2P5-1 (8-9/P4-F)"


cond1 = ".\d,\d\w"
test1 = ""
try:
	test1 = re.findall(cond1,str)
except Exception, ex:
	test = ex
	pass


print(test1)