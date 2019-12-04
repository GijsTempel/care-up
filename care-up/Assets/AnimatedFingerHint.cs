using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimatedFingerHint : MonoBehaviour
{
    public Canvas myCanvas;
    public RectTransform FingerHolder;
    public Animator fingerAnimator;
    float moveSpeed = 500f;
    Vector3 moveTarget = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    void Update()
    {
        FingerHolder.position = Vector3.MoveTowards(FingerHolder.position, moveTarget, Time.deltaTime * moveSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            fingerAnimator.SetTrigger("fly");
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            moveTarget = myCanvas.transform.TransformPoint(pos);
            float dist = Vector3.Distance(FingerHolder.position, moveTarget);
            print(dist);
            moveSpeed = dist * 1f;
        }

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
    }
}
