using UnityEngine;
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

	bool camViewObject = false;
	Quaternion camRotFrom;
	Quaternion camRotTo;
	float startTime;
	float camRotTime;
	//bool backFromObjectPreview = false; never used?
    
    public float minZoom = 0.5f;
    public float maxZoom = 2.0f;
    
    public Mode currentMode = Mode.Free;

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

    //private Quaternion camPosition; never used

    private Controls controls;
    private PlayerScript playerScript;
    private HandsInventory inventory;
    private UnityStandardAssets.ImageEffects.BlurOptimized blur;

    public bool dontMoveCamera;
    
    private bool previewModeFrame; //fix

    public  Mode CurrentMode
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

        confirmUI = (GameObject)Instantiate(Resources.Load("Prefabs/UI/ConfirmUI"), Vector3.zero, Quaternion.identity);
        confirmUI.SetActive(false);
    }

    void Update()
    {
		if (camViewObject)
		{
			//---------------------------------------------------------
	
			camRotTime = Time.time - startTime;
			Camera.main.transform.localRotation = Quaternion.Lerp(camRotFrom, camRotTo, camRotTime);
			if (camRotTime > 1f)
			{
				camViewObject = false;
			}
			return;
			//else
			//{
			//	camRotTime = (Time.time - startTime);
			//	//Camera.main.transform.localRotation = Quaternion.Lerp(camRotTo, camRotFrom, camRotTime);
   //             if (camRotTime > 1f)
   //             {
   //                 camViewObject = false;
			//		backFromObjectPreview = false;
   //             }

			//}
		}
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

            if (controls.MouseClicked())
            {
                if (previewModeFrame && (Input.touchCount > 0)) // skip first touch hopefully
                {
                    previewModeFrame = false;
                }
                else
                {
                    ObjectViewPutDownButton();
                }
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
		if (currentMode == Mode.ObjectPreview && mode == Mode.Free)
        {
			//startTime = Time.time;
            //camViewObject = true;
            //backFromObjectPreview = true;

            TogglePlayerScript(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            blur.enabled = false;

            GameObject.Find("ObjectViewButtons").SetActive(false);
            playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
            playerScript.joystickObject.SetActive(!playerScript.robotUIopened);

            if (GameObject.FindObjectOfType<TutorialManager>() == null ||
                GameObject.FindObjectOfType<Tutorial_UI>() != null ||
                GameObject.FindObjectOfType<Tutorial_Theory>() != null)
            {
                RobotManager.SetUITriggerActive(true);
            }
        }

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

            //GameObject.Find("UI").GetComponent<ExtraObjectOptions>()._show("ObjectViewButtons", true);

            GameObject.FindObjectOfType<GameUI>().transform.Find("ObjectViewButtons").gameObject.SetActive(true);
            //GameObject.Find("UI").transform.Find("ObjectViewButtons").gameObject.SetActive(true);
            //-----------------------------
            camRotFrom = Camera.main.transform.localRotation;
			startTime = Time.time;
			camRotTo = Quaternion.Euler(8.0f, 0.0f, 0.0f);
			camViewObject = true;
            //Camera.main.transform.localRotation = Quaternion.Euler(8.0f, 0.0f, 0.0f);
            

            playerScript.MoveBackButtonObject.SetActive(false);
            playerScript.joystickObject.SetActive(false);
            RobotManager.SetUITriggerActive(false);

            previewModeFrame = true;
        }
        
        else if (mode == Mode.SelectionDialogue)
        {
            TogglePlayerScript(false);
            playerScript.MoveBackButtonObject.SetActive(false);
            playerScript.joystickObject.SetActive(false);
            RobotManager.SetUITriggerActive(false);
            GameObject.FindObjectOfType<RobotManager>().top = false;
        }
        else if (currentMode == Mode.SelectionDialogue)
        {
            TogglePlayerScript(true);
            playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
            playerScript.joystickObject.SetActive(!playerScript.robotUIopened);
            if (GameObject.FindObjectOfType<TutorialManager>() == null ||
                GameObject.FindObjectOfType<Tutorial_UI>() != null ||
                GameObject.FindObjectOfType<Tutorial_Theory>() != null)
            {
                RobotManager.SetUITriggerActive(true);
            }
            GameObject.FindObjectOfType<RobotManager>().top = true;
        }
        else if (currentMode == Mode.Free && mode == Mode.ConfirmUI)
        {
            TogglePlayerScript(false);
            confirmUI.transform.position = Camera.main.transform.position + Camera.main.transform.forward * (0.3f);
            confirmUI.transform.rotation = Camera.main.transform.rotation;
            confirmUI.SetActive(true);

            playerScript.MoveBackButtonObject.SetActive(false);
            playerScript.joystickObject.SetActive(false);
            RobotManager.SetUITriggerActive(false);
        }
        else if (currentMode == Mode.ConfirmUI && mode == Mode.Free)
        {
            TogglePlayerScript(true);
            confirmUI.SetActive(false);

            playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
            playerScript.joystickObject.SetActive(!playerScript.robotUIopened);
            if (GameObject.FindObjectOfType<TutorialManager>() == null ||
                GameObject.FindObjectOfType<Tutorial_UI>() != null ||
                GameObject.FindObjectOfType<Tutorial_Theory>() != null)
            {
                RobotManager.SetUITriggerActive(true);
            }
        }
        else if (mode == Mode.Cinematic)
        {
            playerScript.tutorial_movementLock = true;
            playerScript.mouseLook.clampHorisontalRotation = true;
            playerScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            //camPosition = Camera.main.transform.localRotation; never used

            playerScript.MoveBackButtonObject.SetActive(false);
            playerScript.joystickObject.SetActive(false);
            RobotManager.SetUITriggerActive(false);
        }
        else if (currentMode == Mode.Cinematic && mode == Mode.Free)
        {
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

            if ((GameObject.FindObjectOfType<TutorialManager>() != null &&
                !GameObject.FindObjectOfType<TutorialManager>().finished) ||
                GameObject.FindObjectOfType<TutorialManager>() == null)
            {
                playerScript.MoveBackButtonObject.SetActive(!playerScript.away);
                playerScript.joystickObject.SetActive(!playerScript.robotUIopened);
            }

            if (GameObject.FindObjectOfType<TutorialManager>() == null ||
                GameObject.FindObjectOfType<Tutorial_UI>() != null ||
                GameObject.FindObjectOfType<Tutorial_Theory>() != null)
            {
                RobotManager.SetUITriggerActive(true);
            }
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
        if (animationEnded || cinematicDirection == 1)
            cinematicControl.Find("Arms").transform.rotation =
                Quaternion.Lerp(cinematicRot, cinematicTargetRot, cinematicLerp);

        //if (!dontMoveCamera)
        //    AnimationCameraUpdate(false);

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
                if (GameObject.FindObjectOfType<TutorialManager>() == null ||
                    GameObject.FindObjectOfType<Tutorial_UI>() != null ||
                    GameObject.FindObjectOfType<Tutorial_Theory>() != null )
                {
                    RobotManager.SetUITriggerActive(true);
                }
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
            RobotManager.SetUITriggerActive(false);
        }

        savedRot = Camera.main.transform.rotation;
        targetRot = savedRot * Quaternion.Euler(25.0f, 0.0f, 0.0f);
    }

    public void SetCinematicMode(Transform target)
    {
        if (target.Find("CinematicTarget") == null)
        {
            return;
        }
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
