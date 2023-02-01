import os
file1 = open("files_to_delete.txt", 'r')
Lines = file1.readlines()
count = 0
for line in Lines:
	path = line[:-1]
	if os.path.exists(path):
		print(path)
		os.remove(path)
	if os.path.exists(path + ".meta"):
		pass
		os.remove(path + ".meta")
