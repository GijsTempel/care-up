using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUp.Actions;

public class VideoPlayerManager : MonoBehaviour
{
    public Text TextFrameText;
    public Text TitleText;

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
    public Transform videoActionPanelContent;
    int videoSegment = 0;
    List<VideoActionUnit> videoActionUnits = new List<VideoActionUnit>();
    VideoActionManager videoActionManager;
    bool isFirstPlay = true;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Play();
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
            }
        }
        int currentFrame = 0;
        if (videoPlayer.length > 0)
        {
            videoScrollbar.fillAmount = (float)(videoPlayer.clockTime / videoPlayer.length);
            currentFrame = (int)(videoPlayer.clockTime / videoPlayer.length * videoPlayer.frameCount);
        }
        
        //Debug.Log(videoPlayer.clockTime / videoPlayer.length);
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
        sidePanelIsOpen = !sidePanelIsOpen;
    }

    public void OpenSIdePanel()
    {
        VideoPanelsAnimator.SetTrigger("OpenVideoSidePanel");
    }

    public void CloseSidePanel()
    {
        VideoPanelsAnimator.SetTrigger("CloseVideoSidePanel");
    }
    public void LoadMainMenu()
    {
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
                TitleText.text = videoActionUnits[i].GetTitle();
                TitleText.transform.parent.GetComponent<Animation>().Play();
            }
        }
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
    }   

    public void BuildVideoActionsPanel(VideoActionManager _videoActionManager)
    {
        videoActionManager = _videoActionManager;

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
    }

}
