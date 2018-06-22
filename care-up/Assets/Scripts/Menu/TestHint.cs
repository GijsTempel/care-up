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
	public string ScriptCommand;
	Vector2 res;

	// Use this for initialization

	void UpdateToScreenResolution()
	{
		res = new Vector2(Screen.width, Screen.height);
        screenCorrection = Screen.height / 1080f;
        GetComponentInParent<CanvasScaler>().fallbackScreenDPI = screenCorrection * 72f;
	}

	void Start () {
		cam = GameObject.Find("Camera").GetComponent<Camera>();
		UpdateToScreenResolution();
	}


	
	// Update is called once per frame
	void Update () {
		Vector2 scr = new Vector2(Screen.width, Screen.height);
		if (res != scr)
		{
			UpdateToScreenResolution();
			print("rescaleeeeeeeeeeeeeeee");
		}
		if (Input.GetKeyDown("h"))
		{
			GameObject sel = Selection.activeGameObject;
			if (sel != null)
			{
				if (sel != gameObject)
				{
					WorldObject = sel;
					Selection.activeGameObject = gameObject;
					offset = new Vector3(0f, 0f, 0f);
				}
			}
		}
        
		if (WorldObject != null)
		{
			if (WorldObject.GetComponent<RectTransform>() == null)
			{
				Vector3 ObjWorldPos = WorldObject.transform.position;
				Vector3 ScreenPos = cam.WorldToScreenPoint(ObjWorldPos + offset) / screenCorrection;
				GetComponent<RectTransform>().anchoredPosition = ScreenPos;
				ScriptCommand = "toWorldPosition" + (ObjWorldPos + offset).ToString() + ";";
			}
			else
			{
				if (WorldObject.GetComponentInParent<Canvas>().renderMode.ToString() == "WorldSpace")
				{
					GameObject canv = WorldObject.GetComponentInParent<Canvas>().gameObject;
					print(canv.GetComponent<RectTransform>().position);
					Vector3 ObjWorldPos = WorldObject.GetComponent<RectTransform>().TransformPoint(WorldObject.GetComponent<RectTransform>().position + offset); 
                    Vector3 ScreenPos = cam.WorldToScreenPoint(ObjWorldPos) / screenCorrection;
					GetComponent<RectTransform>().anchoredPosition = ScreenPos;
				}
				else
				{

					float canvasScale = WorldObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().localScale.x;
					Vector3 UIElementPos = WorldObject.GetComponent<RectTransform>().position;
					GetComponent<RectTransform>().anchoredPosition = (UIElementPos + offset * canvasScale) / screenCorrection;
				}
			}
		}
	}
}
