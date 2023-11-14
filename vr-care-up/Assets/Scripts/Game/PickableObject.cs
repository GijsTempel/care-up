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

    private Rigidbody rigidbody = null;

    // required time with no movement to store transform
    private static float stored_time_needed = 2f;
    // set to time_needed to store transform immediately
    private float stored_current_timer = stored_time_needed;
    // temporary memory to check for movement
    private Vector3 stored_temp_position = Vector3.zero;
    private Quaternion stored_temp_rotation = Quaternion.identity;
    // actual stored position when dropped on the "ground"
    private Vector3 stored_position = Vector3.zero;
    private Quaternion stored_rotation = Quaternion.identity;
    // flag to avoid transform saving during teleport and/or multiple teleports
    private bool teleport_transition_flag = false;

    // time in s, from the moment object enters Trigger zone
    // till the moment it is back at stored transform
    private static float teleport_speed = 1f;

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
        VRCollarHolder vRCollarHolder = GameObject.FindObjectOfType<VRCollarHolder>();
        if (vRCollarHolder != null)
            vRCollarHolder.CloseTutorialShelf();
        if (deleteOnDrop)
            Destroy(gameObject);

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
        VRCollarHolder vRCollarHolder = GameObject.FindObjectOfType<VRCollarHolder>();
        if (vRCollarHolder != null)
            vRCollarHolder.CloseTutorialShelf();
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

        // transform storage for dropping
        if (!teleport_transition_flag) // flag to avoid storing position during teleport animation
        {
            stored_current_timer += Time.deltaTime;

            // save transform if enough time passed
            if (stored_current_timer > stored_time_needed)
            {
                stored_position = transform.position;
                stored_rotation = transform.rotation;
                stored_current_timer = 0;
            }

            // order matters, save first to instantly save position when object activates
            // otherwise, it's possible to drop before saving at all

            // reset timer if the object is moving
            if (transform.position != stored_temp_position ||
                transform.rotation != stored_temp_rotation)
            {
                stored_current_timer = 0;
            }

            // store transform to check next frame
            stored_temp_position = transform.position;
            stored_temp_rotation = transform.rotation;
        }
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

        if (!TryGetComponent<Rigidbody>(out rigidbody))
        {
            Debug.LogWarning("Somehow no rigidbody on pickable Object?");
        }
    }

    public IEnumerator OnItemDroppedOnGround(GameObject _dParticles, GameObject _aParticles, GameObject _line)
    {
        if (teleport_transition_flag)
        {
            // we're already teleporting, stop coroutine
            // reason is most likely that pickable child object exists
            yield break;
        }

        teleport_transition_flag = true;

        // trigger tutorial to explain item teleportation?
        //

        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        RigidbodyConstraints constraints = rigidbody.constraints;
        if (rigidbody == null)
        {
            Debug.LogWarning("PickableObject missing rigidbody.");
            yield break; // stop coroutine, cannot continue without rigidbody
        }
        else
        {
            // nullify velocity and temporarily freeze
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        // start animating disappearance? 
        // emit particles during teleportation time?
        if (_dParticles != null)
        {
            GameObject dParticles = Instantiate<GameObject>(_dParticles,
                transform.position, Quaternion.Euler(-90f, 0f, 0f)) as GameObject;
            GameObject.Destroy(dParticles, teleport_speed);
        }

        // possibly a line between _from_ and _to_
        // for a second for the teleportation time?
        if (_line != null)
        {
            GameObject line = Instantiate<GameObject>(_line) as GameObject;
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            Vector3[] points = { 
                new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                new Vector3(stored_position.x, stored_position.y, stored_position.z) 
            };
            lineRenderer.SetPositions(points);
            GameObject.Destroy(line, teleport_speed + 1f);
        }

        yield return new WaitForSeconds(teleport_speed);

        // finally teleport an object
        transform.position = stored_position;
        transform.rotation = stored_rotation;
        rigidbody.constraints = constraints;

        // animate appearance ?
        // burst of particles (firework-like) for a frame?
        if (_aParticles != null)
        {
            GameObject aParticles = Instantiate<GameObject>(_aParticles,
                transform.position, Quaternion.identity) as GameObject;
            GameObject.Destroy(aParticles, 3f);
        }

        teleport_transition_flag = false;
    }
}
