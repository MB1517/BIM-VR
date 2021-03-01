#print('Hello')
"""
mode:
"r" - Read - Default value. Opens a file for reading, error if the file does not exist

"a" - Append - Opens a file for appending, creates the file if it does not exist

"w" - Write - Opens a file for writing, creates the file if it does not exist

"x" - Create - Creates the specified file, returns an error if the file exists
"""
#Open file
f = open("readFile.txt","r")
print(f.read())

#read first characters 
#print(f.read(10))

#read line or lines
#print(f.readlines())
#print(f.readline())

#read line by line
# for l in f:
#     print(l)

#close file
f.close()

# By using "with" statement is the safest way to handle a file operation in Python because "with" statement ensures that the file is closed when the block inside with is exited. 
# with open("my_file.txt", "r") as my_file:
   # do some file operations