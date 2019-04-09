using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour {
    public Transform target;
    HighlightObject.type currentType;
    float timeLeft = float.PositiveInfinity;
    public enum type
    {
        Ball
    };

    public void setType(HighlightObject.type _type)
    {
        currentType = _type;
    }
    // Use this for initialization
    void Start () {
		
	}

    public void setTarget(Transform t)
    {
        target = t;
        name = "hl_" + target.name;
    }

    public void setTimer(float time)
    {
        timeLeft = time;
    }

    public void Destroy()
    {
        GameObject.DestroyImmediate(gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (timeLeft < 0f)
            GameObject.DestroyImmediate(gameObject);
        if (timeLeft < float.PositiveInfinity)
            timeLeft -= Time.deltaTime;

		if (target != null)
        {
            if (target.GetComponent<Collider>() != null)
            {
                Collider c = target.gameObject.GetComponent<Collider>();
                transform.position = c.bounds.center;
            }
            else
            {
                transform.position = target.position;
            }
        }
	}
}
