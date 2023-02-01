﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class VideoPlayerBar : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Image circle;
    [SerializeField]
    private Image progress;
    private void Awake()
    {

    }

    void Update()
    {
        circle.gameObject.SetActive(videoPlayer.length > 0);
        if (videoPlayer.length > 0)
        {
            progress.fillAmount = (float)(videoPlayer.clockTime / videoPlayer.length);
            Vector3 p = circle.rectTransform.anchoredPosition;
            p.x = (progress.rectTransform.rect.xMax - progress.rectTransform.rect.xMin) * progress.fillAmount -
                progress.rectTransform.rect.xMax;
            circle.rectTransform.anchoredPosition = p;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    private void TrySkip(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(progress.rectTransform, 
            eventData.position, camera, out localPoint))
        {
            float pct = Mathf.InverseLerp(progress.rectTransform.rect.xMin, 
                progress.rectTransform.rect.xMax, localPoint.x);
            SkipToProcent(pct);
        }
    }
    private void SkipToProcent(float pct)
    {
        var frame = videoPlayer.frameCount * pct;
        videoPlayer.frame = (long)frame;
    }
}
