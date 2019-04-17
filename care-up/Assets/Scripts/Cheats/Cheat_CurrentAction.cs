using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Developer script. Attach to a dev GameObject, that will be disabled for a release version of the game.
/// Displays a label with a description of a current action on the top center of the screen.
/// </summary>
public class Cheat_CurrentAction : MonoBehaviour
{
    private Text textObjectBiggerDevHint;
    public Text extraText;

    private GameObject extraPanel;
    private Image hintPanelBackground;
    private Image fullScreenButtonBackground;

    [SerializeField] private Text hintPanelText;
    [SerializeField] private GameObject fullScreenButton;

    [SerializeField] private GameObject hintPanel;
    [SerializeField] private GameObject bigger_DevHint;
    [SerializeField] private GameObject ipadTrigger;

    // never used
    //private bool biggerDevHintActive = false;
    //private bool devHintActive = true;

    private float animationTime = 1.0f;

    private int direction;
    private float timer;
    Button extraButton;

    private ActionManager actionManager;
    private Animator controller;

    private float uipanel_animationTime = 0.5f;
    private int uipanel_direction = 1;
    private float uipanel_timer = 1;

    private bool set = false; // fix

    [HideInInspector]
    public bool tutorial_extraOpened = false;
    [HideInInspector]
    public bool tutorial_extraClosed = false;
    [HideInInspector]
    public bool tutorial_devHintOpened = false;
    [HideInInspector]
    public bool tutorial_devHintClosed = false;

    private PlayerPrefsManager manager;

    Tutorial_UI tutorial_UI;

    void Start()
    {
        if (GameObject.Find("Preferences") != null)
        {
            manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
            if (manager == null) Debug.LogWarning("No prefs manager ( start from 1st scene? )");
        }
        else
        {
            Debug.LogWarning("No prefs manager ( start from 1st scene? )");
        }

        extraPanel = GameObject.Find("Extra").gameObject;
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");

        if (GameObject.Find("DevHint") != null)
        {
            Init();

            if (GameObject.Find("Preferences") != null)
            {

                if ((!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().practiceMode &&
                     actionManager.GetComponent<TutorialManager>() == null) || (FindObjectOfType<TutorialManager>() != null && FindObjectOfType<Tutorial_UI>() == null))

                {
                    GameObject.Find("DevHint").SetActive(false);
                    extraPanel.SetActive(false);
                    extraButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Game needs to be started from menu scene for CurrentAction hint to work correctly");
            }
        }

        if (GameObject.Find("BiggerDevHint") != null)
        {
            Init();

            if (GameObject.Find("Preferences") != null)
            {

                if ((!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().practiceMode &&
                    actionManager.GetComponent<TutorialManager>() == null) || (FindObjectOfType<TutorialManager>() != null && FindObjectOfType<Tutorial_UI>() == null))

                {
                    GameObject.Find("BiggerDevHint").SetActive(false);
                    extraPanel.SetActive(false);
                    extraButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Game needs to be started from menu scene for CurrentAction hint to work correctly");
            }
        }

        timer = 0.0f;
        direction = 0;
        uipanel_timer = 1.0f;
        uipanel_direction = 1;

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
    }

    private void Init()
    {
        GameObject biggerDevHint = GameObject.Find("BiggerDevHint");
        textObjectBiggerDevHint = biggerDevHint.transform.GetChild(2).GetComponent<Text>();

        biggerDevHint.SetActive(false);

        extraText = extraPanel.transform.GetChild(1).GetComponent<Text>();
        extraPanel.SetActive(false);
        set = false;

        extraButton = GameObject.Find("ExtraButton").GetComponent<Button>();
        extraButton.onClick.AddListener(ToggleExtraInfoPanel);

        Button extraCloseBtn = extraPanel.transform.Find("CloseExtra").GetComponent<Button>();
        Button extra_Close_Btn = extraPanel.transform.Find("CloseExtraCheckmark").GetComponent<Button>();
        extraCloseBtn.onClick.AddListener(ToggleExtraInfoPanel);
        extra_Close_Btn.onClick.AddListener(ToggleExtraInfoPanel);

        hintPanelBackground = hintPanel.GetComponent<Image>();

        fullScreenButtonBackground = fullScreenButton.GetComponent<Image>();
    }

    private void Update()
    {
        if (hintPanelText == null)
            return;

        if (textObjectBiggerDevHint == null)
            return;

        if (!set)
        {
            hintPanelText.text = actionManager.CurrentDescription.Remove(actionManager.CurrentDescription.Length - 1); ;
            textObjectBiggerDevHint.text = actionManager.CurrentDescription;
            if (extraText != null)
                extraText.text = actionManager.CurrentExtraDescription;
            set = true;
        }

        if (direction == 1)
        {
            if (timer < animationTime)
            {
                timer += Time.deltaTime;
                hintPanelText.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                textObjectBiggerDevHint.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                extraText.color = new Color(0.0f, 0.0f, 0.0f, 0.0f - timer / animationTime);
            }
            else
            {
                hintPanelText.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                textObjectBiggerDevHint.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                extraText.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                hintPanelText.text = actionManager.CurrentDescription.Remove(actionManager.CurrentDescription.Length -1);
                textObjectBiggerDevHint.text = actionManager.CurrentDescription;
                extraText.text = actionManager.CurrentExtraDescription;
                timer = animationTime;
                direction = -1;
            }
        }
        else if (direction == -1)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                hintPanelText.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);

                textObjectBiggerDevHint.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                extraText.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
            }
            else
            {
                hintPanelText.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

                textObjectBiggerDevHint.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                extraText.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                timer = 0.0f;
                direction = 0;
            }
        }

        if (controller == null && GameObject.FindObjectOfType<PlayerScript>() != null)
        {
            controller = GameObject.FindObjectOfType<PlayerScript>()
                .transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }

        if (controller != null)
        {
            uipanel_direction = (controller.GetCurrentAnimatorStateInfo(0).IsTag("UI_On") ||
                (controller.GetCurrentAnimatorStateInfo(0).IsTag("UI_On_Sequence")
                && controller.speed == 0)) ? 1 : -1;

            // very hard mix with everything else that updates text, needs testing
            uipanel_timer += Time.deltaTime * uipanel_direction;
            uipanel_timer = Mathf.Clamp(uipanel_timer, 0, uipanel_animationTime);

            float alpha = uipanel_timer / uipanel_animationTime;

            if (direction == 0 || uipanel_timer != uipanel_animationTime)
            {
                hintPanelText.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            }

            hintPanelBackground.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            fullScreenButtonBackground.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }

    public void UpdateAction()
    {
        direction = 1;
        if (extraPanel != null)
        {
            extraPanel.SetActive(false);
        }
    }

    public void ToggleExtraInfoPanel()
    {
        if (tutorial_UI != null && tutorial_UI.expectedHintsState == extraPanel.activeSelf)
        {
            return;
        }

        extraPanel.SetActive(!extraPanel.activeSelf);


        if (extraPanel.activeSelf)
        {
            tutorial_extraOpened = true;
        }
        else
        {
            tutorial_extraClosed = true;
        }
    }

    public void ShowBiggerDevHint()
    {
        tutorial_devHintOpened = true;
        bigger_DevHint.SetActive(true);

        //biggerDevHintActive = true;
        //devHintActive = false;
    }

    public void RemoveBiggerDevHint()
    {
        tutorial_devHintClosed = true;
        bigger_DevHint.SetActive(false);

        //biggerDevHintActive = false;
        //devHintActive = true;
    }

    public void RemoveDevHint()
    {
        if (manager.practiceMode == true)
        {
            hintPanel.SetActive(false);
        }
    }

    public void ShowDevHint()
    {
        if (manager.practiceMode == true)
        {
            hintPanel.SetActive(true);
        }
    }
}
