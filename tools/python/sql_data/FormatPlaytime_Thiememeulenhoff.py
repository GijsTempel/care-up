#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import sys
import pprint

def load_users():
    content_full = []
    with open("user_list.csv", "r") as f:
        content_full = f.readlines()
        content_full = [x.strip() for x in content_full] 
    users = {}
    for c in content_full:
        current_line = c.replace('"', '').split(',')
        users[current_line[0]] = current_line[1]
    return users

def time_to_minutes(time_str, debug = False):
    splited_time = time_str.split(":")
    if (debug):
        print(splited_time)
    if len(splited_time) < 3:
        return 0
    days = 0
    hours = 0
    mins = 0
    try:
        days = int(splited_time[0].replace('"', ''))
    except ValueError:
        days = 0
    try:
        hours = int(splited_time[1].replace('"', ''))
    except ValueError:
        hours = 0
    try:
        mins = int(splited_time[2].replace('"', ''))
    except ValueError:
        mins = 0
    if (debug):
        print(days)
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

        content.append([current_line[1], time_to_minutes(current_line[2]), current_line[2], current_line[0].replace('"', '')])
    return content

def record_data(playtime_data, file_name, users):
    file = open(file_name + '.csv', 'w')
    ss = "User Name,Playtime in minutes\n"
    for p in playtime_data:
        print("_ ", p[3])
        if p[3] in users:
            ss += p[0] + "," + str(p[1]) + "," + p[2] +"\n"
    file.write(ss)
    file.close

def main():
    files_to_format = ['PlayTime']
    users = load_users()
    for u in users:
        print(u)
    for f in files_to_format:
        playtime_data = load_data(f)
        record_data(playtime_data, "Formated" + f, users)
main()
