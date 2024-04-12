using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;

public class HoveringRayButtonCollider : MonoBehaviour
{
    public UnityEvent triggerButton = new UnityEvent();

    public Image progressImage;
    public float waitTime = 1.5f;
    public PickableObject pickableObject;

    public void SetProgressValue(float value)
    {
        if (value < 0)
        {
            progressImage.gameObject.SetActive(false);
        }
        else
        {
            progressImage.gameObject.SetActive(true);
            progressImage.fillAmount = value;
        }
    }

    public void Execute()
    {
        triggerButton.Invoke();
    }

}
