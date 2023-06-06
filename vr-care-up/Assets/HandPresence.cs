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
            Debug.Log(handName + ":" + item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log(targetDevice.name);
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnController = Instantiate(prefab, transform);
            }
            spawnHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnHandModel.transform.Find("Hand").GetComponent<Animator>();
        }
    }

    // Update is called once per frame
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

            // targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            // Debug.Log(handName + " PrimPress:" + primaryButtonValue.ToString());
            // targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            // if (triggerValue > 0.1f)
            // {
            //     Debug.Log(handName + " triggerValue:" + triggerValue.ToString());
            // }
            // targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
            // if (primary2DAxisValue != Vector2.zero)
            // {
            //     Debug.Log(handName + " primary2DAxisValue:" + primary2DAxisValue.ToString());
            // }
            spawnController.SetActive(showController);
            spawnHandModel.SetActive(!showController);

        }
    }
}