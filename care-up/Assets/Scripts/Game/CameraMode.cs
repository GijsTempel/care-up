﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

/// <summary>
/// Class for handling specific camera configurations other, than default player.
/// </summary>
public class CameraMode : MonoBehaviour {

	public enum Mode
    {
        Free,               // player moves freely
        ObjectPreview,      // object in center, controls locked, close/pick
        SelectionDialogue,  // selection dialogue, controls locked
        ConfirmUI,          // yes/no dialogue, controls locked
        Cinematic           // playing animations, some controls disabled
    };

    public float minZoom = 0.5f;
    public float maxZoom = 2.0f;
    
    private Mode currentMode = Mode.Free;

    private ExaminableObject selectedObject;
    private SystemObject doorSelected;

    private GameObject confirmUI;

    [HideInInspector]
    public bool animating = false;
    [HideInInspector]
    public bool animationEnded = false;
    [HideInInspector]
    public bool cinematicToggle = false;
    private int cinematicDirection = 1;
    private float cinematicLerp = 0.0f;
    private Vector3 cinematicPos;
    private Quaternion cinematicRot;
    private Vector3 cinematicTargetPos;
    private Quaternion cinematicTargetRot;
    private Transform cinematicControl;

    private Quaternion camPosition;

    private Controls controls;
    private RigidbodyFirstPersonController playerScript;
    private HandsInventory inventory;
    private UnityStandardAssets.ImageEffects.BlurOptimized blur;

    public Mode CurrentMode
    {
        get { return currentMode; }
    }

    void Start()
    {
        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls script found");

        playerScript = GameObject.Find("Player").GetComponent<RigidbodyFirstPersonController>();
        if (playerScript == null) Debug.LogError("No Player script found");

        inventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        blur = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
        if (blur == null) Debug.Log("No Blur Attached to main camera.");

        confirmUI = (GameObject)Instantiate(Resources.Load("Prefabs/ConfirmUI"), Vector3.zero, Quaternion.identity);
        confirmUI.SetActive(false);
    }

    void Update()
    {
        // handle confirm mode
        if (currentMode == Mode.ConfirmUI)
        {
            if (controls.keyPreferences.mouseClickKey.Pressed()
                || controls.MouseClicked())
            {
                doorSelected.Use(true);
            }
            else if (controls.keyPreferences.closeObjectView.Pressed()
                || controls.RightMouseClicked())
            {
                ToggleCameraMode(Mode.Free);
            }
        }

        // clicked on something in free mode, possibly change mode
        if (controls.MouseClicked() && currentMode == Mode.Free)
        {
            if (controls.SelectedObject && controls.CanInteract)
            {
                selectedObject = controls.SelectedObject.GetComponent<ExaminableObject>();
                if (selectedObject != null) // if there is a component
                {
                    selectedObject.OnExamine();
                    controls.ResetObject();
                }
                else if (controls.SelectedObject.GetComponent<SystemObject>() != null)
                {
                    doorSelected = controls.SelectedObject.GetComponent<SystemObject>();
                    doorSelected.Use();
                }
            }
        }

        // handle object preview, close/pick,zoom
        if (currentMode == Mode.ObjectPreview)
        {
            if (controls.keyPreferences.closeObjectView.Pressed())
            {
                ToggleCameraMode(Mode.Free);

                selectedObject.ToggleViewMode(false);
                selectedObject = null;
            }
            else if (controls.keyPreferences.pickObjectView.Pressed())
            {
                PickableObject pickableObject = selectedObject.GetComponent<PickableObject>();
                if (pickableObject != null)
                {
                    ToggleCameraMode(Mode.Free);
                    pickableObject.GetComponent<ExaminableObject>().ToggleViewMode(false);
                    inventory.PickItem(pickableObject);
                    selectedObject = null;
                }
            }
            else if (controls.keyPreferences.mouseClickKey.Pressed() ||
                Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (selectedObject.viewSettings.distanceFromCamera > minZoom)
                        selectedObject.viewSettings.distanceFromCamera += Input.GetAxis("Mouse ScrollWheel");
                }
                else
                {
                    if (selectedObject.viewSettings.distanceFromCamera > minZoom)
                        selectedObject.viewSettings.distanceFromCamera -= 0.5f;
                }
            }
            else if (controls.keyPreferences.CombineKey.Pressed() ||
                Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (selectedObject.viewSettings.distanceFromCamera < maxZoom)
                        selectedObject.viewSettings.distanceFromCamera += Input.GetAxis("Mouse ScrollWheel");
                }
                else
                {
                    if (selectedObject.viewSettings.distanceFromCamera < maxZoom)
                        selectedObject.viewSettings.distanceFromCamera += 0.5f;
                }
            }
        }

        // handle player 'moving'
        if ( currentMode == Mode.Cinematic )
        {
            CinematicUpdate();
        }
        else
        {
            // clear unintended flag for non-cinematic animations
            animationEnded = false;
        }

        // handle object update in preview
        if ( currentMode == Mode.ObjectPreview )
        {
            if ( selectedObject == null )
            {
                Debug.LogError("No object selected for View Mode");
            }
            else
            {
                selectedObject.ViewModeUpdate();
            }
        }
    }

    public void ObjectViewPickUpButton()
    {
        PickableObject pickableObject = selectedObject.GetComponent<PickableObject>();
        if (pickableObject != null)
        {
            ToggleCameraMode(Mode.Free);
            pickableObject.GetComponent<ExaminableObject>().ToggleViewMode(false);
            inventory.PickItem(pickableObject);
            selectedObject = null;
        }
    }

    public void ObjectViewPutDownButton()
    {
        ToggleCameraMode(Mode.Free);

        selectedObject.ToggleViewMode(false);
        selectedObject = null;
    }

    /// <summary>
    /// Switches properly mode setting controls locks, cursor, blur, etc.
    /// </summary>
    /// <param name="mode">Next mode.</param>
    public void ToggleCameraMode(Mode mode)
    {
        if (currentMode == Mode.Free && mode == Mode.ObjectPreview)
        {
            TogglePlayerScript(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            blur.enabled = true;

            GameObject buttonsParent = Camera.main.transform.Find("UI").Find("ObjectViewButtons").gameObject;
            buttonsParent.transform.GetChild(0).GetComponent<Button>().interactable =
                (selectedObject.GetComponent<PickableObject>() != null);
            buttonsParent.SetActive(true);
        }
        else if (currentMode == Mode.ObjectPreview && mode == Mode.Free)
        {
            TogglePlayerScript(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            blur.enabled = false;

            GameObject.Find("ObjectViewButtons").SetActive(false);
        }
        else if (mode == Mode.SelectionDialogue)
        {
            TogglePlayerScript(false);
        }
        else if (currentMode == Mode.SelectionDialogue)
        {
            TogglePlayerScript(true);
        }
        else if (currentMode == Mode.Free && mode == Mode.ConfirmUI)
        {
            TogglePlayerScript(false);
            confirmUI.transform.position = Camera.main.transform.position + Camera.main.transform.forward * (0.3f);
            confirmUI.transform.rotation = Camera.main.transform.rotation;
            confirmUI.SetActive(true);
        }
        else if (currentMode == Mode.ConfirmUI && mode == Mode.Free)
        {
            TogglePlayerScript(true);
            confirmUI.SetActive(false);
        }
        else if (mode == Mode.Cinematic)
        {
            playerScript.mouseLook.SetMode(true, Quaternion.identity);
            playerScript.tutorial_movementLock = true;
            playerScript.mouseLook.clampHorisontalRotation = true;
            playerScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            camPosition = Camera.main.transform.localRotation;
        }
        else if (currentMode == Mode.Cinematic && mode == Mode.Free)
        {
            playerScript.mouseLook.SetMode(false, camPosition);
            if (GameObject.Find("GameLogic").GetComponent<TutorialManager>() != null)
            {
                playerScript.tutorial_movementLock =
                    GameObject.Find("GameLogic").GetComponent<TutorialManager>().movementLock;
            }
            else
            {
                playerScript.tutorial_movementLock = false;
            }
            playerScript.mouseLook.clampHorisontalRotation = false;
            playerScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }

        currentMode = mode;
        controls.ResetObject();
    }

    /// <summary>
    /// Enables/disables player controls
    /// </summary>
    /// <param name="value">True - enable, False - disable</param>
    void TogglePlayerScript(bool value)
    {
        if (value)
        {
            playerScript.enabled = true;
            playerScript.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            playerScript.enabled = false;
            playerScript.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    
    /// <summary>
    /// Chunck in update for handling cinematic
    /// </summary>
    void CinematicUpdate()
    {
        if (cinematicDirection == 1 || animationEnded)
        {
            cinematicLerp = Mathf.Clamp01(cinematicLerp + 2 * Time.deltaTime * cinematicDirection);
        }

        cinematicControl.transform.position =
            Vector3.Lerp(cinematicPos, cinematicTargetPos, cinematicLerp);
        cinematicControl.Find("Arms").transform.rotation =
            Quaternion.Lerp(cinematicRot, cinematicTargetRot, cinematicLerp);

        if (cinematicDirection == 1 && cinematicLerp == 1.0f)
        {
            cinematicDirection = -1;
            if ( cinematicToggle )
            PlayerAnimationManager.ToggleAnimationSpeed();
        }
        else if (cinematicDirection == -1 && cinematicLerp == 0.0f)
        {
            ToggleCameraMode(Mode.Free);
            cinematicDirection = 1;
            animationEnded = false;
        }
    }

    public void SetCinematicMode(Transform target)
    {
        ToggleCameraMode(Mode.Cinematic);

        if (cinematicToggle)
        {
            PlayerAnimationManager.ToggleAnimationSpeed();
        }

        cinematicLerp = 0.0f;
        cinematicDirection = 1;
        cinematicControl = playerScript.transform.GetChild(0);
        cinematicPos = cinematicControl.transform.position;
        cinematicRot = cinematicControl.Find("Arms").transform.rotation;

        Transform cTarget = target.Find("CinematicTarget");
        cinematicTargetRot = cTarget.rotation;
        cinematicTargetPos = cTarget.position;
    }
}
