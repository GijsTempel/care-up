using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TutorialHintsN : MonoBehaviour
{
    public GameObject WorldObject;
    public Vector3 offset;
	public bool FullPath = true;
    Camera cam;
    float screenCorrection = 1f;
    public string ScriptCommand;
    Vector2 res;

    void UpdateToScreenResolution()
    {
        res = new Vector2(Screen.width, Screen.height);
        screenCorrection = Screen.height / 1080f;
        GetComponentInParent<CanvasScaler>().fallbackScreenDPI = screenCorrection * 72f;
    }

	//get object path in hierarchy
    //--------------------------------------
	public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
	//--------------------------------------

	public void LockTo(string ObjectPath, Vector3 _offset)
	{
		WorldObject = GameObject.Find(ObjectPath);
		offset = _offset;
	}

    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        UpdateToScreenResolution();
    }



    void Update()
    {
        Vector2 scr = new Vector2(Screen.width, Screen.height);
        if (res != scr)
        {
            UpdateToScreenResolution();
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
			string path = WorldObject.name;
			if (FullPath)
			{
				path = GetGameObjectPath(WorldObject);
			}
            string x = (offset.x.ToString("f").Replace(',', '.') + "f,");
            string y = (offset.y.ToString("f").Replace(',', '.') + "f,");
            string z = (offset.z.ToString("f").Replace(',', '.') + "f");
			ScriptCommand = ("hintsN.LockTo(\"" + (path).ToString() + "\", new Vector3(" + x+y+z + "));");
            if (WorldObject.GetComponent<RectTransform>() == null)
            {
                Vector3 ObjWorldPos = WorldObject.transform.position;
                Vector3 ScreenPos = cam.WorldToScreenPoint(ObjWorldPos + offset) / screenCorrection;
                GetComponent<RectTransform>().anchoredPosition = ScreenPos;
            }
            else
            {
                if (WorldObject.GetComponentInParent<Canvas>().renderMode.ToString() == "WorldSpace")
                {
                    GameObject canv = WorldObject.GetComponentInParent<Canvas>().gameObject;
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
