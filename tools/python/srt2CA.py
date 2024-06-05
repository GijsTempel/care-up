#!/usr/bin/python
# -*- coding: utf-8 -*-
import sys
import os
fullpath = os. getcwd() + '/' + sys.argv[1]

if '/' in sys.argv[1]:
	fullpath = ys.argv[1]
print(sys.argv[1])
file = open(fullpath, 'r')
lines = file.readlines()
print(os. getcwd())
count = 0
res = '<?xml version="1.0" encoding="utf-8" ?>\n<ActionList>\n'
act_open = '    <action\n'
act_close = '    ></action>\n'
title_t = '        title="'
descr_t = '        description=""\n'
frame_t = '        frame="'
current_line_index = 0
current_frame = 0
current_title = ""
for line in lines:
    # print(line)
    if count == 0:
        current_line_index = int(line.replace("\n", ""))
    elif count == 1:
         start_frame_time = line.split(" ")[0]
         current_frame = \
            int((float(start_frame_time.split(":")[1]) * 60.0 + \
            float(start_frame_time.split(":")[2].replace(',','.'))) * 24)
    elif count == 2:
         current_title = line.replace('\n', '')
    count += 1
    if count > 3:
        count = 0
        res += act_open + title_t + current_title + '"\n' + descr_t + frame_t + str(current_frame) + '"\n' + act_close
    
res += '</ActionList>'
out_file = open(fullpath + '.xml', 'w')
out_file.writelines(res)
out_file.close()