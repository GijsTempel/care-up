using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
/// <summary>
/// Handles entire tutorial scene step by step.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [HideInInspector]
    public bool movementLock = false;
    [HideInInspector]
    public bool mouseClickLocked = false;
    [HideInInspector]
    public bool closeObjectViewLocked = false;
    [HideInInspector]
    public bool leftDropKeyLocked = false;
    [HideInInspector]
    public bool rightDropKeyLocked = false;
    [HideInInspector]
    public bool combineKeyLocked = false;
    [HideInInspector]
    public bool getHintKeyLocked = false;
    [HideInInspector]
    public bool pickObjectViewKeyLocked = false;
    [HideInInspector]
    public bool leftUseKeyLocked = false;
    [HideInInspector]
    public bool rightUseKeyLocked = false;

    protected bool pauseEnabled = false;
    protected float pauseTimer = 0.0f;
    protected Text UItext;
    protected RectTransform hintsBox;
	protected TutorialHintsN hintsN;

    protected PlayerScript player;
    protected ActionManager actionManager;
    protected HandsInventory handsInventory;
    protected Controls controls;

    protected GameObject particleHint;
    protected GameObject particleHint_alt;
    
    [HideInInspector]
    public string itemToPick = "";
    [HideInInspector]
    public string itemToPick2 = "";
    [HideInInspector]
    public string itemToDrop = "";
    [HideInInspector]
    public string itemToDrop2 = "";

    protected GameObject endPanel;
    protected GameObject nextButton;
    protected bool nextButtonClicked = false;

    public bool finished = false;

    public bool TutorialEnding
    {
        get { return endPanel.activeSelf; }
    }
    

    void Awake () {
        particleHint = GameObject.Find("ParticleHint");
        particleHint.SetActive(false);
        particleHint_alt = GameObject.Find("ParticleHint (1)");
        particleHint_alt.SetActive(false);

        GameObject gameLogic = GameObject.Find("GameLogic");
        actionManager = gameLogic.GetComponent<ActionManager>();
        handsInventory = gameLogic.GetComponent<HandsInventory>();
        controls = gameLogic.GetComponent<Controls>();
        
        handsInventory.dropPenalty = false;
        
        endPanel = GameObject.Find("TutorialDonePanel");
		/*if (endPanel != null)
        {
			GameObject doneButton = endPanel.transform.Find("Button").gameObject;
            GameObject sl = GameObject.Find("SceneLoader 1");
            doneButton.GetComponent<Button>().onClick.AddListener(delegate { sl.GetComponent<bl_SceneLoader>().LoadLevelButton("MainMenu"); });
        }*/

	}

    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        

		if (GameObject.FindObjectOfType<TutorialHintsN>() != null)
		{
			hintsN = GameObject.FindObjectOfType<TutorialHintsN>();
			hintsBox = GameObject.Find("hintsBox").transform as RectTransform;
			nextButton = GameObject.Find("TutNextButton");
			UItext = GameObject.Find("TutHintsText").GetComponent<Text>();
		}   
	    else if (GameObject.Find("TutorialHints") != null)
        {
			hintsBox = GameObject.Find("TutorialHints").transform as RectTransform;
			nextButton = GameObject.Find("TutorialNextButton");
			UItext = GameObject.Find("TutorialHintsText").GetComponent<Text>();
        }
    }

    protected virtual void Update ()
    {
		if (pauseEnabled && pauseTimer > 0.0f)
        {
            pauseTimer -= Time.deltaTime;
        }
	}

    public void EndButtonClick()
    {
        // TUTORIAL COMPLETION, used only after last tutorial
        GameObject.FindObjectOfType<PlayerPrefsManager>().SetTutorialCompletedWU();
        bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
    }

    protected void SetPauseTimer(float value)
    {
        if (!pauseEnabled)
        {
            pauseEnabled = true;
            pauseTimer = value;
        }
    }

    protected bool TimerElapsed()
    {
        if (pauseEnabled && pauseTimer <= 0.0f)
        {
            pauseEnabled = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool Paused()
    {
        return pauseEnabled && pauseTimer > 0.0f;
    }

    protected void SetAllKeysLocked(bool value)
    {
        mouseClickLocked = value;
        closeObjectViewLocked = value;
        leftDropKeyLocked = value;
        rightDropKeyLocked = value;
        combineKeyLocked = value;
        getHintKeyLocked = value;
        pickObjectViewKeyLocked = value;
        leftUseKeyLocked = value;
        rightUseKeyLocked = value;
        controls.keyPreferences.SetAllLocked(value);
    }

    protected void AddPointWithSound()
    {
        actionManager.UpdatePoints(1);
        ActionManager.CorrectAction();
    }

    public void TutorialNextButton()
    {
        nextButtonClicked = true;
        nextButton.SetActive(false);
    }

    public void SetUpTutorialNextButton()
    {
        nextButtonClicked = false;
        nextButton.SetActive(true);
    }

    protected void TutorialEnd()
    {
        endPanel.SetActive(true);
        GameObject.FindObjectOfType<GameUI>().allowObjectControlUI = false;
        player.enabled = false;
        GameObject.FindObjectOfType<RobotManager>().enabled = false;
        foreach (InteractableObject o in GameObject.FindObjectsOfType<InteractableObject>())
        {
            o.Reset();
            o.enabled = false;
        }
        hintsBox.gameObject.SetActive(false);

        finished = true;
    }
}
