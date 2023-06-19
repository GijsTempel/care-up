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
    private string handName = "Hand";
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
                bool isRightHand = spawnHandModel.GetComponent<HandPoseData>().handType == HandPoseData.HandModelType.Right;
                player.AddHandPoseControl(spawnHandModel.GetComponent<HandPoseControl>(), isRightHand);
                spawnHandModel.GetComponent<HandPoseControl>().animHandsTransform = player.animHandsTransform;
            }
        }
    }

    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
                handAnimator.SetFloat("Trigger", triggerValue);

            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
                handAnimator.SetFloat("Grip", gripValue);
            spawnController.SetActive(showController);
            spawnHandModel.SetActive(!showController);
        }
    }
}