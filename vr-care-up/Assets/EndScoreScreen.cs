using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScoreScreen : MonoBehaviour
{
    bool finished = false;
    public EndScoreRadial endScoreRadial;
    public Animator scoreScreanAnimator;
    public Text scoreValueText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished && endScoreRadial.animationFinished)
        {
            finished = true;
            scoreScreanAnimator.SetTrigger("in");
        }
    }

    public int CalculatePercentage()
    {
        ActionManager actionManager = GameObject.FindObjectOfType<ActionManager>();
        float result = (float)ActionManager.Points / (float)actionManager.TotalPoints;

        return Mathf.FloorToInt(result * 100f);
    }

    void OnEnable()
    {
        finished = false;
        scoreScreanAnimator.SetTrigger("out");
        float percent = CalculatePercentage();
        scoreValueText.text = percent.ToString() + "%";
        endScoreRadial.StartAnimation(CalculatePercentage());
    }
}
