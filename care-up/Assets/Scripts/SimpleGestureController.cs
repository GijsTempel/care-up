using System;
using UnityEngine;

public class SimpleGestureController
{
    private Vector2 firstPressPosition;
    private Vector2 secondPressPosition;
    private Vector2 currentSwipe;

    public void ManageSwipeGestures(Action action, Action secondAction = null)
    {       
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            currentSwipe = new Vector2(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);
            currentSwipe.Normalize();

            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                action.Invoke();
            }
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (secondAction != null)
                    secondAction.Invoke();
            }
        }
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                firstPressPosition = new Vector2(touch.position.x, touch.position.y);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                secondPressPosition = new Vector2(touch.position.x, touch.position.y);
                currentSwipe = new Vector3(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);
                currentSwipe.Normalize();

                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    action.Invoke();
                }
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (secondAction != null)
                        secondAction.Invoke();
                }
            }
        }
    }
}
