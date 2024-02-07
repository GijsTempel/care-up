using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands.Samples.VisualizerSample;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Composites;
using UnityEngine.XR.Interaction.Toolkit.Samples.Hands;


public class HandPresence : MonoBehaviour
{
    List<PickableObject> pickablesInArea = new List<PickableObject>();
    public List<GameObject> triggerColliderObjects = new List<GameObject>();

    public PokeGestureDetector gestureDetector;
    private Animator handAnimator;
    public bool showController = false;
    public GameObject handModelPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
    private InputDevice handTrackingDevice;
    private GameObject spawnController;
    private GameObject spawnHandModel;
    private PlayerScript player;
    private float triggerSavedValue = 0f;
    private float gripSavedValue = 0f;
    private HandPoseControl handPoseControl;
    private const float ACTION_TRESHOULD_UP = 0.9f;
    private const float ACTION_TRESHOULD_DOWN = 0.8f;
    private string handName = "Hand";
    PickableObject objectInHand;
    private GameUIVR gameUIVR;
    bool allowTriggerDrop = false;
    float maxPickupDistance = 0.05f;
    public Transform controllerTransform;
    float gripValue = 0f;
    float triggerValue = 0f;
    Transform trackingHandTransfom = null;
    public HandVisualizer handVisualizer;

    public HandPoseControl GetHandPoseControl()
    {
        return handPoseControl;
    }

    public GameObject GetObjectInHand()
    {
        if (objectInHand != null)

            return objectInHand.gameObject;
        return null;
    }

    public bool IsLeftHand()
    {
        return tag == "LeftHand";
    }

    private ActionModule_ActionTrigger.TriggerHandAction currentHandPose = ActionModule_ActionTrigger.TriggerHandAction.None;
    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
        gameUIVR = GameObject.FindObjectOfType<GameUIVR>();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        handName = transform.parent.name.Split(" ")[0];
        // foreach (var item in devices)
        // {
        //     Debug.Log("@$$" + handName + ":" + item.name + item.characteristics + " " + Random.Range(0, 9999).ToString());
        // }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
                spawnController = Instantiate(prefab, transform);
        }
        if (spawnHandModel == null)
        {
            spawnHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnHandModel.transform.Find("Hand").GetComponent<Animator>();
        }
        if (spawnHandModel != null)
        {
            player = GameObject.FindObjectOfType<PlayerScript>();
            if (player != null)
            {
                player.AddHandPoseControl(spawnHandModel.GetComponent<HandPoseControl>(), IsLeftHand());
                spawnHandModel.GetComponent<HandPoseControl>().animHandsTransform = player.animHandsTransform;
                player.AddHandPresence(IsLeftHand(), this);
                handPoseControl = spawnHandModel.GetComponent<HandPoseControl>();
            }
        }
    }

    void HideHand(bool toHide)
    {
        foreach (SkinnedMeshRenderer s in transform.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            s.enabled = !toHide;
        foreach (BoxCollider b in transform.GetComponentsInChildren<BoxCollider>(true))
        {
            if (!triggerColliderObjects.Contains(b.gameObject))
                b.enabled = !toHide;
        }
    }


    void HideTrackingOrControlHand(bool toHideControlHand, bool toForce = false)
    {
        if (!toForce)
        {
            if (objectInHand == null)
                toHideControlHand = true;
            if (player.IsInCopyAnimationState())
                toHideControlHand = false;
        }
        HideHand(toHideControlHand);
        if (handVisualizer == null)
            return;
        if (IsLeftHand())
            handVisualizer.HideLeft(!toHideControlHand);
        else
            handVisualizer.HideRight(!toHideControlHand);
    }

    private PickableObject FindClosestPickableInArea(bool findMounted = false, bool pinchPickup = false)
    {
        float dist = float.PositiveInfinity;
        PickableObject closest = null;
        List<PickableObject> pInArea = pickablesInArea;
        if (trackingHandTransfom != null
                && trackingHandTransfom.gameObject.activeSelf
                && trackingHandTransfom.parent.parent.GetComponent<TrackingHandPickupControl>() != null)
            pInArea = trackingHandTransfom.parent.parent.GetComponent<TrackingHandPickupControl>().pickablesInArea;


        foreach (PickableObject p in pInArea)
        {
            if (p != null)
            {
                if (pinchPickup && !p.pickupWithPinch)
                    continue;
                if (!p.gameObject.activeInHierarchy)
                    continue;
                if (!findMounted && p.transform.parent.tag == "MountingPoint")
                    continue;
                if (findMounted && p.transform.parent.tag != "MountingPoint")
                    continue;
                if (findMounted)
                {
                    Transform moundetTo = p.transform.parent.transform.parent;
                    if (moundetTo != null
                        && moundetTo.GetComponent<PickableObject>() != null
                        && player.GetHandWithThisObject(moundetTo.gameObject) == null)
                        continue;
                }
                float nextDist = Vector3.Distance(transform.position, p.transform.position);
                if (nextDist < dist)
                {
                    dist = nextDist;
                    closest = p;
                }
            }
        }
        if (closest == null)
        {
            foreach (PickableObject p in GameObject.FindObjectsOfType<PickableObject>())
            {
                if (p != null)
                {
                    if (pinchPickup && !p.pickupWithPinch)
                        continue;
                    if (!p.gameObject.activeInHierarchy)
                        continue;
                    if (Vector3.Distance(transform.position, p.transform.position) > maxPickupDistance)
                        continue;
                    if (!findMounted && p.transform.parent.tag == "MountingPoint")
                        continue;
                    if (findMounted && p.transform.parent.tag != "MountingPoint")
                        continue;
                    if (findMounted)
                    {
                        Transform moundetTo = p.transform.parent.transform.parent;
                        if (moundetTo != null
                            && moundetTo.GetComponent<PickableObject>() != null
                            && player.GetHandWithThisObject(moundetTo.gameObject) == null)
                            continue;
                    }
                    float nextDist = Vector3.Distance(transform.position, p.transform.position);
                    if (nextDist < dist)
                    {
                        dist = nextDist;
                        closest = p;
                    }
                }
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        if (pickableObject != null && !(pickablesInArea.Contains(pickableObject)))
        {
            pickablesInArea.Add(pickableObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        RemoveObjectFromArea(pickableObject);
    }


    private void RemoveObjectFromArea(PickableObject pickableObject)
    {
        if (pickableObject != null && (pickablesInArea.Contains(pickableObject)))
            pickablesInArea.Remove(pickableObject);
    }

    private bool TryToPickUp(bool findMounted = false, bool pinchPickup = false)
    {
        if (objectInHand != null)
            return false;
        if (player == null)
            return false;
        if (handPoseControl == null)
            return false;
        if (handPoseControl.handPoseMode != HandPoseControl.HandPoseMode.Default)
            return false;
        PickableObject closestPickable = FindClosestPickableInArea(findMounted, pinchPickup);
        if (closestPickable == null)
            return false;
        if (findMounted)
        {
            bool toPinchPickup;
            if (closestPickable.pinchPickupTrigger != null && closestPickable.pinchPickupTrigger.gameObject.activeInHierarchy)
            {
                toPinchPickup = closestPickable.pinchPickupTrigger.AttemptTrigger();
            }
            else
            {
                toPinchPickup = true;
            }
            if (toPinchPickup)
                return PickupMountedObject(closestPickable);
            else
                return false;
        }
        else
            return PickUpObject(closestPickable);
    }


    public bool PickUpObject()
    {
        if (spawnHandModel == null)
            return false;

        bool isPickedUp = objectInHand.PickUp(handPoseControl.objectHolder, 0.02f);

        if (isPickedUp)
        {
            GrabHandPose grabHandPose = objectInHand.GetComponent<GrabHandPose>();
            if (grabHandPose != null && spawnHandModel != null)
                grabHandPose.SetupPose(spawnHandModel.GetComponent<HandPoseData>());
            if (gameUIVR != null)
                gameUIVR.UpdateHelpWitDelay(0.1f);
            return true;
        }
        return false;
    }

    public bool PickupMountedObject(PickableObject objToPickup)
    {
        Transform holder = GameObject.FindWithTag("ItemHolder").transform;
        objToPickup.transform.SetParent(holder);

        return PickUpObject(objToPickup);
    }
    public bool PickUpObject(PickableObject objToPickup, bool toForce = false)
    {
        if (spawnHandModel == null)
            return false;

        if (objToPickup.gameObject == player.GetObjectInHand(!IsLeftHand()))
        {
            player.DropFromHand(!IsLeftHand());
        }
        bool isPickedUp = objToPickup.PickUp(handPoseControl.objectHolder, 0.02f);

        if (isPickedUp)
        {
            objectInHand = objToPickup;
            if (!toForce)
            {
                GrabHandPose grabHandPose = objectInHand.GetComponent<GrabHandPose>();
                if (grabHandPose != null && spawnHandModel != null)
                    grabHandPose.SetupPose(spawnHandModel.GetComponent<HandPoseData>());
            }
            if (!toForce && gameUIVR != null)
                gameUIVR.UpdateHelpWitDelay(0.1f);
            return true;
        }
        return false;
    }

    public void DropObjectFromHand(bool noPoseChange = false, bool toForce = false)
    {
        if (!toForce && player.IsInCopyAnimationState())
            return;
        if (objectInHand == null)
            return;
        if (handPoseControl == null)
            return;
        if (!toForce && handPoseControl.handPoseMode == HandPoseControl.HandPoseMode.CopyAnimIn)
            return;
        if (!noPoseChange)
        {
            GrabHandPose grabHandPose = objectInHand.GetComponent<GrabHandPose>();
            if (grabHandPose != null && spawnHandModel != null)
                grabHandPose.UnSetPose(spawnHandModel.GetComponent<HandPoseData>());
        }
        //RemoveObjectFromArea(objectInHand);
        if (objectInHand.Drop())
        {
            objectInHand = null;
            if (gameUIVR != null)
                gameUIVR.UpdateHelpWitDelay(0.1f);
        }
    }


    private void CastAction(ActionModule_ActionTrigger.TriggerHandAction triggerAction)
    {
        foreach (ActionModule_ActionTrigger a in GameObject.FindObjectsOfType<ActionModule_ActionTrigger>())
        {
            a.ReceveTriggerAction(handModelPrefab.GetComponent<HandPoseData>().handType ==
                HandPoseData.HandModelType.Left, triggerAction);
        }
    }

    public ActionModule_ActionTrigger.TriggerHandAction GetCurrentHandPose()
    {
        return currentHandPose;
    }

    public void FollowTrackingHands()
    {
        if (trackingHandTransfom == null)
            return;
        if (handPoseControl.handPoseMode != HandPoseControl.HandPoseMode.CopyAnimIn)
        {
            transform.position = trackingHandTransfom.position;
            transform.rotation = trackingHandTransfom.rotation;
            if (objectInHand != null)
            {
                objectInHand.UpdateFallowPos();
            }
        }
    }

    Transform FindTrackingTransform(bool _isLeft)
    {
        foreach(HTrackingHand h in GameObject.FindObjectsOfType<HTrackingHand>())
        {
            if (_isLeft && h.tag == "LeftHand")
                return h.controllerHandParentTransform;
            else if (!_isLeft && h.tag == "RightHand")
                return h.controllerHandParentTransform;
        }
        return null;
    }

    void Update()
    {
        bool isLeftHand = IsLeftHand();
        bool isTracking = false;
        if (isLeftHand)
            isTracking = HandVisualizer.left_handIsTracked;
        else
            isTracking = HandVisualizer.right_handIsTracked;
        if (isTracking && trackingHandTransfom == null)
            trackingHandTransfom = FindTrackingTransform(isLeftHand);


        if (isTracking)
        {
            gripValue = gestureDetector.gripValue;
            triggerValue = gestureDetector.triggerValue;
            HideTrackingOrControlHand(gripValue < 1 && triggerValue < 1);
        }
        else
        {
            transform.position = controllerTransform.position;
            transform.rotation = controllerTransform.rotation;
            if (objectInHand != null)
                objectInHand.UpdateFallowPos();
            if (!targetDevice.isValid)
                TryInitialize();
            else
            {
                HideTrackingOrControlHand(false, true);
                if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float newGripValue))
                    gripValue = newGripValue;

                if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float newTriggerValue))
                    triggerValue = newTriggerValue;


                if (spawnController != null)
                {
                    spawnController.SetActive(showController);
                }
                spawnHandModel.SetActive(!showController);
            }
        }


        // Grip value update
        bool pickedUp = false;

        if (gripValue > ACTION_TRESHOULD_UP && gripSavedValue <= ACTION_TRESHOULD_UP)
        {
            if (!TryToPickUp())
                CastAction(ActionModule_ActionTrigger.TriggerHandAction.Grip);
            else
                pickedUp = true;
        }
        else if (gripValue < ACTION_TRESHOULD_UP && gripSavedValue >= ACTION_TRESHOULD_UP)
        {
            DropObjectFromHand();
            allowTriggerDrop = false;
        }

        if (gripValue > ACTION_TRESHOULD_UP)
            currentHandPose = ActionModule_ActionTrigger.TriggerHandAction.Grip;
        else
            currentHandPose = ActionModule_ActionTrigger.TriggerHandAction.None;

        gripSavedValue = gripValue;
        if (objectInHand != null)
        {
            handAnimator.SetFloat("Grip", 1f);
        }
        else
        {
            handAnimator.SetFloat("Grip", gripValue);
        }

        // Trigger value update
        handAnimator.SetFloat("Trigger", triggerValue);
        if (triggerValue > ACTION_TRESHOULD_UP && triggerSavedValue <= ACTION_TRESHOULD_UP)
        {
            CastAction(ActionModule_ActionTrigger.TriggerHandAction.Pinch);
            if (TryToPickUp(true))
                allowTriggerDrop = true;
            else if (!pickedUp)
            {
                if (TryToPickUp(false, true))
                    allowTriggerDrop = true;
            }
        }
        else if (triggerValue < ACTION_TRESHOULD_UP && triggerSavedValue >= ACTION_TRESHOULD_UP)
        {
            if (objectInHand != null && objectInHand.pickupWithPinch)
                DropObjectFromHand();
            allowTriggerDrop = false;
        }

        if (triggerValue > ACTION_TRESHOULD_UP && currentHandPose != ActionModule_ActionTrigger.TriggerHandAction.Grip)
        {
            currentHandPose = ActionModule_ActionTrigger.TriggerHandAction.Pinch;
        }
        triggerSavedValue = triggerValue;
    }
}