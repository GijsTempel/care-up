using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour {
    Transform target;
    HighlightControl hl_control;
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

        if (target.GetComponentInChildren<HighlightControl>() != null)
            hl_control = target.GetComponentInChildren<HighlightControl>();
        else
            hl_control = null;

        if (hl_control != null)
        {
            transform.position = hl_control.transform.position;
            transform.rotation = hl_control.transform.rotation;
            transform.localScale = hl_control.transform.localScale;
            setType(hl_control.hl_type);
        }
        
        else if (target.GetComponent<Collider>() != null)
        {
            Collider c = target.gameObject.GetComponent<Collider>();
            transform.position = c.bounds.center;
        }
        else
        {
            transform.position = target.position;
        }
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
            if (hl_control != null)
            {
                transform.position = hl_control.transform.position;
            }
            else if(target.GetComponent<Collider>() != null)
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
