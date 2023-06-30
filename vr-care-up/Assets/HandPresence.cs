using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    List<PickableObject> pickablesInArea = new List<PickableObject>();

    private Animator handAnimator;
    public bool showController = false;
    public GameObject handModelPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
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


    public GameObject GetObjectInHand()
    {
        if (objectInHand != null)

            return objectInHand.gameObject;
        return null;
    }

    public bool IsLeftHand()
    {
        return spawnHandModel.GetComponent<HandPoseData>().handType == HandPoseData.HandModelType.Left;
    }

    private ActionTrigger.TriggerHandAction currentHandPose = ActionTrigger.TriggerHandAction.None;
    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        handName = transform.parent.name.Split(" ")[0];
        foreach (var item in devices)
        {
            // Debug.Log(handName + ":" + item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnController = Instantiate(prefab, transform);
            }
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

    private PickableObject FindClosestPickableInArea()
    {
        float dist = float.PositiveInfinity;
        PickableObject closest = null;
        foreach(PickableObject p in pickablesInArea)
        {
            if (p != null)
            {
                float nextDist = Vector3.Distance(transform.position, p.transform.position);
                if (nextDist < dist)
                {
                    dist = nextDist;
                    closest = p;
                }
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("@ % " + name + ":" + collision.name);
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        if (pickableObject != null && !(pickablesInArea.Contains(pickableObject)))
        {
            pickablesInArea.Add(pickableObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        if (pickableObject != null && (pickablesInArea.Contains(pickableObject)))
        {
            pickablesInArea.Remove(pickableObject);
        }
    }

    private bool TryToPickUp()
    {
        if (objectInHand != null)
            return false;
        if (player == null)
            return false;
        if (handPoseControl == null)
            return false;
        if (handPoseControl.handPoseMode != HandPoseControl.HandPoseMode.Default)
            return false;
        PickableObject closestPickable = FindClosestPickableInArea();
        if (closestPickable == null)
            return false;
        if (PickUpObject(closestPickable))
        {
            return true;
        }

        return false;
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
            return true;
        }
        return false;
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
            return true;
        }
        return false;
    }

    public void DropObjectFromHand(bool noPoseChange = false)
    {
        if (objectInHand == null)
            return;
        if (handPoseControl == null)
            return;
        if (handPoseControl.handPoseMode == HandPoseControl.HandPoseMode.CopyAnimIn)
            return;
        if (!noPoseChange)
        {
            GrabHandPose grabHandPose = objectInHand.GetComponent<GrabHandPose>();
            if (grabHandPose != null && spawnHandModel != null)
                grabHandPose.UnSetPose(spawnHandModel.GetComponent<HandPoseData>());
        }
        objectInHand.Drop();

        objectInHand = null;
    }


    private void CastAction(ActionTrigger.TriggerHandAction triggerAction)
    {
        foreach(ActionTrigger a in GameObject.FindObjectsOfType<ActionTrigger>())
        {
            a.ReceveTriggerAction(handModelPrefab.GetComponent<HandPoseData>().handType ==
                HandPoseData.HandModelType.Left, triggerAction);
        }
    }

    public ActionTrigger.TriggerHandAction GetCurrentHandPose()
    {
        return currentHandPose;
    }

    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                if (gripValue > ACTION_TRESHOULD_UP && gripSavedValue <= ACTION_TRESHOULD_UP)
                {
                    if (!TryToPickUp())
                        CastAction(ActionTrigger.TriggerHandAction.Grip);
                }
                else if (gripValue < ACTION_TRESHOULD_UP && gripSavedValue >= ACTION_TRESHOULD_UP)
                {
                    DropObjectFromHand();
                }

                if (gripValue > ACTION_TRESHOULD_UP)
                    currentHandPose = ActionTrigger.TriggerHandAction.Grip;
                else
                    currentHandPose = ActionTrigger.TriggerHandAction.None;

                gripSavedValue = gripValue;
                if (objectInHand != null)
                {
                    handAnimator.SetFloat("Grip", 1f);
                }
                else
                {
                    handAnimator.SetFloat("Grip", gripValue);
                }
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                handAnimator.SetFloat("Trigger", triggerValue);
                if (triggerValue > ACTION_TRESHOULD_UP && triggerSavedValue <= ACTION_TRESHOULD_UP)
                {
                    CastAction(ActionTrigger.TriggerHandAction.Pinch);
                }
                if (triggerValue > ACTION_TRESHOULD_UP && currentHandPose != ActionTrigger.TriggerHandAction.Grip)
                    currentHandPose = ActionTrigger.TriggerHandAction.Pinch;

                triggerSavedValue = triggerValue;
            }
            Debug.Log("@HandPose_" + name + ":" + currentHandPose.ToString());

            if (spawnController != null)
            {
                spawnController.SetActive(showController);
            }
            spawnHandModel.SetActive(!showController);
        }
    }
}