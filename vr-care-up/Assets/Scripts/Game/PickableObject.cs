using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    private bool savedIsKinematic = false;
    private bool savedUseGravity = true;
    private PlayerScript player;
    private Transform transformToFallow;
    [Header("Pinch pickup and mount")]
    public bool pickupWithPinch = false;
    public ActionModule_ActionTrigger pinchPickupTrigger;
    public ActionModule_ActionTrigger pinchMountTrigger;
    float outlineUpdateTimeout = 0f;
    bool toUpdateOutline = false;

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

    // line animation stuff
    private bool line_animating_flag = false;
    private float line_timer = 0f;
    private Vector3 line_og_position = Vector3.zero;
    private LineRenderer line_ref = null;
    public bool noOutline = false;


    public PickableObject GetParentPickable()
    {
        PickableObject result = null;
        if (transform.parent != null)
            result = transform.parent.gameObject.GetComponentInParent<PickableObject>();
        return result;
    }

    public bool IsMounted()
    {
        if (transform.parent.tag == "MountingPoint")
            return true;
        return false;
    }

    private MountDetector GetMountInChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MountDetector>() != null)
                return transform.GetChild(i).GetComponent<MountDetector>();
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
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ActionModule_DropCondition>()) 
            {
                if (!transform.GetChild(i).GetComponent<ActionModule_DropCondition>().Check())
                    return false;
            }
        }
        VRCollarHolder vRCollarHolder = GameObject.FindObjectOfType<VRCollarHolder>();
        if (vRCollarHolder != null)
            vRCollarHolder.CloseTutorialShelf();
        if (deleteOnDrop)
            Destroy(gameObject);

        ShowViewElements(false);
        transformToFallow = null;
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = savedIsKinematic;
            gameObject.GetComponent<Rigidbody>().useGravity = savedUseGravity;
        }

        MountDetector mountDetector = GetMountInChildren();
        if (mountDetector != null)
        {
            Transform closestMount = mountDetector.FindClosestMount();
            if (closestMount != null)
            {
                if (!AttatchToMount(closestMount))
                    return true;
            }
        }
        if (dropAnchor != null)
        {
            transform.position = dropAnchor.position;
            transform.rotation = dropAnchor.rotation;
            FallowTransform(dropAnchor);
        }

        //Trigger an object detector module, to check, if needed object is out of hand now
        foreach (ActionModule_ObjectDetector objectDetector in GameObject.FindObjectsOfType<ActionModule_ObjectDetector>())
            objectDetector.AttemptTrigger();
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
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            savedUseGravity = gameObject.GetComponent<Rigidbody>().useGravity;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
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

    public void UpdateFallowPos()
    {
        if (transformToFallow == null)
            return;
 
        // before: just lerpValue for pickUp transition, where an objects goes from 
        // from wherever it was lying at to the hand anchor during transition duration
        // after: if that transition ended, i'll smooth out the follow to the hand
        float lerpValue = routineTime / poseTransitionDuration;

        // we're after the picking transition!
        if (lerpValue > 1 && name == "Tablet") // was using for tablet only initially
        {
            // calculate new lerp value, the further we are from the hand, the bigger the value
            // the closer we are to the follow transform - the lower the value
            float dist = Vector3.Distance(transformToFallow.position, transform.position) * 1000f;
            // quadratic graph, at 0.02 distance lerp = 1, anything above just snaps as before
            // at 0.01 value (which is what i was getting while trying to hold still) - lerp = 0.25
            lerpValue = dist * dist * 0.0025f; 

            // calculate and assign new pos for smoothing - from "currentPosition" to "Follow"
            transform.position = Vector3.Lerp(transform.position, transformToFallow.position, lerpValue);
            transform.rotation = Quaternion.Lerp(transform.rotation, transformToFallow.rotation, lerpValue);
        }
        else
        {
            // calculate and assign new pos for transition - from "Start" to "Follow"
            transform.position = Vector3.Lerp(startPos, transformToFallow.position, lerpValue);
            transform.rotation = Quaternion.Lerp(startRotation, transformToFallow.rotation, lerpValue);
        }
    }

    public void InitiateOutlineUpdate(float timeout)
    {
        outlineUpdateTimeout = timeout;
        toUpdateOutline = true;
    }

    private void Update()
    {
        if (toUpdateOutline)
        {
            outlineUpdateTimeout -= Time.deltaTime;
            if (outlineUpdateTimeout <= 0)
            {
                UpdateOutline();
                toUpdateOutline = false;
            }
        }
        UpdateFallowPos();
        routineTime += Time.deltaTime;
        // transform storage for dropping
        if (player != null && !teleport_transition_flag && // flag to avoid storing position during teleport animation
                !player.IsAnimatorPaused) // to avoid storing position during paused animSequence
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

        // handle line animation variables
        HandleLineAnimationStep();
    }

    private void UpdateOutline()
    {
        if (GetComponent<Outline>() != null)
            GetComponent<Outline>().ComputeOutline();
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
        PickableObject newParentPickable = mount.GetComponentInParent<PickableObject>();
        string ss = ("@ mount " + name + ": ");

        if (newParentPickable != null)
        {
            ss += newParentPickable.name;
            newParentPickable.InitiateOutlineUpdate(0.1f);
        }
        Debug.Log(ss);

        // if (GetComponent<Outline>() != null)
        //     GetComponent<Outline>().ComputeOutline();

        return true;
    }

    void Awake()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            savedIsKinematic = gameObject.GetComponent<Rigidbody>().isKinematic;
            if (transform.parent != null && transform.parent.tag == "MountingPoint")
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public void GenerateOutline() 
    {
        Outline outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent(typeof(Outline)) as Outline;
            outline.OutlineWidth = 3f;
            if (pickupWithPinch)
                outline.OutlineColor = Color.yellow;
            outline.enabled = false;
        }
        if (GetComponent<OutilneControl>() == null)
        {
            OutilneControl outilneControl = gameObject.AddComponent(typeof(OutilneControl)) as OutilneControl;
            outilneControl.outlines = new List<Outline>();
            outilneControl.outlines.Add(outline);
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        ShowViewElements(false);
        if (dropAnchor != null)
            FallowTransform(dropAnchor);
        GenerateOutline();
    }

    bool TeleportationRayTest(Vector3 pos)
    {
        const int layerMask = 0b100000001001;
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit phit, 5f, layerMask))
        {
            if (phit.collider.GetComponent<DropControl>() != null)
                return true;
        }
        return false;
    }

    Vector3 FindTeleportationPosition(Vector3 dropPos)
    {
        dropPos.y += 0.1f;
        if (!TeleportationRayTest(dropPos))
        {
            foreach(DropControl dropControl in GameObject.FindObjectsOfType<DropControl>())
            {
                foreach(Vector3 point in dropControl.GetPoints())
                {
                    if (TeleportationRayTest(point))
                        return point;
                }
            }
        }
        return dropPos;
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

        Vector3 newPos = FindTeleportationPosition(stored_position);
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
            // line_ref = line.GetComponent<LineRenderer>();
            // line_og_position = transform.position;
            // line_animating_flag = true;

            // handle bg line quickly
            LineRenderer bgLine = line.transform.GetChild(0).GetComponent<LineRenderer>();
            float teleportDist = Vector3.Distance(transform.position, newPos);
            float stepBaseDist = 0.1f;
            int steps = (int)((float)teleportDist / stepBaseDist);
            // float topOffset = Mathf.Abs(newPos.y - transform.position.y) + 0.5f;
            List<Vector3> pointsList = new List<Vector3>();
            for(int i = 0; i < steps; i++)
            {
                float factor = (float)i / (float)steps;
                float wideFactor = ActionManager.Remap(factor, 0f, 1f, -1f, 1f);
                Vector3 currentPoint = Vector3.Lerp(transform.position, newPos, factor);
                currentPoint.y += 1f * (1f - wideFactor * wideFactor);
                pointsList.Add(currentPoint);
            }
            pointsList.Add(newPos);
            Vector3[] points = pointsList.ToArray();
            // {
            //     new Vector3(transform.position.x, transform.position.y, transform.position.z),
            //     new Vector3(newPos.x, newPos.y, newPos.z)
            // };
            bgLine.positionCount = pointsList.Count;
            bgLine.SetPositions(points);

            GameObject.Destroy(line, teleport_speed * 1.5f);
        }

        yield return new WaitForSeconds(teleport_speed);

        // finally teleport an object
        transform.position = newPos;
        transform.rotation = stored_rotation;
        rigidbody.constraints = constraints;

        // animate appearance ?
        // burst of particles (firework-like) for a frame?
        if (_aParticles != null)
        {
            GameObject aParticles = Instantiate<GameObject>(_aParticles,
                transform.position, Quaternion.Euler(-90f, 0f, 0f)) as GameObject;
            GameObject.Destroy(aParticles, 3f);
        }

        teleport_transition_flag = false;
    }

    public void HandleLineAnimationStep()
    {
        if (!line_animating_flag)
            return;

        line_timer += Time.deltaTime;

        if (line_timer > teleport_speed)
        { // if we're here, object is teleported
            line_animating_flag = false;
            line_timer = 0f;
            return;
        }

        // calculate starting and ending positions of the line
        // use line_timer as the center percentage (p_timer) 
        // and size as an offset from that center to start/end points
        // upd. addint third middle point for "curve-ish" look of the line
        const float line_size = 0.1f;
        float p_timer = line_timer / teleport_speed;
        float s_timer = p_timer - line_size;
        float e_timer = p_timer + line_size;
        Vector3 sPos = Vector3.Lerp(line_og_position, stored_position, s_timer);
        Vector3 mPos = Vector3.Lerp(line_og_position, stored_position, p_timer);
        Vector3 ePos = Vector3.Lerp(line_og_position, stored_position, e_timer);
        Vector3[] points = {
                new Vector3(sPos.x, sPos.y, sPos.z),
                new Vector3(mPos.x, mPos.y, mPos.z),
                new Vector3(ePos.x, ePos.y, ePos.z)
            };
        line_ref.SetPositions(points);
    }
}
