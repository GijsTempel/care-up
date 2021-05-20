#!/usr/bin/env python
import os, sys
base_folder = "../care-up/Assets/"
with open("folders_to_hide.txt") as f:
	for folder in f.readlines():
		full_path = base_folder + folder.replace("\n", "")
		object_name = full_path.split("/")[-1]
		content_folder = full_path[0:len(full_path) - len(object_name)]
		hide_path = content_folder + "." + object_name

		a = full_path
		b = hide_path
		print_word = "++ Hide "

		if(len(sys.argv) > 1):
			if sys.argv[1] == 'u':
				a = hide_path
				b = full_path
				print_word = "-- Unhidden "

		if os.path.exists(a):
			os.rename(a,b)
			print(print_word + full_path);
		if os.path.exists(a + ".meta"):
			os.rename(a + ".meta",b + ".meta")
		else:
			print(" *  " + full_path + " not found")

