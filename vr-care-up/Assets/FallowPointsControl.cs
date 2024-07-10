using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallowPointsControl : MonoBehaviour
{
    public Transform pointHolder;
    public Transform testObject;
    public float distanceA = 0.2f;
    List<FallowPoint> fallowPoints = new List<FallowPoint>();
    List<float> distances = new List<float>();
    float fullPathDistance = 1f;
    int currentPointIndex = 0;
    float minTrackDist = 0.1f;
    float progresNormalized = 0f;
    float progresSmoothNormalized = 0f;


    void BuildPointList()
    {
        currentPointIndex = 0;
        fallowPoints.Clear();
        distances.Clear();
        fullPathDistance = distanceA;
        distances.Add(distanceA);
        int extraCounter = 0;
        for (int i = 0; i < pointHolder.childCount; i++)
        { 
            if (pointHolder.GetChild(i).GetComponent<FallowPoint>() != null)
            {
                fallowPoints.Add(pointHolder.GetChild(i).GetComponent<FallowPoint>());
                if (fallowPoints.Count > 1)
                {
                    float nextDist = Vector3.Distance(fallowPoints[extraCounter].transform.position,
                        fallowPoints[extraCounter - 1].transform.position);
                    distances.Add(nextDist);
                    fullPathDistance += nextDist;
                }
                extraCounter++;
            }
        }
    }

    void UpdatePointsScale()
    {
        float baseScale = 1f;
        for (int i = 0; i < fallowPoints.Count; i++)
        {
            if (i == currentPointIndex)
            {
                fallowPoints[i].transform.localScale = Vector3.one *  baseScale;
            }
            else
            {
                fallowPoints[i].transform.localScale = Vector3.one *  baseScale * 0.2f; 
            }
        }
    }

    void Start()
    {
        BuildPointList();
        UpdatePointsScale();
    }

    void CalculateProgress(float currentDist)
    {
        if (currentPointIndex >= distances.Count)
        {
            progresNormalized = 1f;
            progresSmoothNormalized = Mathf.Lerp(progresSmoothNormalized, progresNormalized, Time.deltaTime * 100f);
            return;
        }
        
        float trackedDist = 0;
        for (int i = currentPointIndex; i < distances.Count; i++)
        {
            if (i == currentPointIndex)
            {
                if (currentDist > distances[i])
                    trackedDist += distances[i];
                else
                    trackedDist += currentDist;
            }
            else
            {
                trackedDist += distances[i];
            }
        }
        float newProgresNormalized = 1f - (trackedDist / fullPathDistance);
        if (newProgresNormalized > progresNormalized)
            progresNormalized = newProgresNormalized;
        progresSmoothNormalized = Mathf.Lerp(progresSmoothNormalized, progresNormalized, Time.deltaTime * 30f);
    }

    void Update()
    {
        if (testObject == null)
            return;
        float pointTrackDist = 0f;
        bool toNextPoint = false;
        if (fallowPoints.Count > currentPointIndex)
        {
            pointTrackDist = Vector3.Distance(fallowPoints[currentPointIndex].transform.position,
                testObject.position);
            // Debug.Log(pointTrackDist);
            toNextPoint = pointTrackDist < minTrackDist;
        }
        UpdatePointsScale();
        CalculateProgress(pointTrackDist);
        Debug.Log(progresSmoothNormalized);
        if (toNextPoint)
            currentPointIndex += 1;
    }

    public float GetProgress()
    {
        return progresSmoothNormalized;
    }
}
