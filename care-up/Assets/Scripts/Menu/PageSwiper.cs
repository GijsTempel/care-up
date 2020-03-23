using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;

    [SerializeField]
    private Transform objectToSwipe;

    [SerializeField]
    private float percentThreshold = 0.2f;

    [SerializeField]
    private float easing = 0.3f;

    private Vector3 startPosition;

    private Vector3 endPosition = new Vector3(-3200f, 0f, 0f);

    private Vector3 currentPosition;

    private bool moved = false;

    [SerializeField]
    private Sprite currentDotSprite;

    [SerializeField]
    private Sprite dotSprite;

    [SerializeField]
    private GameObject dotPanel;

    private List<GameObject> dots = new List<GameObject>();
    private int pagesCount = 3;

    private int index = 0;

    private void DotsInstantiating()
    {
        for (int i = 0; i < pagesCount; i++)
        {
            GameObject dot = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/dotTut"), dotPanel.transform) as GameObject;
            dot.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            dot.transform.SetParent(dotPanel.transform);
            dots.Add(dot);
        }

        UpdateDots();
    }

    private void UpdateDots()
    {
        foreach (GameObject d in dots)
        {
            d.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
        }
        dots[index].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }

    private void Start()
    {
        startPosition = objectToSwipe.localPosition;
        panelLocation = objectToSwipe.localPosition;

        DotsInstantiating();
    }

    public void OnDrag(PointerEventData data)
    {
        if ((objectToSwipe.localPosition.x < 50f) && (objectToSwipe.localPosition.x > -3250f))
        {
            if (!moved)
            {
                float difference = (data.pressPosition.x - data.position.x) / 5;
                currentPosition = panelLocation - new Vector3(difference, 0, 0);
                objectToSwipe.localPosition = currentPosition;

                float percentage = (data.pressPosition.x - data.position.x) / Screen.width;

                if ((Mathf.Abs(percentage) >= percentThreshold) && !moved)
                {
                    Vector3 newLocation = panelLocation;

                    if (percentage > 0)
                    {
                        index++;
                        newLocation += new Vector3(-1600f, 0, 0);
                    }
                    else if (percentage < 0)
                    {
                        index--;
                        newLocation += new Vector3(1600f, 0, 0);
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

    private IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            objectToSwipe.localPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    private void ChangePosition(Vector2 pressPosition, Vector2 position)
    {
        float percentage = (pressPosition.x - position.x) / Screen.width;

        if (Mathf.Abs(percentage) >= percentThreshold && (objectToSwipe.localPosition.x < 50f) && (objectToSwipe.localPosition.x > -3250f))
        {
            Vector3 newLocation = panelLocation;

            if (percentage > 0)
            {
                index++;
                newLocation += new Vector3(-1600f, 0, 0);
            }
            else if (percentage < 0)
            {
                index--;
                newLocation += new Vector3(1600f, 0, 0);
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
}
