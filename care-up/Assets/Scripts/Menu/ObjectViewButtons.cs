using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectViewButtons : MonoBehaviour {

    CameraMode cameraMode;

	// Use this for initialization
	void Start () {

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode");
        else
        {
            transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => cameraMode.ObjectViewPutDownButton());
        }
	}
    private void OnEnable()
    {
        if (GameObject.FindObjectOfType<GameUI>().AllowAutoPlay(false))
        {
            Invoke("AutoClose", 1f);

        }
    }

    void AutoClose()
    {
        if (gameObject.activeSelf)
        {
            if (transform.Find("Close") != null)
                transform.Find("Close").GetComponent<Button>().onClick.Invoke();
        }
    }
}
