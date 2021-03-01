def findNearestData(p0,points,data):
	d = 100000
	idx = 0
	x0 = p0.X
	y0 = p0.Y
	
	if not len(points) == len(data):
		return "Counting not match"
	
	for i in range(len(points)):
		p = points[i]
		x = p.X
		y = p.Y		
		dd = calDistance(x0,y0,x,y)
		if dd < d:
			d = dd
			idx = i
	return data[idx]
		
		
def calDistance(x0,y0,x,y):
	dx = abs(x0-x)
	dy = abs(y0-y)		
	res = (dx**2 + dy**2)**(0.5)		
	return res