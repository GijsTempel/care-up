using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using UnityEngine.Rendering;

/// <summary>
/// Instance of dialogue with up to 4 options.
/// </summary>
public class SelectDialogue : MonoBehaviour
{
    public Image progressImage;

    /*public bool tutorial_lock = false;
    public bool cheated = false;*/
    int correctAnswerID = -1;
    //GameUI gameUI;
    public GameObject selectionDialogueElements;
    public RectTransform pointCursor;
    public Image pointImage;

    //public GameObject prevStepInfoElements;
    //public Button prevStepInfoButton;
    DialogueOption prevStepInfo = null;
    float progressCounter = -1f;
    const float MAX_PROGRESS_COUNTER = 2f;
    float fadeOutTimer;
    const float FADEOUT_MAX = 0.1f;

    float contactTimer;

    GameObject progressTarget;
    GameObject newProgressTarget;

    public class DialogueOption
    {
        public delegate void OptionAction(string attr = "", List<DialogueOption> additionalOption = null, string question = null, string audio = "");

        public string text;
        public string attribute;
        public string question;
        public string audio;
        public OptionAction function;
        public OptionSide side;
        public List<DialogueOption> additional;

        public DialogueOption(string txt, OptionAction func, string attr, string aud, List<DialogueOption> additionalOptions = null, string questionText = null)
        {
            text = txt;
            function = func;
            attribute = attr;
            audio = aud;
            question = questionText;
            additional = additionalOptions;
        }
    }

    Vector2 WorldToCanvasPos(Vector3 worldPos)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPos);
        float _scale = selectionDialogueElements.GetComponent<RectTransform>().localScale.x;
        return new Vector2(localPos.x / _scale, localPos.y / _scale);
    }

    public void PointerRayHit(Vector3 hitPos)
    {
        Vector2 canvasHitPos = WorldToCanvasPos(hitPos);
        fadeOutTimer = FADEOUT_MAX;
        pointCursor.anchoredPosition = canvasHitPos;
        GameObject sqButtonInPos = GetSqButtonInPosition(hitPos);
        if (sqButtonInPos != null)
        {
            progressImage.gameObject.SetActive(true);
            pointImage.color = Color.green;
            newProgressTarget = sqButtonInPos;
            contactTimer = FADEOUT_MAX / 2;
        }
        else
        {
            progressImage.gameObject.SetActive(false);
            pointImage.color = Color.gray;
        }
    }

    void Update()
    {
        if (fadeOutTimer > 0)
            fadeOutTimer -= Time.deltaTime;
        if (fadeOutTimer < 0)
        {
            fadeOutTimer = 0;
        }
        if (contactTimer > 0 && (contactTimer - Time.deltaTime <= 0))
            newProgressTarget = null;

        contactTimer -= Time.deltaTime;
        pointCursor.GetComponent<CanvasGroup>().alpha = fadeOutTimer / FADEOUT_MAX;
        if (progressTarget != newProgressTarget)
            progressCounter = MAX_PROGRESS_COUNTER;
        else if (progressTarget != null && progressCounter > -1f)
            progressCounter -= Time.deltaTime;

        progressTarget = newProgressTarget;
        if (progressTarget != null)
        {
            progressImage.fillAmount = 1f - Mathf.Clamp01(progressCounter / MAX_PROGRESS_COUNTER);
            if (progressCounter < 0 && progressCounter > -1f)
            {
                if (progressTarget.GetComponent<ActionTrigger>() != null)
                {
                    progressTarget.GetComponent<ActionTrigger>().AttemptTrigger();
                }
                progressCounter = -2f;
            }
        }
    }

    GameObject GetSqButtonInPosition(Vector3 pos)
    {
        for (int i = 0; i < sqButtons.Count; i++)
        {
            if (!sqButtons[i].activeSelf)
                continue;

            Vector3 localPos = sqButtons[i].transform.InverseTransformPoint(pos);
            if (sqButtons[i].GetComponent<RectTransform>().rect.Contains(localPos))
                return sqButtons[i];
        }
        return null;
    }

    public void ShowPrevStepInfo(bool toShow = true)
    {
        selectionDialogueElements.SetActive(!toShow);
        //prevStepInfoElements.SetActive(toShow);
    }
    public enum OptionSide
    {
        None,
        Top,
        Bottom,
        Right,
        Left
    };

    private List<DialogueOption> options = new List<DialogueOption>();
    //private OptionSide currentOption = OptionSide.None;
    private Color currentMaterial;
    public List<GameObject> sqButtons;
    private string text;
    //private static CameraMode cameraMode;
    //private static Controls controls;
    //private bool optionSelected = false;

    static DialogueOption optionWithAdditions = null;
    static string questionWithHint = null;



    public void AddPrevStepInfo(List<DialogueOption> list)
    {
        foreach (DialogueOption o in list)
        {
            if (o.attribute != "" && o.attribute != "CM_Leave")
            {
                prevStepInfo = o;
            }
        }

        /*if (prevStepInfo != null)
        {
            prevStepInfoElements.transform.Find("Buttons/b0/Text").GetComponent<Text>().text = prevStepInfo.text;
            prevStepInfoButton.gameObject.SetActive(true);
        }
        else
            prevStepInfoButton.gameObject.SetActive(false);*/

    }

    public void AddOptions(List<DialogueOption> list, bool cheat = false)
    {
        foreach (DialogueOption item in list)
        {
            options.Add(item);
        }
        InitOptions();
        //cheated = cheat;
    }

    /// <summary>
    /// Sets options sides based on how many are there.
    /// </summary>
    private void InitOptions()
    {
        /*if (gameUI == null)
            gameUI = GameObject.FindObjectOfType<GameUI>();*/

        if (options.Count == 0)
        {
            Debug.LogError("0 options inited.");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < options.Count)
            {
                sqButtons[i].gameObject.SetActive(true);
                /*string cheatSimbol = "";
#if UNITY_EDITOR
                ObjectsIDsController objectsIDsController = GameObject.FindAnyObjectByType<ObjectsIDsController>();
                if (objectsIDsController != null && objectsIDsController.cheat)
                {
                    if (options[i].attribute != "" && options[i].attribute != "CM_Leave")
                        cheatSimbol = "@";
                }
#endif*/
                sqButtons[i].transform.Find("Text").GetComponent<Text>().text = /*cheatSimbol +*/ options[i].text;
                sqButtons[i].GetComponent<ActionTrigger>().LeftActionManagerObject = options[i].attribute;
                /*if (gameUI.AllowAutoPlay(false))
                    if (options[i].attribute != "" && options[i].attribute != "CM_Leave")
                    {
                        correctAnswerID = i;
                        Invoke("AutoPlay", 3f);
                    }*/
            }
            else
                sqButtons[i].gameObject.SetActive(false);
        }
     
        //ShowAnswer();
    }

    /*void AutoPlay()
    {
        if (correctAnswerID >= 0)
        {
            if (sqButtons[correctAnswerID].gameObject.activeSelf)
                sqButtons[correctAnswerID].GetComponent<Button>().onClick.Invoke();
        }
    }*/

    public void SetText(string t)
    {
        text = t;
    }

    public void ShowAnswer()
    {
        int i = 0;
        foreach (DialogueOption o in options)
        {
            //print(o.attribute);
            if (o.attribute != "" && o.attribute != "CM_Leave")
            {
                sqButtons[i].GetComponent<Image>().color = Color.yellow;
            }
            i++;
        }
    }

    public void ButtonClick(int num)
    {
        //sqButtons[num].interactable = false;

        // main thing?
        //options[num].function(options[num].attribute, options[num].additional, options[num].question, options[num].audio);

        if (options[num].attribute != "")
        {
            Destroy(gameObject);
            //cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
        }
        else
        {
            /*ColorBlock newColorBlock = sqButtons[num].colors;
            newColorBlock.disabledColor = new Color(1f, 0f, 0f, 0.4f);
            sqButtons[num].colors = newColorBlock;*/

            GameObject.FindObjectOfType<ActionManager>().OnSequenceStepAction("");
            GameObject currentHintPanel = GameObject.Find("HintPanel");

            if (ActionManager.practiceMode && currentHintPanel != null)
            {
                string hintText = FindObjectOfType<ActionManager>().CurrentDescription[0];

                foreach (DialogueOption dialoqueOption in options)
                {
                    if (dialoqueOption.additional != null)
                    {
                        optionWithAdditions = dialoqueOption;
                        hintText = dialoqueOption.text;
                        break;
                    }
                }

                if (currentHintPanel.transform.Find("Text") != null)
                {
                    Text hint = currentHintPanel.transform.Find("Text").gameObject.GetComponent<Text>();

                    if (optionWithAdditions != null)
                    {
                        if (hint.text == optionWithAdditions.question || hint.text == questionWithHint)
                        {
                            questionWithHint = optionWithAdditions.question + " " + FindObjectOfType<ActionManager>().CurrentDescription[0];
                            hint.text = questionWithHint;
                        }
                        else if (options[num] == optionWithAdditions)
                            hint.text = optionWithAdditions.question;
                    }
                    else
                        hint.text = hintText;
                }
            }
        }
    }
}
