import module1
import os
import sys
import clr
clr.AddReference('System')
import System

pathRevitAPI = ""
sys.path.append(pathRevitAPI) # append Path for Add reference
# clr.AddReference('RevitAPI')
# module1.sayHello()
# print os.getcwd()

path = os.path.abspath(module1.__file__) # get module Path
print (path)