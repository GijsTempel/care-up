using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedPopUp : MonoBehaviour
{
    public float time = 5.0f;

    private float timer = 0.0f;
    private bool set = false;

    private Image panel = null;
    private Text text = null;

    private static List<TimedPopUp> popUps = new List<TimedPopUp>();

    public string MistakeMsg
    {
        get { return text == null ? "" : text.text; }
    }

    private void Start()
    {
        popUps.Add(this);

        Transform btn = transform.Find("closeBTN");
        if (btn != null)
        {
            btn.GetComponent<Button>().onClick.AddListener(ForceHide);
        }
    }

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

    public static void ForceHide()
    {
        foreach (TimedPopUp tpu in popUps)
        {
            if (tpu != null)
                tpu.GetComponent<Animator>().SetBool("set", false);
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

        if (GetComponent<Animator>().GetBool("set"))
        {
            GetComponent<Animator>().SetTrigger("reset");
        }

        popUps.RemoveAll(item => item == null);

        foreach (TimedPopUp tpu in popUps)
        {
            if (tpu != null)
                tpu.GetComponent<Animator>().SetBool("set", false);
        }

        GetComponent<Animator>().SetBool("set", true);

        if (GameObject.Find("UI") != null)
        {
            if (GameObject.Find("UI").transform.Find("GameOver") != null)
            {
                GameObject.Find("UI").transform.Find("GameOver").GetComponent<GameOverUI>().SetDescription(text.text);
            }
        }
    }
}
