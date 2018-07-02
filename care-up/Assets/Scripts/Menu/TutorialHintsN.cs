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
	Vector2 originalPos;
	Vector2 originalSize;
	public int IconPosition = 0;
	float screenRation;
	int iconPos = 0;

    void UpdateToScreenResolution()
    {
        res = new Vector2(Screen.width, Screen.height);
		screenRation = (float)Screen.width / (float)Screen.height;
		GetComponentInParent<CanvasScaler>().referenceResolution = new Vector2(1920f, 1920f / screenRation);
		screenCorrection = (float)Screen.width / 1920f;
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
		originalPos = new Vector2(267f, -176f);
		originalSize = new Vector2(452f, 300f);
    }


	public void SetSize(float x, float y)
	{
		RectTransform hintsBox = transform.Find("hintsBox").GetComponent<RectTransform>();
		float x_shift = originalPos.x + (x - originalSize.x) / 2f;
		float y_shift = originalPos.y - (y - originalSize.y) / 2f;
		hintsBox.anchoredPosition = new Vector2(x_shift, y_shift);
		hintsBox.sizeDelta = new Vector2(x, y);
		SetIconPosition(iconPos);

			//sizeDelta
	}
	public void ResetSize()
	{
		RectTransform hintsBox = transform.Find("hintsBox").GetComponent<RectTransform>();
		hintsBox.anchoredPosition = originalPos;
        hintsBox.sizeDelta = originalSize;
		SetIconPosition(iconPos);
	}

	public void SetIconPosition(int pos)
	{
		iconPos = pos;
		RectTransform Icon = transform.Find("hintsBox/RegisterProtocols/Arrow").GetComponent<RectTransform>();
		RectTransform hintsBox = transform.Find("hintsBox").GetComponent<RectTransform>();
		Vector2 hintSize = hintsBox.sizeDelta;
		switch (pos)
		{
			case 0:
				hintsBox.anchoredPosition = new Vector3(hintSize.x / 2f + 40, -hintSize.y / 2f - 15f, 0f);
				Icon.anchoredPosition = new Vector2(10f, -10f);
				break;
			case 1:
				hintsBox.anchoredPosition = new Vector3(-hintSize.x / 2f - 40, -hintSize.y / 2f - 15f, 0f);
				Icon.anchoredPosition = new Vector2(hintSize.x - 10f, -10f);
				break;
			case 2:
				hintsBox.anchoredPosition = new Vector3(-hintSize.x / 2f - 40, hintSize.y / 2f + 15f, 0f);
				Icon.anchoredPosition = new Vector2(hintSize.x - 10f, -hintSize.y + 10f);
				break;
			case 3:
				hintsBox.anchoredPosition = new Vector3(hintSize.x / 2f + 40, hintSize.y / 2f + 15f, 0f);

				Icon.anchoredPosition = new Vector2(10f, -hintSize.y + 10f);
				break;
		}

	}

    void Update()
    {
        Vector2 scr = new Vector2(Screen.width, Screen.height);
        if (res != scr)
        {
            UpdateToScreenResolution();
        }

#if UNITY_EDITOR
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
#endif

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
