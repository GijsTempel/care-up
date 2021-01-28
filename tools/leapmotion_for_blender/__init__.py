# Care Up Leap Motion Helper
# Version 0.1.0
# Copyright 2018 Shmorhun Vitalii
from time import sleep

bl_info = {
	"name": "Leap Motion for Blender",
	"author": "Vitalii Shmorhun",
	"version": (0, 9, 0),
	"blender": (2, 79, 0),
	"description": "Receve and decode data from Leap Motion device",
	"warning": "This addon is still in development.",
	"category": "Animation"
}
import bpy
import json
from mathutils import Vector
import mathutils

from ws4py.client.threadedclient import WebSocketClient
from bpy.props import (StringProperty,
                       BoolProperty,
                       IntProperty,
                       FloatProperty,
                       EnumProperty,
                       )
from bpy_extras import anim_utils
ws = None
data_struct_version = 2
frame = 0

# -------------------------  Panel  -------- Panel  ---------------------------------
# -----------------------------------------------------------------------------------
class ActionsPanel(bpy.types.Panel):
	bl_idname = "Leap_Panel"
	bl_space_type = 'VIEW_3D'
	bl_region_type = 'TOOLS'
	bl_category = "Care Up"
	bl_label = 'Leap Panel'

	def draw(self, context):
		global data_struct_version
		scn = bpy.context.scene
		global ws
		if ws == None:
			self.layout.operator("anim.connect_leap_motion", icon='FULLSCREEN_EXIT', text="Connect")
		else:
			self.layout.operator("anim.disconnect_leap_motion", icon='FULLSCREEN_ENTER', text="Disconnect")
		layout = self.layout
		row = layout.row()
		sub = row.column(align=True)
		if ws != None:
			if "Leap_Data_Version" in bpy.context.scene:
				if scn['Leap_Data_Version'] == data_struct_version:
					active_obj = ""
					if bpy.context.active_object:
						if bpy.context.active_object.name:
							active_obj = bpy.context.active_object.name
					sub.prop(scn, "LeapScale", text="scale")
					sub.label()

					Offset_Button_Text = "Offset by: " + scn['Leap_Data']['Offset_Obj']
					if scn['Leap_Data']['Offset_Obj'] == "":
						Offset_Button_Text = "Active as Offset"
					off_row = sub.row(align=True)
					set_offset = off_row.operator("anim.leap_data", text = Offset_Button_Text, icon='ARROW_LEFTRIGHT')
					set_offset.action = "offset"
					set_offset.str_data = active_obj

					remove_offset = off_row.operator("anim.leap_data", icon='X', text = '')
					remove_offset.action = "offset"
					remove_offset.str_data = ""

					ml_row = sub.row(align=True)
					marcer_left_label = "Left Marcer: " + scn['Leap_Data']['marcer_left']
					if not(scn['Leap_Data']['marcer_left'] in bpy.data.objects) and scn['Leap_Data']['marcer_left'] != "":
						marcer_left_label = "!! Invalid Left Marker !!"

					marcer_left = ml_row.operator("anim.leap_data", text = marcer_left_label, icon='TRIA_LEFT')
					marcer_left.action = "marcer_left"
					marcer_left.str_data = active_obj

					marcer_left_x = ml_row.operator("anim.leap_data", icon='X', text='')
					marcer_left_x.action = "marcer_left"
					marcer_left_x.str_data = ""

					ml_row = sub.row(align=True)
					marcer_right_label = "Right Marcer: " + scn['Leap_Data']['marcer_right']
					if not(scn['Leap_Data']['marcer_right'] in bpy.data.objects) and scn['Leap_Data']['marcer_right'] != "":
						marcer_right_label = "!! Invalid Right Marker !!"

					marcer_right = ml_row.operator("anim.leap_data", text = marcer_right_label, icon='TRIA_RIGHT')
					marcer_right.action = "marcer_right"
					marcer_right.str_data = active_obj

					marcer_right_x = ml_row.operator("anim.leap_data", icon='X', text='')
					marcer_right_x.action = "marcer_right"
					marcer_right_x.str_data = ""
					sub.label()


					tr_row = sub.row(align=True)
					tr_label = "Tracking Rig: " + scn['Leap_Data']['tracking_rig']
					if not (scn['Leap_Data']['tracking_rig'] in bpy.data.objects) and scn['Leap_Data']['tracking_rig'] != "":
						tr_label = "!! Invalid Tracking Rig !!"

					tr = tr_row.operator("anim.leap_data", text=tr_label, icon='POSE_HLT')
					tr.action = "tracking_rig"
					tr.str_data = active_obj

					tr_x = tr_row.operator("anim.leap_data", icon='X', text='')
					tr_x.action = "tracking_rig"
					tr_x.str_data = ""


					rr_row = sub.row(align=True)
					rr_label = "Record Rig: " + scn['Leap_Data']['recording_rig']
					if not (scn['Leap_Data']['recording_rig'] in bpy.data.objects) and scn['Leap_Data']['recording_rig'] != "":
						rr_label = "!! Invalid Tracking Rig !!"

					rr = rr_row.operator("anim.leap_data", text=rr_label, icon='POSE_HLT')
					rr.action = "recording_rig"
					rr.str_data = active_obj

					rr_x = rr_row.operator("anim.leap_data", icon='X', text='')
					rr_x.action = "recording_rig"
					rr_x.str_data = ""


					sub.label()
					if scn['Leap_Data']['to_record']:
						rec = sub.operator("anim.leap_data", text="Stop", icon='CANCEL')
						rec.action = "record"
					else:
						rec = sub.operator("anim.leap_data", text="Record", icon='COLOR')
						rec.action = "record"

					sub.label()
					tb_row = sub.row()

					emb = scn['Leap_Data']['bone_tool_mode'] != 1
					add_bone = tb_row.operator("anim.leap_data",text="",  icon='ZOOMIN', emboss=emb)
					add_bone.action = "tool_mode"
					add_bone.int_data = 1

					emb = scn['Leap_Data']['bone_tool_mode'] != 0
					add_bone = tb_row.operator("anim.leap_data",text="",  icon='ZOOMOUT', emboss=emb)
					add_bone.action = "tool_mode"
					add_bone.int_data = 0

					emb = scn['Leap_Data']['bone_tool_mode'] != 2
					add_bone = tb_row.operator("anim.leap_data", text="", icon='HAND', emboss=emb)
					add_bone.action = "tool_mode"
					add_bone.int_data = 2

					for k in range(2):
						pref0 = 'l'
						pref1 = 'left'
						if k == 0:
							sub.label(text="left")
						else:
							sub.label(text="right")
							pref0 = 'r'
							pref1 = 'right'
						for i in range(5):
							rl = sub.row(align=True)
							for j in range(4):
								if j == 3 and i == 4 and k == 0:
									fb = rl.label()

								elif j == 3 and i == 0 and k == 1:
									fb = rl.label()
								else:
									ii = 4 - i
									jj = j

									if (i == 4 and k == 0) or (i == 0 and k == 1):
										jj = j + 1

									if k == 1:
										ii = i
									_action = 'anchor_' + pref0
									if scn['Leap_Data']['bone_tool_mode'] == 2:
										_action = 'select_anchor_' + pref0
									ach_obj = ""
									if scn['Leap_Data']['bone_tool_mode'] == 1 and bpy.context.active_object:
										ach_obj = bpy.context.active_object.name

									button_icon = 'SMALL_TRI_RIGHT_VEC'
									b_name = scn['Leap_Data']['anch_' + pref1][str(ii)][str(jj)]

									if b_name in bpy.data.objects:
										button_icon = 'KEYTYPE_JITTER_VEC'
									elif b_name != "":
										button_icon = 'CANCEL'
									fb = rl.operator("anim.leap_data", text=" ", icon=button_icon)
									fb.action = _action
									fb.int_data = ii
									fb.int2_data = jj
									fb.str_data = ach_obj

				else:
					sub.label(text="Legacy data version")
			else:
				sub.label(text="No Leap Data Structure")
		else:
			sub.label(text="No connection to the device")

# -----------------------------------------------------------------------------------
# -----------------------------------------------------------------------------------

class ActionRec(bpy.types.Operator):
	bl_idname = "vit.rec_action"
	bl_label = "Record Actions"
	name = bpy.props.StringProperty()
	current_frame = -1

	def execute(self, context):
		scene = context.scene
		if 'recAction' not in scene:
			scene['recAction'] = True
		else:
			scene['recAction'] = not scene['recAction']

		b = True
		handler_list = bpy.app.handlers.frame_change_pre
		fin = len(handler_list)
		for idx, func in enumerate(reversed(handler_list)):
			if func.__name__ == 'recAction':
				handler_list.pop(fin - 1 - idx)
				b = False
		bpy.app.handlers.frame_change_pre.append(recAction)
		if b:
			scene['recAction'] = True
		return {'FINISHED'}


def recAction(scene):
	if scene['recAction']:
		print(bpy.context.scene.frame_current)

		# 	action = anim_utils.bake_action(
		# 		c_frame,
		# 		c_frame,
		# 		frame_step=1,
		# 		only_selected=True,
		# 		do_pose='Pose',
		# 		do_object='Object',
		# 		do_visual_keying=True,
		# 		do_constraint_clear=False,
		# 		do_parents_clear=False,
		# 		do_clean=True,
		# 		action=bpy.data.actions['0']
		# )
# -----------------------------------------------------------------------------------
# -----------------------------------------------------------------------------------
class TestBake(bpy.types.Operator):
	bl_idname = "anim.test_bake"
	bl_label = "Test Bake"

	def execute(self, context):
		objects = context.selected_editable_objects

		object_action_pairs = (
			[(obj, getattr(obj.animation_data, "action", None)) for obj in objects]
			if True else
			[(obj, None) for obj in objects]
		)
		c_frame = bpy.context.scene.frame_current

		action = anim_utils.bake_action(c_frame,
										c_frame,
										frame_step=1,
										only_selected=True,
										do_pose='Pose',
										do_object='Object',
										do_visual_keying=True,
										do_constraint_clear=False,
										do_parents_clear=False,
										do_clean=True,
										action=bpy.data.actions['0']
										)
		return {'FINISHED'}




class LeapConnect(bpy.types.Operator):
	bl_idname = "anim.connect_leap_motion"
	bl_label = "Connect Leap Motion"

	def execute(self, context):
		scn = bpy.context.scene
		bpy.ops.screen.animation_cancel()
		bpy.ops.anim.leap_data(action="create")

		global ws
		try:
			global ws
			ws = DummyClient('ws://127.0.0.1:6437/v7.json', protocols=['http-only', 'chat'])
			ws.connect()
			if 'Leap_Data' in scn:
				if 'to_record' in scn['Leap_Data']:
					scn['Leap_Data']['to_record'] = False
			self.report({'INFO'}, "Connected")
		except KeyboardInterrupt:
			ws.close()
		return {'FINISHED'}


class LeapDisconnect(bpy.types.Operator):
	bl_idname = "anim.disconnect_leap_motion"
	bl_label = "Disconnect Leap Motion"

	def execute(self, context):
		global ws
		bpy.ops.screen.animation_cancel()
		if (ws != None):
			ws.close()
			ws = None
		return {'FINISHED'}



class DummyClient(WebSocketClient):
	def opened(self):
		def data_provider():
			for i in range(1, 200, 25):
				yield "#" * i

		self.send(data_provider())

	def closed(self, code, reason=None):
		ws = None
		print("Closed down", code, reason)

	def leap_to_vec(self, co, scale_offset = False):
		scn = bpy.context.scene
		_vec = Vector((0.0, 0.0, 0.0))
		_vec.x = co[0]
		_vec.y = -co[2]
		_vec.z = co[1]
		if scale_offset:
			l_scale = scn.LeapScale
			offset_obj_name = scn['Leap_Data']['Offset_Obj']
			offset_vec = Vector((0, 0, 0))
			if offset_obj_name != "":
				if offset_obj_name in bpy.data.objects:
					offset_vec = bpy.data.objects[offset_obj_name].location


			return _vec * 0.01 * l_scale + offset_vec
		return _vec * 0.01

	def leap_rot(self, l_norm, vec=Vector((0,1,0))):
		norm = self.leap_to_vec(l_norm)
		n_quat = vec.rotation_difference(norm)
		return n_quat

	def received_message(self, m):
		if ws == None:
			return
		global frame
		data_dict = json.loads(m.data.decode("utf-8"))
		scn = bpy.context.scene
		l_scale = scn.LeapScale
		offset_obj_name = scn['Leap_Data']['Offset_Obj']
		offset_vec = Vector((0,0,0))
		if offset_obj_name != "":
			if offset_obj_name in bpy.data.objects:
				offset_vec = bpy.data.objects[offset_obj_name].location
		left_id = -1
		right_id = -1
		to_record = False

		hand_l = None
		hand_r = None
		if (scn['Leap_Data']['marcer_left'] in bpy.data.objects):
			hand_l = bpy.data.objects[scn['Leap_Data']['marcer_left']]
		if (scn['Leap_Data']['marcer_right'] in bpy.data.objects):
			hand_r = bpy.data.objects[scn['Leap_Data']['marcer_right']]
		if 'hands' in data_dict:
			for h in data_dict['hands']:
				if h['type'] == 'left':
					left_id = h['id']
				if h['type'] == 'right':
					right_id = h['id']

				current_hand_obj = hand_l
				if h['type'] == 'right':
					current_hand_obj = hand_r
				if current_hand_obj != None:
					# current_hand_obj.rotation_mode = 'QUATERNION'
					_dir = self.leap_to_vec(h['palmNormal'])
					_up = self.leap_to_vec(h['direction'])
					mat = mathutils.Matrix((_up.cross(_dir), _up, _dir))
					qu = mat.inverted().to_quaternion()
					current_hand_obj.rotation_quaternion = qu
					current_hand_obj.location = self.leap_to_vec(h['palmPosition'], True)
					if scn['Leap_Data']['to_record']:
						if scn['Leap_Data']['recording_rig'] in bpy.data.objects:
							if scn['Leap_Data']['tracking_rig'] in bpy.data.objects:
								to_record = True

			if 'pointables' in data_dict:
				for pointable in data_dict['pointables']:
					pref = 'left'
					if pointable['handId']  == right_id:
						pref = 'right'

					i = int(pointable["type"])
					j_list = ['carpPosition', 'mcpPosition', 'pipPosition', 'btipPosition']
					for j in range(4):
						o_name = scn['Leap_Data']['anch_' + pref][str(i)][str(j)]
						if o_name in bpy.data.objects:
							obj = bpy.data.objects[o_name]
							obj.location = self.leap_to_vec(pointable[j_list[j]], True)
							base = pointable['bases'][j]
							__forw = self.leap_to_vec(base[0])
							__up = self.leap_to_vec(base[1])
							__left = self.leap_to_vec(base[2])
							if pref == 'right':
								__left = -__left
							mat = mathutils.Matrix((__left, __up, __forw))
							qu = mat.inverted().to_quaternion()

							obj.rotation_mode = 'QUATERNION'
							obj.rotation_quaternion = qu


			if to_record and frame != bpy.context.scene.frame_current:
				frame = bpy.context.scene.frame_current
				a = bpy.data.objects[scn['Leap_Data']['tracking_rig']]
				b = bpy.data.objects[scn['Leap_Data']['recording_rig']]
				action = b.animation_data.action
				bones_list = [
					'IK_hand.L',
					'IK_hand.R',
					'handRotation.L',
					'handRotation.R',

					'LeftHandThumb1',
					'LeftHandThumb3',
					'LeftHandThumb2',
					'LeftHandIndex1',
					'LeftHandIndex2',
					'LeftHandIndex3',
					'LeftHandMiddle1',
					'LeftHandMiddle2',
					'LeftHandMiddle3',
					'LeftHandRing1',
					'LeftHandRing2',
					'LeftHandRing3',
					'LeftHandPinky1',
					'LeftHandPinky2',
					'LeftHandPinky3',

					'RightHandThumb1',
					'RightHandThumb2',
					'RightHandThumb3',
					'RightHandIndex1',
					'RightHandIndex2',
					'RightHandIndex3',
					'RightHandMiddle1',
					'RightHandMiddle2',
					'RightHandMiddle3',
					'RightHandRing1',
					'RightHandRing2',
					'RightHandRing3',
					'RightHandPinky1',
					'RightHandPinky2',
					'RightHandPinky3'
				]

				for _bone in bones_list:
					if _bone in b.pose.bones and  _bone in a.pose.bones:
						m = a.convert_space(a.pose.bones[_bone], a.pose.bones[_bone].matrix, 'POSE', 'LOCAL')
						if _bone == 'IK_hand.L' or _bone == 'IK_hand.R':
							b.pose.bones[_bone].location = m.to_translation()
							b.keyframe_insert(data_path='pose.bones["' + _bone + '"].location')
						else:
							b.pose.bones[_bone].rotation_quaternion = m.to_quaternion()
							b.keyframe_insert(data_path='pose.bones["' + _bone + '"].rotation_quaternion')

class LeapData(bpy.types.Operator):
	bl_idname = "anim.leap_data"
	bl_label = "Leap Data"
	action = bpy.props.StringProperty()
	str_data = bpy.props.StringProperty()
	int_data = bpy.props.IntProperty()
	int2_data = bpy.props.IntProperty()

	def execute(self, context):
		print(self.action)
		scn = bpy.context.scene
		if self.action == "create":
			self.create_data()
		elif self.action == "offset":
			scn['Leap_Data']['Offset_Obj'] = self.str_data
		elif self.action == "marcer_left":
			scn['Leap_Data']['marcer_left'] = self.str_data
			if scn['Leap_Data']['marcer_left'] in bpy.data.objects:
				bpy.data.objects[scn['Leap_Data']['marcer_left']].rotation_mode = 'QUATERNION'
		elif self.action == "marcer_right":
			scn['Leap_Data']['marcer_right'] = self.str_data
			if scn['Leap_Data']['marcer_right'] in bpy.data.objects:
				bpy.data.objects[scn['Leap_Data']['marcer_right']].rotation_mode = 'QUATERNION'
		elif self.action == "tracking_rig":
			scn['Leap_Data']['tracking_rig'] = self.str_data
		elif self.action == "recording_rig":
			scn['Leap_Data']['recording_rig'] = self.str_data

		elif self.action == "record":
			bpy.ops.screen.animation_cancel()
			scn['Leap_Data']['to_record'] = not (scn['Leap_Data']['to_record'])
			if scn['Leap_Data']['to_record']:
				bpy.context.scene.frame_start = 0
				bpy.context.scene.frame_end = 1000000
				bpy.ops.screen.animation_play()
				data_path_list = [
					'location',
					'rotation_quaternion'
				]
				b = bpy.data.objects[scn['Leap_Data']['recording_rig']]
				for bBone in b.data.bones:
					if bBone.select:
						for d in data_path_list:
							if d == 'location':
								if not (bBone.name == 'IK_hand.L' or bBone.name == 'IK_hand.R'):
									continue
							b.keyframe_insert(data_path='pose.bones["' + bBone.name + '"].' + d)
				bpy.data.actions[b.animation_data.action.name].use_fake_user = True

		elif self.action == "tool_mode":
			scn['Leap_Data']['bone_tool_mode'] = self.int_data
		elif self.action == "anchor_l":
			scn['Leap_Data']['anch_left'][str(self.int_data)][str(self.int2_data)] = self.str_data
		elif self.action == "anchor_r":
			scn['Leap_Data']['anch_right'][str(self.int_data)][str(self.int2_data)] = self.str_data

		elif self.action == "select_anchor_l":
			a_name = scn['Leap_Data']['anch_left'][str(self.int_data)][str(self.int2_data)]
			if a_name in bpy.data.objects:
				bpy.ops.object.select_all(action='DESELECT')
				bpy.context.scene.objects.active = bpy.data.objects[a_name]
				bpy.data.objects[a_name].select = True
		elif self.action == "select_anchor_r":
			a_name = scn['Leap_Data']['anch_right'][str(self.int_data)][str(self.int2_data)]
			if a_name in bpy.data.objects:
				bpy.ops.object.select_all(action='DESELECT')
				bpy.context.scene.objects.active = bpy.data.objects[a_name]
				bpy.data.objects[a_name].select = True
		return {'FINISHED'}

	def create_data(self):
		scn = bpy.context.scene
		to_create = not('Leap_Data' in scn)
		if scn.get('Leap_Data_Version', False) or scn.get('Leap_Data_Version') != data_struct_version:
			to_create = True

		global data_struct_version

		if to_create:
			bpy.types.Scene.LeapScale = bpy.props.FloatProperty(name="Leap Motion Tracking Scale", default=1.0)
			bpy.types.Scene.LM_OffsetObj = bpy.props.StringProperty(name="Leap Motion Tracking Offset Object", default="")

			update_data = False
			if not('Leap_Data' in scn):
				scn['Leap_Data'] = {}
				update_data = True

			Leap_Data = scn['Leap_Data']
			if update_data or not("Offset_Obj" in Leap_Data):
				Leap_Data['Offset_Obj'] = ""
			if update_data or not("bones" in Leap_Data):
				Leap_Data['bones'] = {}
			if update_data or not("marcer_left" in Leap_Data):
				Leap_Data['marcer_left'] = ""
			if update_data or not("marcer_right" in Leap_Data):
				Leap_Data['marcer_right'] = ""

			if update_data or not("tracking_rig" in Leap_Data):
				Leap_Data['tracking_rig'] = ""

			if update_data or not("recording_rig" in Leap_Data):
				Leap_Data['recording_rig'] = ""

			if update_data or not("to_record" in Leap_Data):
				Leap_Data['to_record'] = False

			if update_data or not("bone_tool_mode" in Leap_Data):
				Leap_Data['bone_tool_mode'] = 1

			if update_data or not("anch_left" in Leap_Data):
				Leap_Data['anch_left'] = {}
				for i in range(5):
					Leap_Data['anch_left'][str(i)] = {}
					for j in range(4):
						Leap_Data['anch_left'][str(i)][str(j)] = ""
			if update_data or not("anch_right" in Leap_Data):
				Leap_Data['anch_right'] = {}
				for i in range(5):
					Leap_Data['anch_right'][str(i)] = {}
					for j in range(4):
						Leap_Data['anch_right'][str(i)][str(j)] = ""
		scn['Leap_Data_Version'] = data_struct_version

def register():
	bpy.utils.register_module(__name__)

def unregister():
	if (ws != None):
		ws.close()
	bpy.ops.screen.animation_cancel()
	bpy.utils.unregister_module(__name__)
