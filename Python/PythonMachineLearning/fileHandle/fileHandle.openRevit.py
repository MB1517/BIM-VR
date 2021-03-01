import os




#open revit
filename = "C:\\Users\\USER\\Documents\PythonMachineLearning\\pythonFromZero\\Project1.rvt"
fd = os.system("start "+filename)
print ("OPEN the file successfully!!")
#close revit
os.close(fd)
print ("CLOSE the file successfully!!")


# remove file
# cur_file = "file1.txt"
# os.remove(cur_file)

# rename files 
# cur_file = "file1.txt"
# new_file = "file2.txt"
# os.rename(cur_file, new_file)