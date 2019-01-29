using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class GestureControls : MonoBehaviour
{

    private TapGestureRecognizer tapGesture;
    private TapGestureRecognizer doubleTapGesture;
    private SwipeGestureRecognizer swipeGesture;
    private LongPressGestureRecognizer longPressGesture;

    private GameObject initedObject = null;
    private Controls controls;
    private HandsInventory inv;
    private TutorialManager tutorial;
    private CameraMode cameraMode;
    private Tutorial_Combining tutorialCombine;
    
    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }

    private void DoubleTapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            DebugText("Double tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            if (IsViableObject())
            {
                if (tutorial == null || (tutorial != null &&
                (tutorial.itemToDrop == initedObject.name ||
                tutorial.itemToDrop2 == initedObject.name)))
                {
                    if (inv.LeftHandObject == initedObject)
                    {
                        inv.DropLeft();
                    }
                    else if (inv.RightHandObject == initedObject)
                    {
                        inv.DropRight();
                    }

                    PickableObject item = initedObject.GetComponent<PickableObject>();
                    if (item != null)
                    {
                        for (int i = item.ghostObjects.Count - 1; i >= 0; --i)
                        {
                            GameObject g = item.ghostObjects[i].gameObject;
                            item.ghostObjects.RemoveAt(i);
                            Destroy(g);
                        }
                    }
                }
            }
        }
    }

    private void CreateDoubleTapGesture()
    {
        doubleTapGesture = new TapGestureRecognizer();
        doubleTapGesture.NumberOfTapsRequired = 2;
        doubleTapGesture.StateUpdated += DoubleTapGestureCallback;
        FingersScript.Instance.AddGesture(doubleTapGesture);
    }

    private void SwipeGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            //DebugText("Swiped from {0},{1} to {2},{3}; velocity: {4}, {5}", gesture.StartFocusX, gesture.StartFocusY, gesture.FocusX, gesture.FocusY, swipeGesture.VelocityX, swipeGesture.VelocityY);
            if (Mathf.Abs(swipeGesture.VelocityX) > Mathf.Abs(swipeGesture.VelocityY))
            {
                // if we are here - this means it's more likely to be a horisontal swipe
                // horisontal swipe mean we're trying to combine
                if (inv.LeftHandEmpty() || inv.RightHandEmpty())
                {
                    // if we're here we're missing one object it seems, make a warning maybe somewhere?
                    string message = "missing something in hands? combining something + nothing?";
                    RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
                    messageCenter.NewMessage("missing 2nd object", message, RobotUIMessageTab.Icon.Warning);
                    return;
                }
            }
            else // else it's a vertical swipe, vertical means we're trying to DEcombine
            {
                // we can't decombine with both hands having an item, we need one item in hands and another one free
                if (inv.LeftHandEmpty() == inv.RightHandEmpty())
                {
                    // here we have either both hands filled or both hands empty, both wrong
                    string message = "hands full warning message";
                    RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
                    messageCenter.NewMessage("hands full warning", message, RobotUIMessageTab.Icon.Warning);
                    return;
                }
            }
            
            if (tutorialCombine != null && !tutorialCombine.decombiningAllowed)
            {
                return;
            }
            
            inv.OnCombineAction();

        }
    }

    private void CreateSwipeGesture()
    {
        swipeGesture = new SwipeGestureRecognizer();
        swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
        swipeGesture.StateUpdated += SwipeGestureCallback;
        swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
        FingersScript.Instance.AddGesture(swipeGesture);
    }

    private void LongPressGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            //DebugText("Long press began: {0}, {1}", gesture.FocusX, gesture.FocusY);

            if (IsViableObject())
            {
                if (initedObject.GetComponent<ExaminableObject>() != null)
                {
                    if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
                        GameObject.FindObjectOfType<PlayerScript>().itemControls.Close();

                    if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI
                        || cameraMode.currentMode == CameraMode.Mode.Free)
                    {
                        cameraMode.selectedObject = initedObject.GetComponent<ExaminableObject>();
                        if (cameraMode.selectedObject != null) // if there is a component
                        {
                            cameraMode.selectedObject.OnExamine();
                            controls.ResetObject();
                        }
                    }
                }
            }
        }
    }

    private void CreateLongPressGesture()
    {
        longPressGesture = new LongPressGestureRecognizer();
        longPressGesture.MaximumNumberOfTouchesToTrack = 1;
        longPressGesture.StateUpdated += LongPressGestureCallback;
        FingersScript.Instance.AddGesture(longPressGesture);
    }

    private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            //DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            if (IsViableObject())
            {
                GameObject.FindObjectOfType<PlayerScript>().itemControls.Init(controls.SelectedObject);
            }
        }
    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    private void Start()
    {
        // don't reorder the creation of these :)
        CreateDoubleTapGesture();
        CreateTapGesture();
        CreateSwipeGesture();
        CreateLongPressGesture();
        
        // show touches, only do this for debugging as it can interfere with other canvases
        FingersScript.Instance.ShowTouches = true;

        controls = GameObject.FindObjectOfType<Controls>();
        inv = GameObject.FindObjectOfType<HandsInventory>();
        tutorial = GameObject.FindObjectOfType<TutorialManager>();
        cameraMode = GameObject.FindObjectOfType<CameraMode>();
        tutorialCombine = GameObject.FindObjectOfType<Tutorial_Combining>();
    }

    private bool IsViableObject()
    {
        initedObject = controls.SelectedObject;

        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        
        return !player.away && controls.SelectedObject != null
                && controls.SelectedObject.GetComponent<InteractableObject>() != null
                && !player.itemControls.gameObject.activeSelf
                && !PlayerScript.actionsLocked
                && !player.usingOnMode &&
                 ((controls.SelectedObject == inv.LeftHandObject)
                || (controls.SelectedObject == inv.RightHandObject));
    }
}
