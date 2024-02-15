using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MountGhostHighlit : MonoBehaviour
{
    const long ghostTimeout = 100;
    public List<GameObject> ghostObjects = new List<GameObject>();
    Dictionary<string, long> ghostTimeStamps = new Dictionary<string, long>();

    void Start()
    {
        UpdateGhostObjects();
    }

    void UpdateGhostObjects()
    {
        long currentTimeMil = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

        foreach(GameObject ghostObject in ghostObjects)
        {
            if (ghostTimeStamps.Keys.Contains(ghostObject.name))
            {
                if ((ghostTimeStamps[ghostObject.name] + ghostTimeout) > currentTimeMil)
                {
                    ghostObject.SetActive(true);
                }
                else
                {
                    ghostTimeStamps.Remove(ghostObject.name);
                    ghostObject.SetActive(false);
                }
            }
            else
            {
                ghostObject.SetActive(false);
            }
        }
    }


    void Update()
    {
        if (ghostTimeStamps.Count > 0)
            UpdateGhostObjects();
    }

    GameObject GetGhostObject(string ghostName)
    {
        foreach(GameObject g in ghostObjects)
        {
            if (g.name == ghostName)
                return g;
        }
        return null;
    }

    public void ShowGhost(string ghostName)
    {
        GameObject currentGhostObject = GetGhostObject(ghostName);
        if (currentGhostObject == null)
            return;
        currentGhostObject.SetActive(true);
        long currentTimeMil = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        ghostTimeStamps[ghostName] = currentTimeMil;
    }

    public void HideGhost(string ghostName)
    {
        GameObject currentGhostObject = GetGhostObject(ghostName);
        if (currentGhostObject == null)
            return;
        currentGhostObject.SetActive(false);
        if (ghostTimeStamps.Keys.Contains(ghostName))
            ghostTimeStamps.Remove(ghostName);
    }
}
