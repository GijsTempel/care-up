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

    bool lastHighlightState = false;
    private GameObject text;
    public GameObject cone;
    public bool ButtonHovered = false;
    public string description;
    //[HideInInspector]
    public WalkToGroup LeftWalkToGroup = null;
    //[HideInInspector]
    public WalkToGroup RightWalkToGroup = null;
    PlayerScript player;
    public bool ManualNeighborhood = false; 

    public float interactionDistance = -1;

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
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            text.SetActive(player.away);
            cone.SetActive(false);
            return;
        }
     
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
        player = GameObject.FindObjectOfType<PlayerScript>();
        //FindNeighbors();   
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

    public void FindNeighbors()
    {
        Vector3 tVec = transform.forward;
        WalkToGroup closestLeft = null;
        WalkToGroup closestRight = null;
        foreach (WalkToGroup w in GameObject.FindObjectsOfType<WalkToGroup>())
        {
            if (w != this)
            {
                Vector3 direct = (transform.position - w.position).normalized;
                float wDot = Vector3.Dot(tVec, direct);
                if (wDot < 0)
                {
                    if (closestLeft == null)
                        closestLeft = w;
                    else
                    {
                        float currentDist = Vector3.Distance(closestLeft.transform.position, transform.position);
                        float candidateDist = Vector3.Distance(w.transform.position, transform.position);
                        if (candidateDist < currentDist)
                            closestLeft = w;
                    }
                }
                else
                {
                    if (closestRight == null)
                        closestRight = w;
                    else
                    {
                        float currentDist = Vector3.Distance(closestRight.transform.position, transform.position);
                        float candidateDist = Vector3.Distance(w.transform.position, transform.position);
                        if (candidateDist < currentDist)
                            closestRight = w;
                    }
                }

            }
        }
        if (!ManualNeighborhood)
        {
            LeftWalkToGroup = closestLeft;
            RightWalkToGroup = closestRight;
        }
    }

    protected void Update()
    {
        if (player.away)
        {
            bool value = false;
            if (cameraMode.CurrentMode == CameraMode.Mode.Free)
            {
                if (controls.SelectedObject == gameObject && !cameraMode.animating || ButtonHovered/*&& (player.away || player.freeLook)*/)
                {
                    if (gameLogic.GetComponent<TutorialManager>() != null)
                        if (gameLogic.GetComponent<TutorialManager>().TutorialEnding)
                            return;
                    value = true;
                }
                else
                {
                    value = false;
                }
            }
            if (lastHighlightState != value)
                HighlightGroup(value);
            lastHighlightState = value;
        }

    }
}