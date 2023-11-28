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
    Dictionary<PickableObject, byte> pickablesInArea = new Dictionary<PickableObject, byte>();

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
    private HandPoseControl handPoseControl;
    private const float ACTION_THRESHOLD_UP = 0.9f;
    private const float ACTION_THRESHOLD_DOWN = 0.75f;
    private string handName = "Hand";
    PickableObject objectInHand;
    private GameUIVR gameUIVR;
    private ActionTrigger.TriggerHandAction currentHandPose = ActionTrigger.TriggerHandAction.None;
    float maxPickupDistance = 0.05f;
    public Transform controllerTransform;
    float gripValue = 0f;
    float triggerValue = 0f;
    float previousGripValue = 0f;
    float previousTriggerValue = 0f;
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
        foreach (var item in devices)
        {
            Debug.Log("@$$" + handName + ":" + item.name + item.characteristics + " " + Random.Range(0, 9999).ToString());
        }
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
        foreach (KeyValuePair<PickableObject, byte> p in pickablesInArea)
        {
            if (p.Key != null)
            {
                if (pinchPickup && !p.Key.pickupWithPinch)
                    continue;
                if (!p.Key.gameObject.activeInHierarchy)
                    continue;
                if (!findMounted && p.Key.transform.parent.tag == "MountingPoint")
                    continue;
                if (findMounted && p.Key.transform.parent.tag != "MountingPoint")
                    continue;
                if (findMounted)
                {
                    Transform moundetTo = p.Key.transform.parent.transform.parent;
                    if (moundetTo != null
                        && moundetTo.GetComponent<PickableObject>() != null
                        && player.GetHandWithThisObject(moundetTo.gameObject) == null)
                        continue;
                }
                float nextDist = Vector3.Distance(transform.position, p.Key.transform.position);
                if (nextDist < dist)
                {
                    dist = nextDist;
                    closest = p.Key;
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
        if (pickableObject != null)
        {
            if (pickablesInArea.ContainsKey(pickableObject))
            {
                pickablesInArea[pickableObject]++;
            }
            else
            {
                pickablesInArea.Add(pickableObject, 1);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        if (pickableObject != null)
        {
            if (pickablesInArea.ContainsKey(pickableObject))
            {
                if (--pickablesInArea[pickableObject] <= 0)
                {
                    pickablesInArea.Remove(pickableObject);
                }
            }
        }
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

            // not totally arbitrary
            // if PickUpObject is called from somewhere outside, we need to set pose (grip = default)
            // but if it's called from Update() in this script, it's gonna overwrite pose anyways so it's fine
            currentHandPose = ActionTrigger.TriggerHandAction.Grip;

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
            currentHandPose = ActionTrigger.TriggerHandAction.None;
        }
    }


    private void CastAction(ActionTrigger.TriggerHandAction triggerAction)
    {
        foreach (ActionTrigger a in GameObject.FindObjectsOfType<ActionTrigger>())
        {
            a.ReceveTriggerAction(handModelPrefab.GetComponent<HandPoseData>().handType ==
                HandPoseData.HandModelType.Left, triggerAction);
        }
    }

    public ActionTrigger.TriggerHandAction GetCurrentHandPose()
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

        // Handling picking/dropping with grip/pinch

        // potato fix: for some reason, currentHandPose can be not-None while objectInHand doesnt exist
        // no clue how and why, but let's just double check and force state
        if (objectInHand == null)
        {
            currentHandPose = ActionTrigger.TriggerHandAction.None;
        }

        // Grip value update
        if (gripValue > ACTION_THRESHOLD_UP && previousGripValue < ACTION_THRESHOLD_UP) // picking
        {
            // if we didn't have anything in hand
            if (currentHandPose == ActionTrigger.TriggerHandAction.None)
            {
                if (TryToPickUp())
                {
                    currentHandPose = ActionTrigger.TriggerHandAction.Grip;
                }
                else
                {
                    // what's this for?
                    CastAction(ActionTrigger.TriggerHandAction.Grip);
                }
            }
            // if we were pinch-holding
            else if (currentHandPose == ActionTrigger.TriggerHandAction.Pinch)
            {
                // if we were holding an item in a pinch, let's switch to grip completely
                // TODO: implement multiple holding poses and switch them here
                currentHandPose = ActionTrigger.TriggerHandAction.Grip;
            }
        }
        // dropping from grip
        else if (currentHandPose == ActionTrigger.TriggerHandAction.Grip && gripValue < ACTION_THRESHOLD_DOWN)
        {
            // you know what, cloth is weird, let's do a lower threshold just for cloth 
            if (objectInHand.name != "clothIn" || gripValue < 0.5f ) // sorry for using value here
            {
                DropObjectFromHand();
            }
        }


        // Trigger value update
        if (triggerValue > ACTION_THRESHOLD_UP && previousTriggerValue < ACTION_THRESHOLD_UP) // picking
        {
            // if we didn't have anything in hand
            if (currentHandPose == ActionTrigger.TriggerHandAction.None)
            {
                bool flag = TryToPickUp(true);
                if (flag)
                {
                    currentHandPose = ActionTrigger.TriggerHandAction.Pinch;
                }
                else
                {
                    flag = TryToPickUp(false, true);
                    if (flag)
                        currentHandPose = ActionTrigger.TriggerHandAction.Pinch;
                }

                // ?
                if (!flag)
                    CastAction(ActionTrigger.TriggerHandAction.Pinch);
            }
            // if we were grip-holding
            else if (currentHandPose == ActionTrigger.TriggerHandAction.Grip)
            {
                // if we were holding an item in a grip, let's switch to pinch completely
                // TODO: implement multiple holding poses and switch them here
                // p.s. not everything can be picked up with pinch, check it here
                if (objectInHand != null && objectInHand.pickupWithPinch)
                {
                    currentHandPose = ActionTrigger.TriggerHandAction.Pinch;
                }
            }
        }
        // dropping from pinch
        else if (currentHandPose == ActionTrigger.TriggerHandAction.Pinch && triggerValue < ACTION_THRESHOLD_DOWN)
        {
            DropObjectFromHand();
        }

        // animation snapping i guess?
        if (objectInHand != null)
        {
            if (currentHandPose == ActionTrigger.TriggerHandAction.Grip)
            {
                handAnimator.SetFloat("Grip", 1f);
            }
            else if (currentHandPose == ActionTrigger.TriggerHandAction.Pinch)
            {
                handAnimator.SetFloat("Trigger", 1f);
            }
        }
        else
        {
            handAnimator.SetFloat("Grip", gripValue);
            handAnimator.SetFloat("Trigger", triggerValue);
        }

        previousGripValue = gripValue;
        previousTriggerValue = triggerValue;
    }
}