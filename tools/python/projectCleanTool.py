import os.path
import pathlib
BASE_DIR = os.path.dirname(os.path.realpath(__file__))
log_files = []
base_dir_fixed = ""

def build_used_file_list(list_path, format_index = 0):
    file1 = open(list_path, 'r')
    Lines = file1.readlines()
    count = 0
    for line in Lines:
        if len(line) > 0:
            if line[0] == " ":
                split_line = line.split(" ")
                file_path = ""
                for i in range(4, len(split_line)):
                    file_path += split_line[i]
                    if i < (len(split_line) - 1):
                        file_path += " "
                file_path = file_path.replace("/", "\\")
                file_path = file_path.replace("\n", "")
                full_path = base_dir_fixed + file_path
                if os.path.isfile(full_path):
                    #print(full_path)
                    if not full_path in log_files:
                        log_files.append(full_path)
        count += 1


base_dir_split = BASE_DIR.split('\\')
for i in range(len(base_dir_split)):
    if i < len(base_dir_split) - 2:
        base_dir_fixed += base_dir_split[i] + "\\"
base_dir_fixed += "care-up\\"
build_used_file_list('careup_WebGL_build_log01.txt')
build_used_file_list('careup_Android_build_log01.txt')
build_used_file_list('organizer_files_log.txt')
print(len(log_files))

dir_path = pathlib.Path(base_dir_fixed + "Assets//")
counter = 0
for item in list(dir_path.rglob("*")):
    if item.is_file():
        if not str(item).replace(".meta", "") in log_files:
            
            print(str(item))
            counter += 1
print(counter)
