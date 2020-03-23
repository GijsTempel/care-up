using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Transform objectToSwipe;

    [SerializeField]
    private GameObject panelHolder;

    [SerializeField]
    private Sprite dotSprite;

    [SerializeField]
    private GameObject dotPanel;

    [SerializeField]
    private float percentThreshold = 0.2f;

    [SerializeField]
    private float easing = 0.3f;

    private bool moved = false;
    private int pagesCount;
    private int index = 0;
    private float offset = 50f;
    private float pageWidth = 0f;

    private Vector3 panelLocation;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private List<GameObject> dots;

    private bool BoundariesCheck
    {
        get
        {
            if (objectToSwipe != null)
            {
                return (objectToSwipe.localPosition.x < (startPosition.x + offset))
                    && (objectToSwipe.localPosition.x > (EndPositionX - offset));
            }
            else return false;
        }
    }

    private float EndPositionX
    {
        get
        {
            return -pageWidth * (pagesCount - 1);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (BoundariesCheck)
        {
            if (!moved)
            {
                float difference = (data.pressPosition.x - data.position.x) / 5;
                currentPosition = panelLocation - new Vector3(difference, 0, 0);

                if (objectToSwipe != null)
                    objectToSwipe.localPosition = currentPosition;

                float percentage = (data.pressPosition.x - data.position.x) / Screen.width;

                if ((Mathf.Abs(percentage) >= percentThreshold) && !moved)
                {
                    Vector3 newLocation = panelLocation;

                    if (percentage > 0)
                    {
                        index++;
                        newLocation += new Vector3(-pageWidth, 0, 0);
                    }
                    else if (percentage < 0)
                    {
                        index--;
                        newLocation += new Vector3(pageWidth, 0, 0);
                    }

                    StartCoroutine(SmoothMove(currentPosition, newLocation, easing));
                    panelLocation = newLocation;
                    UpdateDots();

                    moved = true;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (!moved)
        {
            ChangePosition(data.pressPosition, data.position);
        }

        moved = false;
    }

    private void Start()
    {
        if (objectToSwipe != null)
        {
            panelLocation = objectToSwipe.localPosition;
            startPosition = panelLocation;
        }

        if (panelHolder != null)
        {
            pagesCount = panelHolder.transform.childCount;

            if (pagesCount > 0)
            {
                if (panelHolder.transform.GetChild(0) != null)
                {
                    if (panelHolder.transform.GetChild(0).gameObject.GetComponent<RectTransform>() != null)
                    {
                        pageWidth = panelHolder.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x;
                    }
                }
            }
        }

        InstantiateDots();
    }

    private void UpdateDots()
    {
        foreach (GameObject d in dots)
        {
            d.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
        }

        if (dots[index] != null)
        {
            dots[index].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }
    }

    private void ChangePosition(Vector2 pressPosition, Vector2 position)
    {
        float percentage = (pressPosition.x - position.x) / Screen.width;

        if (Mathf.Abs(percentage) >= percentThreshold && BoundariesCheck)
        {
            Vector3 newLocation = panelLocation;

            if (percentage > 0)
            {
                index++;
                newLocation += new Vector3(-pageWidth, 0, 0);
            }
            else if (percentage < 0)
            {
                index--;
                newLocation += new Vector3(pageWidth, 0, 0);
            }

            StartCoroutine(SmoothMove(currentPosition, newLocation, easing));
            panelLocation = newLocation;
            UpdateDots();
        }
        else
        {
            StartCoroutine(SmoothMove(currentPosition, panelLocation, easing));
        }
    }

    private IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float time = 0f;
        while (time <= 1.0)
        {
            time += Time.deltaTime / seconds;
            objectToSwipe.localPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, time));
            yield return null;
        }
    }

    private void InstantiateDots()
    {
        dots = new List<GameObject>();

        for (int i = 0; i < pagesCount; i++)
        {
            GameObject dot = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/dotTut"), dotPanel.transform) as GameObject;
            dot.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            dot.transform.SetParent(dotPanel.transform);
            dots.Add(dot);
        }

        UpdateDots();
    }
}
