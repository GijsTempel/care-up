using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : MonoBehaviour {

    public float speed = 1.0f;
    public List<GameObject> KyePoints;
    int NextPointIndex = 0;
    public bool toWalk = false;

    // Use this for initialization
    void Awake()
    {
        if (NextPointIndex < KyePoints.Count)
            transform.LookAt(KyePoints[NextPointIndex].transform.position);
        
    } 
    public void SetKeyPoints(string PointHolderName)
    {
        if (GameObject.Find(PointHolderName) != null)
        {
            GameObject PointHolder = GameObject.Find(PointHolderName);
            if (PointHolder.GetComponent<ExtraObjectOptions>() != null)
            {
                KyePoints.Clear();
                foreach(GameObject g in PointHolder.GetComponent<ExtraObjectOptions>().hidenObjects)
                {
                    KyePoints.Add(g);
                }
                NextPointIndex = 0;
                toWalk = true;
            }
        }
    }

    public void StartWalking()
    {
        NextPointIndex = 0;
        toWalk = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (toWalk)
        {
            GameObject nextPoint = null;
            if (NextPointIndex < KyePoints.Count)
                nextPoint = KyePoints[NextPointIndex];
            else
            {
                toWalk = false;
            }
            float step = speed * Time.deltaTime;

            if (nextPoint != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPoint.transform.position, step);

                if (Vector3.Distance(transform.position, nextPoint.transform.position) < 0.001f)
                {

                    transform.position = nextPoint.transform.position;
                    NextPointIndex++;
                    transform.LookAt(KyePoints[NextPointIndex].transform.position);
                }
            }

        }
    }
}
