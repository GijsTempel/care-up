using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TestHint : MonoBehaviour {
	public GameObject WorldObject;
	public Vector3 offset;
	Camera cam;
	float screenCorrection = 1f;

	// Use this for initialization
	void Start () {
		screenCorrection = Screen.height / 1080f;
		print(screenCorrection * 72f);
		cam = GameObject.Find("Camera").GetComponent<Camera>();
		GetComponentInParent<CanvasScaler>().fallbackScreenDPI = screenCorrection * 72f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("h"))
		{
			print("h key was pressed");
			WorldObject = Selection.activeGameObject;
			Selection.activeGameObject = gameObject;
			offset = new Vector3(0f, 0f, 0f);
		}
        
		if (WorldObject != null)
		{
			if (WorldObject.GetComponent<RectTransform>() == null)
			{
				Vector3 ObjWorldPos = WorldObject.transform.position;
				Vector3 ScreenPos = cam.WorldToScreenPoint(ObjWorldPos + offset) / screenCorrection;
				GetComponent<RectTransform>().anchoredPosition = ScreenPos;
			}
			else
			{
				print("addddddddddddddddddddddddddddddd");
				Vector3 UIElementPos = WorldObject.GetComponent<RectTransform>().position;
				GetComponent<RectTransform>().anchoredPosition = UIElementPos + offset;
				//RectTransformUtility.ScreenPointToWorldPointInRectangle(WorldObject.GetComponent<RectTransform>()
			}
		}
	}
}
