using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WalkToGroup : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;

    private GameObject text;

    CameraMode cameraMode;
    Controls controls;
    PlayerScript player;

    GameObject gameLogic;

    public void HighlightGroup(bool value)
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            return;
        text.SetActive(value);
    }

    private void Start()
    {
        gameLogic = GameObject.Find("GameLogic");

        cameraMode = gameLogic.GetComponent<CameraMode>();
        controls = gameLogic.GetComponent<Controls>();
        player = GameObject.FindObjectOfType<PlayerScript>();

        text = transform.GetChild(0).gameObject;
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            text.SetActive(false);
        }
    }

    protected void Update()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject == gameObject && !cameraMode.animating && player.away)
            {
                if (gameLogic.GetComponent<TutorialManager>() != null)
                    if (gameLogic.GetComponent<TutorialManager>().TutorialEnding)
                        return;
                HighlightGroup(true);
            }
            else
            {
                HighlightGroup(false);
            }
        }
    }
}