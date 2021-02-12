#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import sys
import pprint

def time_to_minutes(time_str):
    splited_time = time_str.split(":")
    if len(splited_time) < 3:
        return 0
    days = 0
    hours = 0
    mins = 0
    try:
        days = int(splited_time[0])
    except ValueError:
        days = 0
    try:
        hours = int(splited_time[1])
    except ValueError:
        hours = 0
    try:
        mins = int(splited_time[2])
    except ValueError:
        mins = 0
    total_mins = days * 1440 + hours * 60 + mins
    return total_mins


def load_data(file_name):
    content_full = []
    with open(file_name + ".csv", "r") as f:
        content_full = f.readlines()
        content_full = [x.strip() for x in content_full] 
    content = []
    users_data = {}
    for c in content_full:
        current_line = c.split(',')
        if len(current_line) < 3:
            continue
        content.append([current_line[1], time_to_minutes(current_line[2]), current_line[2]])
    return content

def record_data(playtime_data, file_name):
    file = open(file_name + '.csv', 'w')
    ss = "User Name,Playtime in minutes\n"
    for p in playtime_data:
        ss += p[0] + "," + str(p[1]) + "," + p[2] +"\n"
    file.write(ss)
    file.close

def main():
    files_to_format = ['PlayTime']
    for f in files_to_format:
        playtime_data = load_data(f)
        record_data(playtime_data, "Formated" + f)
main()