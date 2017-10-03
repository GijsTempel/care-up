using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedPopUp : MonoBehaviour {

    private float timer = 0.0f;
    private float time = 0.0f;
    private bool set = false;

    private Image panel = null;
    private Text text = null;
    
    void Update()
    {
        if (set)
        {
            timer += Time.deltaTime;
            if (timer >= time)
            {
                gameObject.SetActive(false);
                set = false;
            }
        }
    }

    public void Set(float _time, string _text)
    {
        if (panel == null)
        {
            panel = GetComponent<Image>();
            text = transform.GetChild(0).GetComponent<Text>();
        }

        gameObject.SetActive(true);

        timer = 0.0f;
        time = _time;
        set = true;

        text.text = _text;
    }
}
