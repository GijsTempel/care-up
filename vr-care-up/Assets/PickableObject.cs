using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;


public class PickableObject : MonoBehaviour
{
    public List<GameObject> inHandMeshes = new List<GameObject>();
    public List<GameObject> outHandMeshes = new List<GameObject>();
    public Transform dropAnchor;

    private float poseTransitionDuration = 0.2f;
    private float routineTime = float.PositiveInfinity;

    private Vector3 startPos;
    private Quaternion startRotation;
    public bool deleteOnDrop = false;


    bool isKinematic = false;
    private PlayerScript player;
    private Transform transformToFallow;
    [Header("Pinch pickup and mount")]
    public bool pickupWithPinch = false;
    public ActionTrigger pinchPickupTrigger;
    public ActionTrigger pinchMountTrigger;


    public bool IsMounted()
    {
        if (transform.parent.tag == "MountingPoint")
            return true;
        return false;
    }

    private MountDetector GetMountInChildren()
    {
        foreach(MountDetector m in gameObject.GetComponentsInChildren<MountDetector>())
        {
            if (m.transform.parent == transform)
                return m;
        }
        return null;
    }


    void ShowViewElements(bool inHand = true)
    {
        foreach(GameObject g in inHandMeshes)
            g.SetActive(inHand);
        foreach(GameObject g in outHandMeshes)
            g.SetActive(!inHand);
    }


    public bool Drop()
    {
        if (deleteOnDrop)
            Destroy(gameObject);

        Debug.Log("@ " + name + ": Drop");
        ShowViewElements(false);
        transformToFallow = null;
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = isKinematic;

        MountDetector mountDetector = GetMountInChildren();
        if (mountDetector != null)
        {
            Transform closestMount = mountDetector.FindClosestMount();
            if (closestMount != null)
            {
                if (!AttatchToMount(closestMount))
                    return false;
            }
        }
        if (dropAnchor != null)
        {
            transform.position = dropAnchor.position;
            transform.rotation = dropAnchor.rotation;
            FallowTransform(dropAnchor);
        }
        return true;
    }

    

    public bool PickUp(Transform handTransform, float transuitionDuration = 0.2f)
    {
        ShowViewElements(true);
        
        FallowTransform(handTransform, transuitionDuration);
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        return true;
    }

    public void FallowTransform(Transform trans, float transuitionDuration = 0.2f)
    {
        poseTransitionDuration = transuitionDuration;
        startPos = transform.position;
        startRotation = trans.rotation;
        routineTime = 0f;
        transformToFallow = trans;
    }

    private void Update()
    {
        float lerpValue = routineTime / poseTransitionDuration;
        if (transformToFallow != null)
        {
            Vector3 p = Vector3.Lerp(startPos, transformToFallow.position, lerpValue);
            Quaternion r = Quaternion.Lerp(startRotation, transformToFallow.rotation, lerpValue);

            transform.position = p;
            transform.rotation = r;

        }
        routineTime += Time.deltaTime;
    }

    bool AttatchToMount(Transform mount)
    {
        if (pinchMountTrigger != null && pinchMountTrigger.gameObject.activeInHierarchy)
            if (!pinchMountTrigger.AttemptTrigger())
                return false;
        transform.SetParent(mount);
        transform.position = mount.position;
        transform.rotation = mount.rotation;
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        return true;
    }

    void Awake()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            isKinematic = gameObject.GetComponent<Rigidbody>().isKinematic;
            if (transform.parent != null && transform.parent.tag == "MountingPoint")
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        ShowViewElements(false);
        if (dropAnchor != null)
            FallowTransform(dropAnchor);
    }
}
