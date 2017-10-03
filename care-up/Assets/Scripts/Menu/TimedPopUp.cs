using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedPopUp : MonoBehaviour {

    public float time = 5.0f;

    private float timer = 0.0f;
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
                //gameObject.SetActive(false);
                set = false;
                GetComponent<Animator>().SetBool("set", false);
            }
        }
    }

    public void Set(string _text)
    {
        if (panel == null)
        {
            panel = GetComponent<Image>();
            text = transform.Find("Text").GetComponent<Text>();
        }

        gameObject.SetActive(true);

        timer = 0.0f;
        set = true;

        text.text = _text;

        GetComponent<Animator>().SetBool("set", true);
    }
}
