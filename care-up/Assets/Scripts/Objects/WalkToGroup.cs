using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WalkToGroup : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    
    public Vector3 robotPosition;
    public Vector3 robotRotation;

    private GameObject text;
    public GameObject cone;
    public bool ButtonHovered = false;

    public WalkToGroup LeftWalkToGroup = null;
    public WalkToGroup RightWalkToGroup = null;

    public enum GroupType
    {
        NotSet,
        WorkField,
        Doctor,
        Patient,
        Sink
    };

    public WalkToGroup.GroupType WalkToGroupType;
    CameraMode cameraMode;
    Controls controls;

    GameObject gameLogic;
    ParticleSystem particles;

    private Transform target;

    public Vector3 Position
    {
        get { return (target == null) ? position : target.position; }
    }

    public Vector3 Rotation
    {
        get { return (target == null) ? rotation : target.rotation.eulerAngles; }
    }

    public void HighlightGroup(bool value)
    {
        Camera cam = null;
        foreach(Camera c in GameObject.FindObjectsOfType<Camera>())
        {
            if (c.transform.parent != null)
            {
                if (c.transform.parent.name == "Head")
                    cam = c;
            }
        }
        if (cam == null)
            return;
        if (SystemInfo.deviceType == DeviceType.Handheld)
            return;
        text.SetActive(value);
        if (cone != null)
            cone.SetActive(value);

        //text.transform.rotation = Camera.main.transform.rotation;

        if (particles != null)
        {
            ParticleSystem.EmissionModule emission = particles.emission;
            emission.enabled = value;
        }
    }


    public void SetTarget(Transform _target)
    {
        target = _target;
    }



    private void Start()
    {
        gameLogic = GameObject.Find("GameLogic");

        cameraMode = gameLogic.GetComponent<CameraMode>();
        controls = gameLogic.GetComponent<Controls>();

        text = transform.Find("TextMesh").gameObject;

        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            text.SetActive(false);
            if (cone != null)
                cone.SetActive(false);
        }

        particles = GetComponent<ParticleSystem>();
        if (particles != null)
        {
            ParticleSystem.EmissionModule emission = particles.emission;
            emission.enabled = false;
        }

        if (transform.Find("Target") != null)
        {
            target = transform.Find("Target").transform;
        }
        else
        {
            target = null;
        }
    }



    protected void Update()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject == gameObject && !cameraMode.animating || ButtonHovered/*&& (player.away || player.freeLook)*/)
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