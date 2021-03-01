# Load the Python Standard and DesignScript Libraries
import sys
# import clr
# clr.AddReference('ProtoGeometry')
# from Autodesk.DesignScript.Geometry import *

from shutil import copyfile
import tempfile
import os
from os import listdir
from os.path import isfile, join
import sqlite3
import uuid
# The inputs to this node will be stored as a list in the IN variables.
# dataEnteringNode = IN
# dbPath = IN[0]

dbName = "dof.db"
dbpath = "C:/Users/USER/Downloads/DYNAMO-210218/210224/wip/"+dbName

res =[]
uid = uuid.uuid4()
tableName = "bim_4d_tb"#project4dTB
#-----------------------------------------
# INSERT COMMAND
iniInsert = "INSERT INTO {0} VALUES ('{1}','0ace5d6b-9f8e-4d28-a62a-61937a6fa895-000cc472','DOF-01-FD','FD','23/02/2021','23/02/2021','23/02/2021','23/02/2021','tvpduy','NCR123456',0.5,100,'m3','Foundation','tvpduy','25/02/2021')"
commandInsert = iniInsert.format(tableName,uid)

# CREATE TABLE COMMAND
iniCreateTable = "CREATE TABLE {} (guid TEXT PRIMARY KEY,elemGUID TEXT, itNumber TEXT, itemName TEXT, PSdate TEXT, PEdate TEXT, ASdate TEXT, AEdate TEXT, personInCharged TEXT, NCR TEXT, complete REAL,quantity REAL, qUnit TEXT, abreviation TEXT, byUser TEXT, ActualUpdatedDate TEXT)"
commandCreateTable = iniCreateTable.format(tableName)
# SEARCH COMMAND
iniSearch = 'SELECT * FROM {}'
commandSearch = iniSearch.format(tableName)
#-----------------------------------------

#### CONNECT DATABASE
conn = sqlite3.connect(dbpath)
c = conn.cursor()

### CREATE TABLE 1 time, no need if database and table already created
# c.execute(commandCreateTable)

### Insert a row of data
# c.execute(commandInsert)

####SEARCH DATA
for cc in c.execute(commandSearch):
    res.append(cc)
print()
#Droping table if already exists = delete table
# c.execute("DROP TABLE tableName")

conn.commit()
conn.close()

#-----------------------------------------

# Assign your output to the OUT variable.
# OUT = res
# print('delete table')