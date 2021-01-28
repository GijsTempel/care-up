using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using System.Linq;

public class GestureControls : MonoBehaviour
{
    private TapGestureRecognizer tapGesture;
    private TapGestureRecognizer doubleTapGesture;
    private SwipeGestureRecognizer swipeGesture;
    private LongPressGestureRecognizer longPressGesture;

    private GameObject initedObject = null;
    private Controls controls;
    private HandsInventory handsInventory;
    private TutorialManager tutorial;
    private CameraMode cameraMode;
    private Tutorial_Combining tutorialCombine;

    private PlayerScript player;

    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }

    void DebugList(List<PickableObject> items)
    {
        foreach (PickableObject i in items)
        {
            Debug.Log(i.name + " " + i.positionID + " " +
                Vector3.Distance(i.transform.position, player.transform.position));
        }
    }

    private void DoubleTapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            //DebugText("Double tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            if (IsViableWithUIOpen() && cameraMode.animating == false)
            {
                if (tutorial == null || (tutorial != null &&
                (tutorial.itemToDrop == initedObject.name ||
                tutorial.itemToDrop2 == initedObject.name)))
                {
                    PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
                    player.itemControls.Close(true);

                    PickableObject item = initedObject.GetComponent<PickableObject>();

                    DebugList(item.ghostObjects);
                    List<PickableObject> ghosts = item.ghostObjects.OrderBy(x =>
                        Vector3.Distance(x.transform.position, player.transform.position)).ToList();
                    DebugList(ghosts);

                    GameObject ghost = ghosts[0].gameObject;

                    if (handsInventory.LeftHandObject == initedObject)
                    {
                        handsInventory.DropLeft(ghost);
                    }
                    else if (handsInventory.RightHandObject == initedObject)
                    {
                        handsInventory.DropRight(ghost);
                    }

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
        if (PlayerScript.actionsLocked == true)
            return;

        if (gesture.State == GestureRecognizerState.Ended && !player.freeLook)
        {
            if (handsInventory.LeftHandEmpty() && handsInventory.RightHandEmpty())
                return; // both hands empty, nothing to combine/decombine

            //DebugText("Swiped from {0},{1} to {2},{3}; velocity: {4}, {5}", gesture.StartFocusX, gesture.StartFocusY, gesture.FocusX, gesture.FocusY, swipeGesture.VelocityX, swipeGesture.VelocityY);
            if (Mathf.Abs(swipeGesture.VelocityX) > Mathf.Abs(swipeGesture.VelocityY))
            {
                // if we are here - this means it's more likely to be a horisontal swipe
                // horisontal swipe mean we're trying to combine
                if (handsInventory.LeftHandEmpty() || handsInventory.RightHandEmpty())
                {
                    // if we're here we're missing one object it seems, make a warning maybe somewhere?
                    string message = "Je hebt geen object om mee te combineren. Zorg dat je een object in beide handen hebt om ze met elkaar te combineren.";
                    GameObject.FindObjectOfType<GameUI>().ShowBlockMessage("Geen tweede object", message);

                    player.itemControls.Close();
                    return;
                }
            }
            else // else it's a vertical swipe, vertical means we're trying to DEcombine
            {
                // we can't decombine with both hands having an item, we need one item in hands and another one free
                if (handsInventory.LeftHandEmpty() == handsInventory.RightHandEmpty())
                {
                    // here we have either both hands filled
                    string message = "Je kunt niet objecten scheiden/uit elkaar halen met beide handen vol. Leg een object terug om een hand vrij te maken.";
                    GameObject.FindObjectOfType<GameUI>().ShowBlockMessage("Je hebt je handen vol.", message);

                    player.itemControls.Close();
                    return;
                }
            }

            if (tutorialCombine != null && !tutorialCombine.decombiningAllowed)
            {
                return;
            }

            player.itemControls.Close(true);
            handsInventory.OnCombineAction();
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

            if (IsViableWithUIOpen())
            {
                if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
                    GameObject.FindObjectOfType<PlayerScript>().itemControls.Close();

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
            PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
            player.itemControls.Close();
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
        player = GameObject.FindObjectOfType<PlayerScript>();

        // don't reorder the creation of these :)
        CreateDoubleTapGesture(); // double tap for dropping still enabled
        /* disabling gestures ? i forgot if i needed to disable
        CreateTapGesture();
        CreateSwipeGesture();
        CreateLongPressGesture();
        */

        // show touches, only do this for debugging as it can interfere with other canvases
        //FingersScript.Instance.ShowTouches = true;

        controls = GameObject.FindObjectOfType<Controls>();
        handsInventory = GameObject.FindObjectOfType<HandsInventory>();
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
                 ((controls.SelectedObject == handsInventory.LeftHandObject)
                || (controls.SelectedObject == handsInventory.RightHandObject));
    }

    private bool IsViableWithUIOpen()
    {
        initedObject = controls.SelectedObject;

        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();

        return !player.away && controls.SelectedObject != null
                && controls.SelectedObject.GetComponent<InteractableObject>() != null
                && !PlayerScript.actionsLocked &&
                 ((controls.SelectedObject == handsInventory.LeftHandObject)
                || (controls.SelectedObject == handsInventory.RightHandObject));
    }
}
