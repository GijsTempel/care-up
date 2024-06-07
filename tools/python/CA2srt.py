#!/usr/bin/python
# -*- coding: utf-8 -*-
import sys
import os
from lxml import etree as ET

def frame_to_time_text(frame_value):
    sec = (frame_value / 24.0) % 60
    min = int(((frame_value / 24) - sec) / 60)
    return "00:" + str('{:02x}'.format(min)) + ":" + str('{:06.3f}'.format(sec)).replace('.',',')


fullpath = os. getcwd() + '/' + sys.argv[1]


ff = open(fullpath, 'r', encoding='utf-8')
data = ff.read()
ff.close()


tree = ET.ElementTree(ET.fromstring(bytes(data, encoding='utf-8')))
root = tree.getroot()
print(root.tag)
counter = 1
times = []
current_time_text = ""
prev_time_text = ""
current_frame_value = 0
titles = []
for node in root.xpath('//action'):
    prev_time_text = current_time_text
    current_frame_value = float(node.attrib['frame'])
    titles.append(node.attrib['title'])
    current_time_text = frame_to_time_text(current_frame_value)
    if counter > 1:
        times.append(prev_time_text + " --> " + current_time_text)
    counter += 1

prev_time_text = current_time_text
current_frame_value += 50
current_time_text = frame_to_time_text(current_frame_value)
times.append(prev_time_text + " --> " + current_time_text)

counter = 1
out_text = ""
for i in range(len(titles)):
    out_text += str(counter) + '\n' + times[i] + '\n' + titles[i] + '\n'
    counter += 1
print(out_text)

out_file = open(fullpath + '.srt', 'w')
out_file.writelines(out_text)
out_file.close()