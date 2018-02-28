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
        Cinematic,          // playing animations, some controls disabled
        ItemControlsUI      // UI showing actions is active
    };

    public float minZoom = 0.5f;
    public float maxZoom = 2.0f;
    
    private Mode currentMode = Mode.Free;

    public ExaminableObject selectedObject;
    public SystemObject doorSelected;

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
    private Quaternion savedRot;
    private Quaternion targetRot;
    private bool soloCamera;

    private Quaternion camPosition;

    private Controls controls;
    private PlayerScript playerScript;
    private HandsInventory inventory;
    private UnityStandardAssets.ImageEffects.BlurOptimized blur;

    public bool dontMoveCamera;

    public Mode CurrentMode
    {
        get { return currentMode; }
    }

    void Start()
    {
        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls script found");

        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
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

        // handle object preview, close/pick,zoom
        if (currentMode == Mode.ObjectPreview && !selectedObject.animationExamine)
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
            else if (Input.GetMouseButtonDown(1)) // right click
            {
                ObjectViewPutDownButton();
            }
        }

        // handle player 'moving'
        if (currentMode == Mode.Cinematic)
        {
            CinematicUpdate();
        }
        else
        {
            if (soloCamera)
            {
                AnimationCameraUpdate(true);
            }
            else
            {
                // clear unintended flag for non-cinematic animations
                animationEnded = false;
            }
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
        if (mode == Mode.ObjectPreview)
        {
            TogglePlayerScript(false);

            if (!selectedObject.animationExamine)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                #if UNITY_STANDALONE_OSX
                    blur.enabled = true;
                #elif UNITY_STANDALONE_WIN
                    blur.enabled = true;
                #else
                    blur.enabled = false;
                #endif
            }

            GameObject buttonsParent = Camera.main.transform.Find("UI").Find("ObjectViewButtons").gameObject;
            buttonsParent.transform.GetChild(0).GetComponent<Button>().interactable =
                (selectedObject.GetComponent<PickableObject>() != null) &&
                (selectedObject.gameObject != inventory.LeftHandObject) &&
                (selectedObject.gameObject != inventory.RightHandObject);

            if (GetComponent<TutorialManager>() != null)
            {
                if (controls.keyPreferences.pickObjectView.locked)
                {
                    buttonsParent.transform.GetChild(0).GetComponent<Button>().interactable = false;
                }
            }

            buttonsParent.SetActive(true);
            playerScript.MoveBackButtonObject.SetActive(false);
        }
        else if (currentMode == Mode.ObjectPreview && mode == Mode.Free)
        {
            TogglePlayerScript(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            blur.enabled = false;

            GameObject.Find("ObjectViewButtons").SetActive(false);
            playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
        }
        else if (mode == Mode.SelectionDialogue)
        {
            TogglePlayerScript(false);
            playerScript.MoveBackButtonObject.SetActive(false);
            GameObject.FindObjectOfType<RobotManager>().top = false;
        }
        else if (currentMode == Mode.SelectionDialogue)
        {
            TogglePlayerScript(true);
            playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
            GameObject.FindObjectOfType<RobotManager>().top = true;
        }
        else if (currentMode == Mode.Free && mode == Mode.ConfirmUI)
        {
            TogglePlayerScript(false);
            confirmUI.transform.position = Camera.main.transform.position + Camera.main.transform.forward * (0.3f);
            confirmUI.transform.rotation = Camera.main.transform.rotation;
            confirmUI.SetActive(true);

            playerScript.MoveBackButtonObject.SetActive(false);
        }
        else if (currentMode == Mode.ConfirmUI && mode == Mode.Free)
        {
            TogglePlayerScript(true);
            confirmUI.SetActive(false);

            playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
        }
        else if (mode == Mode.Cinematic)
        {
            playerScript.mouseLook.SetMode(true, Quaternion.identity);
            playerScript.tutorial_movementLock = true;
            playerScript.mouseLook.clampHorisontalRotation = true;
            playerScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            camPosition = Camera.main.transform.localRotation;

            playerScript.MoveBackButtonObject.SetActive(false);
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

            playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
            dontMoveCamera = false;
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

        if (!dontMoveCamera)
            AnimationCameraUpdate(false);

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

    void AnimationCameraUpdate(bool solo)
    {
        if (solo)
        {
            if (cinematicDirection == 1 || animationEnded)
            {
                cinematicLerp = Mathf.Clamp01(cinematicLerp + 2 * Time.deltaTime * cinematicDirection);
            }
        }

        Camera.main.transform.rotation = Quaternion.Lerp(savedRot, targetRot, cinematicLerp);

        if (solo)
        {
            if (cinematicDirection == 1 && cinematicLerp == 1.0f)
            {
                cinematicDirection = -1;
            }
            else if (cinematicDirection == -1 && cinematicLerp == 0.0f)
            {
                cinematicDirection = 1;
                animationEnded = false;
                soloCamera = false;

                playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
                dontMoveCamera = false;
            }
        }
    }

    public void SetCameraUpdating(bool solo)
    {
        if (solo)
        {
            soloCamera = true;
            cinematicLerp = 0.0f;
            cinematicDirection = 1;

            playerScript.MoveBackButtonObject.SetActive(false);
        }

        savedRot = Camera.main.transform.rotation;
        targetRot = savedRot * Quaternion.Euler(25.0f, 0.0f, 0.0f);
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
        
        if (!dontMoveCamera)
            SetCameraUpdating(false);
    }
}
