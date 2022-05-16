#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import sys

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

def load_data(file_name):
    content_full = []
    scenes = []
    with open(file_name + ".csv", "r") as f:
        content_full = f.readlines()
        content_full = [x.strip() for x in content_full] 
    content = []
    users_data = {}
    for c in content_full:
        current_line = c.split('","')
        if len(current_line) < 5:
            continue
        for ii in range(len(current_line)):
            current_line[ii] = current_line[ii].replace('"', '').replace(',','.')
        content.append(current_line)
        scene_name = current_line[3].replace("score_", "")
        if scene_name not in scenes:
            if "stars_" not in scene_name:
                scenes.append(scene_name)
        if current_line[0] not in users_data:
            users_data[current_line[0]] = {}
        
        if scene_name not in users_data[current_line[0]]:
            users_data[current_line[0]][scene_name] = current_line[4]
    return scenes, users_data

def record_data(scenes, users_data, users, file_name):
    file = open(file_name + '.csv', 'w')
    ss = "User Name,"
    for s in scenes:
        ss += s + ","
    ss += "\n"
    for u in users_data.keys():
        if users_data[u]:
            if u in users:
                _name = u
                ss += _name + ","
                for s in scenes:
                    value = ""
                    if s in users_data[u]:
                        value = str(int(float(users_data[u][s])))
                    ss += value + ","
                ss += "\n"
    file.write(ss)
    file.close

def main():
    files_to_format = ['TestHighscores', "PracticeHighscores"]
    users = load_users()

    for f in files_to_format:
        scenes, users_data = load_data(f)
        record_data(scenes, users_data, users, "Formated" + f)

main()