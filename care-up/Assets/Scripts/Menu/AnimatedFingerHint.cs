using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimatedFingerHint : MonoBehaviour
{
    public bool toMove = false;
    bool toWave = false;
    bool shown = false;
    public RectTransform FingerHolder;
    public RectTransform TargetPointer;

    public Animator fingerAnimator;
    float moveSpeed = 500f;
    Vector3 moveTarget = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        moveTarget = FingerHolder.position;
    }
    void ShowHand(bool toShow, bool showEffect = false, bool _toWave = false)
    {
        if (toShow)
        {
            if (!FingerHolder.gameObject.activeSelf)
            {
                FingerHolder.gameObject.SetActive(true);
                toMove = true;
            }
            if (showEffect)
            {
                toMove = false;
                fingerAnimator.SetTrigger("show");
                if (_toWave)
                    fingerAnimator.SetTrigger("hi");
            }
        }
        else
        {
            FingerHolder.gameObject.SetActive(false);
            toMove = false;
        }

    }

    public void startMove()
    {

    }

    void Update()
    {
        if (toMove)
        {
            FingerHolder.position = Vector3.MoveTowards(FingerHolder.position, moveTarget, Time.deltaTime * moveSpeed);
        }
        if (Input.GetMouseButtonDown(0))
        {
            fingerAnimator.SetTrigger("fly");
            toMove = true;
            Vector2 pos;
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, 
                Input.mousePosition, canvas.worldCamera, out pos);
            moveTarget = canvas.transform.TransformPoint(pos);
            TargetPointer.position = moveTarget;
            float dist = Vector3.Distance(FingerHolder.position, moveTarget);
            print(Input.mousePosition);
            moveSpeed = dist * 1f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            ShowHand(!FingerHolder.gameObject.activeSelf, true, toWave);
            Debug.Log("Pressed secondary button.");
        }

        if (Input.GetMouseButtonDown(2))
        {
            fingerAnimator.SetTrigger("hi");

            //Debug.Log("Pressed middle click.");
            //toWave = !toWave;
            //print(toWave);
        }
    }
}
