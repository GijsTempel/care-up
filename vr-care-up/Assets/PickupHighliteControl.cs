using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PickupHighliteControl : MonoBehaviour
{
    public List<HandPresence> handPresences;
    List<PickableObject> highlitedObjects = new List<PickableObject>();
    bool toUpdateHighlite = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < handPresences.Count; i++)
        {
            highlitedObjects.Add(null);
        }
    }

    void LateUpdate()
    {
        if (toUpdateHighlite)
        {
            List<PickableObject> nextHighliteObjects = new List<PickableObject>();
            toUpdateHighlite = false;
            foreach(HandPresence handPresence in handPresences)
            {
                PickableObject closestPickable = handPresence.FindClosestPickableInArea();
                nextHighliteObjects.Add(closestPickable);
            }
            for(int i = 0; i < highlitedObjects.Count; i++)
            {
                if (highlitedObjects[i] != null && !nextHighliteObjects.Contains(highlitedObjects[i]))
                {
                    if (highlitedObjects[i].GetComponent<OutilneControl>() != null)
                    {
                        highlitedObjects[i].GetComponent<OutilneControl>().ShowOutline(false);
                    }
                }
                if (nextHighliteObjects[i] != null && !highlitedObjects.Contains(nextHighliteObjects[i]))
                {
                    if (nextHighliteObjects[i].GetComponent<OutilneControl>() != null)
                    {
                        nextHighliteObjects[i].GetComponent<OutilneControl>().ShowOutline(true);
                    }
                }
            }
            highlitedObjects = nextHighliteObjects;
        }
    }

    public void InitUpdateHighlite()
    {
        toUpdateHighlite = true;

    }
}
