using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VideoActionUnit : MonoBehaviour
{
    public int index = -1;
    public GameObject highlight;
    public Text description;
    public Text title;
    VideoPlayerManager videoPlayerManager;
    void Start()
    {
        
    }

    public void SetTitle(string _title)
    {
        title.text = _title;
    }

    public void SetVideoPlayerManager(VideoPlayerManager _videoPlayerManager)
    {
        videoPlayerManager = _videoPlayerManager;
    }

    public void SetDescription(string _description)
    {
        description.text = _description;
    }

    public void SetValues(int _index, string _title, string _description)
    {
        title.text = _title;
        description.text = _description;
        index = _index;
    }


    public string GetTitle()
    {
        return title.text;
    }

    public string GetDescription()
    {
        return description.text;
    }
    public void ButtonClicked()
    {
        if (videoPlayerManager != null)
        {
            videoPlayerManager.JumpToSegment(index);
        }
    }

    public void HighlightUnit(bool toHighlight)
    {
        highlight.SetActive(toHighlight);
    }
}
