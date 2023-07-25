#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import os
import json
from lxml import etree as ET

work_dict_name = "test"
res_path = "../../../care-up/Assets/Resources/"
dict_root_path =  res_path + "Dictionaries/Dutch/"
dict_names = []
setOfDictionaries = []


def load_dict(dict_path):
    with open(dict_path, 'r') as file:
        j_data = json.load(file)
    print(j_data)
    return j_data


def save_dict(dict_index):
    json_object = json.dumps(setOfDictionaries[dict_index], indent = 4) 
    file = open(dict_names[dict_index] + ".json", "w")
    file.write(json_object)
    file.close()


def is_it_key(key):
    if (len(key) >= 3):
        if key[0] == "[" and key[-1] == "]":
            return True
    return False
 

def get_value_if_key(key):
    if is_it_key(key):
        return get_localization_value(key)
    return key


def get_localization_value(key):
    for i in range(len(setOfDictionaries)):
        if (key in setOfDictionaries[i]):
            return setOfDictionaries[i][key]
    return ""
    

def find_dict_and_key_by_value(value):
    for i in range(len(setOfDictionaries)):
        for k in setOfDictionaries[i]:
            if setOfDictionaries[i][k] == value:
                return dict_names[i], k

for file_path in os.listdir(dict_root_path):
    spl = file_path.split('.')
    if (len(spl) == 2 and spl[1] == "json"):
        # print(file_path)
        dict_names.append(spl[0])
        print("\n" + spl[0])
        setOfDictionaries.append(load_dict(dict_root_path + file_path))

save_dict(1)


needed_attr = ["description", "fullDescription", "messageTitle", "messageContent", "isInProducts"]
node_names = ["action", "scene"]

tree = ET.parse('Actions_WoundCare.xml')
root = tree.getroot()

for n in node_names:
    for node in tree.iter(node_names):
        for k in node.attrib:
            if k in needed_attr:
                pass
                node.attrib[k] = str(100)

ET.indent(tree, '  ')

# xml_str = ET.tostring(tree, encoding='unicode', pretty_print=True).replace("\t", "  ")
# new_text = ""
# buffer_text = ""
# for i in range(len(xml_str)):
#     c = xml_str[i]
#     buffer_text += c
#     if c == ' ':
#         new_text += buffer_text
#         buffer_text = "" 
#     if c == "=":
#         buffer_text = "\n      " + buffer_text

# new_text += buffer_text
# file = open("output.xml", "w")
# file.write(new_text)
# file.close()

