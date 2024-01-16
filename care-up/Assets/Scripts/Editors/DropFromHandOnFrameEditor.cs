using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DropFromHandOnFrameV2))]
public class DropFromHandOnFrameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DropFromHandOnFrameV2 gui = (DropFromHandOnFrameV2)target;

        gui.hand = (DropFromHandOnFrameV2.Hand)EditorGUILayout.EnumPopup("Hand to drop", gui.hand);
        gui.dropFrame = EditorGUILayout.IntField("Drop to frame", gui.dropFrame);
        gui.dropToObject = EditorGUILayout.TextField("Drop to object", gui.dropToObject);
        if (!string.IsNullOrEmpty(gui.dropToObject))
        {
            gui.dropObjectAsChild = EditorGUILayout.Toggle("Drop object as child", gui.dropObjectAsChild);
        }
    }
}
