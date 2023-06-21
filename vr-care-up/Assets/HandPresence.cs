using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
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
    private const float ACTION_TRESHOULD = 0.9f;
    private string handName = "Hand";

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
            Debug.Log(spawnHandModel.name);
            if (player != null)
            {
                bool isLeftHand = spawnHandModel.GetComponent<HandPoseData>().handType == HandPoseData.HandModelType.Left;
                player.AddHandPoseControl(spawnHandModel.GetComponent<HandPoseControl>(), isLeftHand);
                spawnHandModel.GetComponent<HandPoseControl>().animHandsTransform = player.animHandsTransform;
                player.AddHandPresence(isLeftHand, this);
            }
        }
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
                if (gripValue > ACTION_TRESHOULD && gripSavedValue <= ACTION_TRESHOULD)
                {
                    CastAction(ActionTrigger.TriggerHandAction.Grip);
                }

                if (gripValue > ACTION_TRESHOULD)
                    currentHandPose = ActionTrigger.TriggerHandAction.Grip;
                else
                    currentHandPose = ActionTrigger.TriggerHandAction.None;

                gripSavedValue = gripValue;
                handAnimator.SetFloat("Grip", gripValue);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                handAnimator.SetFloat("Trigger", triggerValue);
                if (triggerValue > ACTION_TRESHOULD && triggerSavedValue <= ACTION_TRESHOULD)
                {
                    CastAction(ActionTrigger.TriggerHandAction.Pinch);
                }
                if (triggerValue > ACTION_TRESHOULD && currentHandPose != ActionTrigger.TriggerHandAction.Grip)
                    currentHandPose = ActionTrigger.TriggerHandAction.Pinch;

                triggerSavedValue = triggerValue;
            }
            Debug.Log("@HandPose_" + name + ":" + currentHandPose.ToString());

            spawnController.SetActive(showController);
            spawnHandModel.SetActive(!showController);

            //Check trigger actions
            
        }
    }
}