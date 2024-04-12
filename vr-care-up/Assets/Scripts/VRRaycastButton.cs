using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class VRRaycastButton : MonoBehaviour
{
    public Image progressImage;

    public RectTransform pointCursor;
    float progressCounter = -1f;
    const float MAX_PROGRESS_COUNTER = 2f;
    float fadeOutTimer;
    const float FADEOUT_MAX = 0.1f;

    float contactTimer;

    GameObject progressTarget;
    GameObject newProgressTarget;

    Vector2 WorldToCanvasPos(Vector3 worldPos)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPos);
        float _scale = transform.parent.localScale.x; //selectionDialogueElements.GetComponent<RectTransform>().localScale.x;
        return new Vector2(localPos.x / _scale, localPos.y / _scale);
    }

    public void PointerRayHit(Vector3 hitPos)
    {
        Vector2 canvasHitPos = WorldToCanvasPos(hitPos);
        fadeOutTimer = FADEOUT_MAX;
        pointCursor.anchoredPosition = canvasHitPos;
        //GameObject sqButtonInPos = GetSqButtonInPosition(hitPos);
        //if (sqButtonInPos != null)
        {
            progressImage.gameObject.SetActive(true);
            pointCursor.GetComponent<Image>().color = Color.green;
            //newProgressTarget = sqButtonInPos;
            contactTimer = FADEOUT_MAX / 2;
        }
        /*else
        {
            progressImage.gameObject.SetActive(false);
            pointCursor.GetComponent<Image>().color = Color.gray;
        }*/
    }

    void Update()
    {
        if (fadeOutTimer > 0)
            fadeOutTimer -= Time.deltaTime;
        if (fadeOutTimer < 0)
        {
            fadeOutTimer = 0;
        }
        if (contactTimer > 0 && (contactTimer - Time.deltaTime <= 0))
            newProgressTarget = null;

        contactTimer -= Time.deltaTime;
        pointCursor.GetComponent<CanvasGroup>().alpha = fadeOutTimer / FADEOUT_MAX;
        if (progressTarget != newProgressTarget)
            progressCounter = MAX_PROGRESS_COUNTER;
        else if (progressTarget != null && progressCounter > -1f)
            progressCounter -= Time.deltaTime;

        progressTarget = newProgressTarget;
        if (progressTarget != null)
        {
            progressImage.fillAmount = Mathf.Clamp01(progressCounter / MAX_PROGRESS_COUNTER);
            if (progressCounter < 0 && progressCounter > -1f)
            {
                if (progressTarget.GetComponent<ActionModule_ActionTrigger>() != null)
                {
                    progressTarget.GetComponent<ActionModule_ActionTrigger>().GetComponentInChildren<ActionCondition_ActionCollider>().RayTriggerAction();
                }
                progressCounter = -2f;
            }
        }
    }
}
