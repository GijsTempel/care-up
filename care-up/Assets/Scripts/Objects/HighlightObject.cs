using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour {
    Transform target;
    HighlightControl hl_control;
    HighlightObject.type currentType = HighlightObject.type.NoChange;
    public GameObject content;
    public List<GameObject> BallElements;
    public List<GameObject> QubeElements;
    public List<GameObject> ArrowElements;

    float lifetime = float.PositiveInfinity;
    float startDelay = 0;

    public enum type
    {
        NoChange,
        Ball,
        Qube,
        Arrow
    };

    public void setType(HighlightObject.type _type)
    {
        currentType = _type;
        foreach (GameObject b in BallElements)
            b.SetActive(currentType == HighlightObject.type.Ball);
        foreach (GameObject q in QubeElements)
            q.SetActive(currentType == HighlightObject.type.Qube);
        foreach (GameObject a in ArrowElements)
            a.SetActive(currentType == HighlightObject.type.Arrow);
        if (_type == HighlightObject.type.Arrow)
        {
            transform.rotation = new Quaternion();
            transform.localScale = new Vector3(1,1,1);
        }
    }

    // Use this for initialization
    void Start () {
		
	}

    public void setStartDelay(float value)
    {
        if(value > 0)
        {
            content.SetActive(false);
            startDelay = value;
        }
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
            setType(hl_control.hl_type);
            transform.position = hl_control.transform.position;
            if (currentType != HighlightObject.type.Arrow)
            {   
                transform.rotation = hl_control.transform.rotation;
                transform.localScale = hl_control.transform.localScale;
            }
            
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
        if (currentType == HighlightObject.type.NoChange)
            setType(HighlightObject.type.Ball);
    }

    public void setTimer(float time)
    {
        lifetime = time;
    }

    public void Destroy()
    {
        GameObject.DestroyImmediate(gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (startDelay > 0)
            startDelay -= Time.deltaTime;
        else if (!content.activeSelf)
            content.SetActive(transform);

        if (target == null)
            Destroy();
        if (lifetime < 0f)
            GameObject.DestroyImmediate(gameObject);

        if (lifetime < float.PositiveInfinity)
            lifetime -= Time.deltaTime;
        if (target != null){
            if (hl_control != null)
            {
                transform.position = hl_control.transform.position;
                if (currentType != HighlightObject.type.Arrow)
                    transform.rotation = hl_control.transform.rotation;

            }
            else if (target.GetComponent<Collider>() != null)
            {
                Collider c = target.gameObject.GetComponent<Collider>();
                transform.position = c.bounds.center;
            }
            else
            {
                transform.position = target.position;
                if (currentType != HighlightObject.type.Arrow)
                    transform.rotation = target.rotation;
            }
        }
	}
}
