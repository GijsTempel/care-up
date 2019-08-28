#!/usr/bin/env python
import os, sys
base_folder = "../care-up/Assets/"
with open("folders_to_hide.txt") as f:
	folders_names = f.readlines()
	for folder in folders_names:
		folder_name = folder.replace("\n", "")
		if os.path.exists(base_folder + folder_name):
			os.rename(base_folder + folder_name,base_folder + "." + folder_name)
			print("++ Hide " + folder_name);
		else:
			print(folder_name + " not found")

