using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastProgressBar : MonoBehaviour
{
    public ActionCollider target;
    [Range(0f, 1f)]
    public float progressOffset = .3f;
    [Min(0f)]
    public float currentProgress;

    private UnityEngine.UI.Image image;

    public float MaxRayTriggerTime {
        get
        {
            if (target != null)
            {
                return target.rayTriggerTime + progressOffset;
            }
            else
            {
                Debug.LogWarning("Target = null. Returning default value");
                return 1f + progressOffset;
            }
        }
    }

    void Start()
    {
        image = GetComponentInChildren<UnityEngine.UI.Image>();
    }


    void Update()
    {
        if (target == null)
        {
            currentProgress = 0;
            image.fillAmount = 0;
            return;
        }

        // follow the object
        transform.position = target.gameObject.transform.position;

        // look towards the camera
        transform.LookAt(Camera.main.transform);

        currentProgress = Mathf.Clamp(currentProgress, 0f, target.rayTriggerTime + progressOffset);
        image.fillAmount = Mathf.Clamp01((currentProgress - progressOffset) / target.rayTriggerTime);
    }
}
