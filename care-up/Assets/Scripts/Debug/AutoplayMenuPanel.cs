using UnityEngine;
using UnityEngine.UI;

public class AutoplayMenuPanel : MonoBehaviour
{
    public GameObject panel;
    GameObject StartButton;
    GameObject PauseButton;
    Text TimeLeftText;

    AutoPlayer autoPlayer;
    float TimeLeft = -30f;
    // Start is called before the first frame update
    bool countDoun = false;

    public void Start()
    {
        Init();
    }

    void Init()
    {
        autoPlayer = GameObject.FindObjectOfType<AutoPlayer>();
        StartButton = panel.transform.Find("StartAutoplay").gameObject;
        PauseButton = panel.transform.Find("PauseAutoplay").gameObject;

        TimeLeftText = panel.transform.Find("TimeLeft").GetComponent<Text>();
        SetButtonState();
    }
    private void Update()
    {
        if (autoPlayer != null)
        {
            if (!autoPlayer.toStartAutoplaySession || autoPlayer.GetSceneListSize() == 0 || TimeLeft < -10f)
            {
                SetButtonState();
                countDoun = false;
                return;
            }
            if (countDoun)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft > 0)
                {
                    TimeLeftText.text = TimeLeftToStr();

                }
                else
                {
                    TimeLeft = -15f;
                    TimeLeftText.text = "START...";
                    autoPlayer.StartAutoplaySession();
                    countDoun = false;
                }
            }
        }
    }

    string TimeLeftToStr()
    {
        return "00:" + ((int)TimeLeft).ToString("D2");
    }

    public void SetButtonState(bool isStartButton = true)
    {
        if (autoPlayer == null)
            Init();
        StartButton.SetActive(isStartButton);
        PauseButton.SetActive(!isStartButton);
        if (isStartButton)
            TimeLeftText.text = "";
    }

    public void StartButtonPressed()
    {
        if (autoPlayer == null)
            Init();
        autoPlayer.toStartAutoplaySession = true;
        SetButtonState(false);
        countDoun = true;
        TimeLeft = 5f;
        TimeLeftText.text = TimeLeftToStr();
    }

    public void PauseButtonPressed()
    {
        if (autoPlayer == null)
            Init();
        autoPlayer.toStartAutoplaySession = false;
        SetButtonState();
        countDoun = false;
        TimeLeftText.text = "";
    }

    void OnEnable()
    {
        if (autoPlayer == null)
            Init();
        panel.SetActive(PlayerPrefsManager.simulatePlayerActions);
#if !(UNITY_EDITOR || DEVELOPMENT_BUILD)
        panel.SetActive(false);
#endif

        if (autoPlayer.toStartAutoplaySession)
            StartButtonPressed();
    }
}
