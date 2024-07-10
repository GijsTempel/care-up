'''
Copyright (C) 2017 VITALII SHMORHUN
3dvits@gmail.com

Created by VITALII SHMORHUN

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
'''

bl_info = {
    "name": "Care Up Tools",
    "description": "A small set of tools created during for on a CareUp project",
    "author": "Vitalii Shmorhun",
    "version": (0, 0, 3),
    "blender": (3, 6, 0),
    "location": "View3D",
    "warning": "This addon is still in development.",
    "wiki_url": "",
    "category": "Animation" }


import bpy
import pathlib

class CopyBoneTransform(bpy.types.Operator):
    bl_idname = "catools.copybonetrans"
    bl_label = "Copy Bone Transformation"
    bl_options = {'REGISTER', 'UNDO'}

    def execute(self, contex):

        armature = bpy.context.active_object
        if armature.type == 'ARMATURE':
            aBone = bpy.context.active_bone
            print(aBone.name)
            print("-----------")
            bpy.ops.object.mode_set(mode='POSE')
            selected = []
            for bone in armature.pose.bones:
                # print(bone.name)
                if armature.data.bones[bone.name].select:
                    selected.append(bone)
            for bone in selected:
                print(bone.name)
                if bone.name != aBone.name:
                    bone.matrix = armature.pose.bones[aBone.name].matrix
                    aBone.select = False
                    bpy.ops.anim.keyframe_insert_menu(type='LocRotScale')
        print("Copy___")
        return {'FINISHED'}


class InvertQuaternion(bpy.types.Operator):
    bl_idname = "catools.quatinv"
    bl_label = "Invert Bone Quaternion"

    def execute(self, contex):

        armature = bpy.context.active_object
        if armature.type == 'ARMATURE':
            aBoneName = bpy.context.active_bone.name
            aBone = armature.pose.bones[aBoneName]
            q = aBone.rotation_quaternion
            for i in range(4):
                aBone.rotation_quaternion[i] = -q[i]
                bpy.ops.anim.keyframe_insert_menu(type='Rotation')

        return {'FINISHED'}



class ListActionsToFile(bpy.types.Operator):
    bl_idname = "catools.list_actions_to_file"
    bl_label = "Care Up List Actions To File"

    def execute(self, contex):
        blend_path = pathlib.Path(bpy.data.filepath)
        folder_path = str(blend_path.parent)
        blend_filename = str(blend_path.name)
        anim_file_name = blend_filename + '.animlist'
        list_file = open(folder_path + '/' + anim_file_name, 'w')
        for a in bpy.data.actions:
            list_file.write(a.name + '\n')
        list_file.close()

        return {'FINISHED'}



class ListObjectsToFile(bpy.types.Operator):
    bl_idname = "catools.list_objects_to_file"
    bl_label = "Care Up List Objects To File"

    def execute(self, contex):
        blend_path = pathlib.Path(bpy.data.filepath)
        folder_path = str(blend_path.parent)
        blend_filename = str(blend_path.name)
        object_file_name = blend_filename + '.objectlist'
        list_file = open(folder_path + '/' + object_file_name, 'w')
        for o in bpy.data.objects:
            list_file.write(o.name + '\n')
        list_file.close()
        return {'FINISHED'}


def menu_func(self, context):
    self.layout.operator(InvertQuaternion.bl_idname)
    self.layout.operator(CopyBoneTransform.bl_idname)


def obj_menu_func(self, context):
    self.layout.operator(ListActionsToFile.bl_idname)
    self.layout.operator(ListObjectsToFile.bl_idname)


def register():
    bpy.utils.register_class(InvertQuaternion)
    bpy.utils.register_class(CopyBoneTransform)
    bpy.utils.register_class(ListActionsToFile)
    bpy.utils.register_class(ListObjectsToFile)

    bpy.types.VIEW3D_MT_pose.append(menu_func)
    bpy.types.VIEW3D_MT_object.append(obj_menu_func)


def unregister():
    bpy.utils.unregister_class(InvertQuaternion)
    bpy.utils.unregister_class(CopyBoneTransform)
    bpy.utils.unregister_class(ListActionsToFile)
    bpy.utils.unregister_class(ListObjectsToFile)


if __name__ == "__main__":
    register()
