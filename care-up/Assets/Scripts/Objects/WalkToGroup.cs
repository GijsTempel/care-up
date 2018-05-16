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
    ParticleSystem particles;

    public void HighlightGroup(bool value)
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            return;
        text.SetActive(value);

        text.transform.rotation = Camera.main.transform.rotation;

        if (particles != null)
        {
            particles.enableEmission = value;
        }
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

        particles = GetComponent<ParticleSystem>();
        if (particles != null) particles.enableEmission = false;
    }

    protected void Update()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject == gameObject && !cameraMode.animating /*&& (player.away || player.freeLook)*/)
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