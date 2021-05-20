﻿using UnityEngine;
using UnityEngine.UI;

public class EndScoreRadial : MonoBehaviour
{
    public GameObject ovation;
    public GameObject sadMusic;

    Text ScoreProgressNum;
    Image ScoreProgressImage;
    float animStartTime;
    float animationTime = 3.0f;
    public bool animationFinished = false;
    Animator animator;

    public float score = 70f;
    float lastScore ;

    void Start()
    {
        animator = GetComponent<Animator>();
        ScoreProgressNum = transform.Find("ScoreProgressNum").GetComponent<Text>();
        ScoreProgressImage = transform.Find("ScoreProgressImage").GetComponent<Image>();
        //StartAnimation(70f);
    }

    public void StartAnimation(float value)
    {
        animationFinished = false;
        animStartTime = Time.time;
        score = value;
        animator.SetTrigger("pop");
    }

    void Update()
    {
        if (!animationFinished)
        {
            if ((Time.time - animStartTime) < animationTime)
            {
                float value =  (Time.time - animStartTime) / animationTime;
                ScoreProgressImage.fillAmount = value * score * 0.01f;
                ScoreProgressNum.text = ((int)(value * score)).ToString();
            }
            else
            {
                ScoreProgressNum.text = ((int)score).ToString();
                ScoreProgressImage.fillAmount = score * 0.01f;
                animationFinished = true;
                animator.SetTrigger("finished");
                GameObject.FindObjectOfType<EndButtonRemover>().ShowResultInfoHolder();
                string trigger = "dance";
                if (score < 70)
                {
                    trigger = "sad";
                    sadMusic.SetActive(true);
                }
                else
                    ovation.SetActive(true);
                trigger += Random.Range(1, 3).ToString();
                GameObject.Find("MainPlayerAvatar").GetComponent<Animator>().SetTrigger(trigger);
                GameObject.Find("ResultInfoHolder").GetComponent<CanvasGroup>().alpha = 1f;

            }
        }
        //if (score != lastScore)
        //    StartAnimation(score);

        lastScore = score;
    }
}
