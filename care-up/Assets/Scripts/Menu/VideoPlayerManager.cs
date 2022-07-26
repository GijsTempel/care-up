using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUp.Actions;

public class VideoPlayerManager : MonoBehaviour
{
    public GameObject loadingScreenPanel;
    public GameObject InPlayButtonIcon;

    public Text extraText;
    public Text TextFrameText;
    public GameObject titlePanel;
    public Animator VideoPanelsAnimator;
    bool playState = false;
    bool fullScreenMode = false;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Button playButton;
    public Image videoScrollbar;
    public UnityEngine.Video.VideoPlayer videoPlayer;
    bool sidePanelIsOpen = false;
    bool actionDataLoaded = false;
    Transform videoActionPanelContent;
    public ScrollRect videoActionPanelScrollRect;
    int videoSegment = 0;
    List<VideoActionUnit> videoActionUnits = new List<VideoActionUnit>();
    VideoActionManager videoActionManager;
    bool initialized = false;
    bool isFirstPlay = true;
    int sceneComplition = 0;
    PlayerPrefsManager manager;
    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
            manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (manager != null)
        {
            string videoURL = VideoActionManager.baseURL + manager.videoSceneName + "/video.mp4";
            videoPlayer.source = UnityEngine.Video.VideoSource.Url;
            videoPlayer.url = videoURL;
        }
        videoPlayer.Prepare();
        videoPlayer.Play();
        ShowLoadingScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstPlay)
        {
            if (videoPlayer.length > 0)
            {
                isFirstPlay = false;
                videoPlayer.Pause();
                ShowLoadingScreen(false);
            }
        }
        int currentFrame = 0;
        if (videoPlayer.length > 0)
        {
            videoScrollbar.fillAmount = (float)(videoPlayer.clockTime / videoPlayer.length);
            currentFrame = (int)(videoPlayer.clockTime * 24);
            if (((float)sceneComplition / 100f) < videoScrollbar.fillAmount)
                sceneComplition = (int)(videoScrollbar.fillAmount * 100f);
        }
        string ss = "";
        ss += "videoPlayer.clockTime = " + videoPlayer.clockTime.ToString() + "\n";
        ss += "videoPlayer.length = " + videoPlayer.length.ToString() + "\n";
        ss += "videoPlayer.frameCount = " + videoPlayer.frameCount.ToString() + "\n";
        ss += "currentFrame = " + currentFrame.ToString() + "\n";
        if (videoActionManager != null)
        {
            foreach (VideoAction videoAction in videoActionManager.videoActions)
            {
                ss += videoAction.startFrame.ToString() + " | ";
            }

        }
        extraText.text = ss;
        if (initialized)
        {
            string segmentValueStr = "  0";
            int currentSegment = GetCurrentSegment(currentFrame);
            segmentValueStr = "  " + currentSegment.ToString();
            if (videoPlayer.isPlaying)
            {

                if (currentSegment != videoSegment)
                {
                    videoSegment = currentSegment;
                    HighlightCurrentUnit();
                }
            }
            TextFrameText.text = currentFrame.ToString() + segmentValueStr;
        }

    }

    public void SaveComplitionValue()
    {
        DatabaseManager.UpdateCompletedSceneScore(manager.currentSceneVisualName, 0, sceneComplition);
    }
    void ShowLoadingScreen(bool toShow = true)
    {
        loadingScreenPanel.SetActive(toShow);
    }

    int GetCurrentSegment(int currentFrame)
    {
        int currentSegment = 0;
        foreach(VideoAction videoAction in videoActionManager.videoActions)
        {
            if (currentFrame < videoAction.startFrame)
            {
                break;
            }
            currentSegment++;
        }
        if (currentSegment > 0)
            currentSegment = currentSegment - 1;
        return currentSegment;
    }

    public void PlayButtonClicked()
    {
        playState = !playState;
        if (playState)
        {
            videoPlayer.Play();
            playButton.GetComponent<Image>().sprite = pauseSprite;
        }
        else
        {
            videoPlayer.Pause();
            playButton.GetComponent<Image>().sprite = playSprite;
        }
        InPlayButtonIcon.SetActive(!playState);
    }

    public void ToggleSidePanel()
    {
        if (sidePanelIsOpen)
        {
            CloseSidePanel();
        }
        else
        {
            OpenSIdePanel();
        }
        
    }

    public void OpenSIdePanel()
    {
        VideoPanelsAnimator.SetTrigger("OpenVideoSidePanel");
        sidePanelIsOpen = true;
    }

    public void CloseSidePanel()
    {
        VideoPanelsAnimator.SetTrigger("CloseVideoSidePanel");
        sidePanelIsOpen = false;

    }
    public void LoadMainMenu()
    {
        GameObject.FindObjectOfType<MainMenuAutomationData>().toAutomate = true;
        DatabaseManager.UpdateField("AccountStats", "TutorialCompleted", "true");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
    }

    void HighlightCurrentUnit()
    {

        for (int i = 0; i < videoActionUnits.Count; i++)
        {
            videoActionUnits[i].HighlightUnit(i == videoSegment);
            if (i == videoSegment)
            {
                titlePanel.transform.Find("TitleText").GetComponent<Text>().text = videoActionUnits[i].GetTitle();
                titlePanel.transform.Find("Desc").GetComponent<Text>().text = videoActionUnits[i].GetDescription();

                //infoEffectPanel.transform.Find("VideoActionUnit/Title").GetComponent<Text>().text = videoActionUnits[i].GetTitle();
                //infoEffectPanel.transform.Find("VideoActionUnit/Desc").GetComponent<Text>().text = videoActionUnits[i].GetDescription();
                if (videoSegment != 0 && !sidePanelIsOpen)
                {
                    titlePanel.GetComponent<Animation>().Stop();
                    titlePanel.GetComponent<Animation>().Play();
                    //infoEffectPanel.GetComponent<Animation>().Play();
                }
            }
        }
        float scrollPos = 1f - ((float)videoSegment / (float)(videoActionUnits.Count - 1));
        videoActionPanelScrollRect.verticalNormalizedPosition = scrollPos;
    }
    public void NextPrevSegment(int segmentDirection = 1)
    {
        JumpToSegment(videoSegment + segmentDirection);
    }

    public void JumpToSegment(int _segment)
    {
        if (_segment < 0)
            _segment = 0;
        if (_segment >= videoActionManager.videoActions.Count)
            return;
            //_segment = videoActionManager.videoActions.Count - 1;
        videoPlayer.frame = videoActionManager.videoActions[_segment].startFrame;
        videoSegment = _segment;
        HighlightCurrentUnit();
        if (videoPlayer.isPaused)
        {
            isFirstPlay = true;
            videoPlayer.Play();
        }
    }   

    public void BuildVideoActionsPanel(VideoActionManager _videoActionManager)
    {
        videoActionManager = _videoActionManager;
        videoActionPanelContent = videoActionPanelScrollRect.transform.Find("Protocols/content");
        int actionID = 0;
        foreach (VideoAction videoAction in videoActionManager.videoActions)
        {
            GameObject videoActionUnitInstance = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/VideoActionUnit"), videoActionPanelContent);
            VideoActionUnit videoActionUnit = videoActionUnitInstance.GetComponent<VideoActionUnit>();
            videoActionUnit.SetValues(actionID, videoAction.title, videoAction.description);
            videoActionUnits.Add(videoActionUnit);
            videoActionUnit.SetVideoPlayerManager(this);
            actionID++;
        }
        HighlightCurrentUnit();
        initialized = true;
    }

}
