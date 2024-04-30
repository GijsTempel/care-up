#/bin/bash
blender36 -b "$@" --python-expr "import bpy 
bpy.ops.catools.list_objects_to_file()"
