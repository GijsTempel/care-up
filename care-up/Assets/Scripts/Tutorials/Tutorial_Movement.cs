using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Movement : TutorialManager
{
    public enum TutorialStep
    {
        First,
        Welcome,
        PointsExpl,
        MoveToTable,
        MoveBack,
        MoveToDoctor,
        FreeLookExpl,
        MoveWithFreeLook,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    private GameObject wfPos;
    private GameObject docPos;

    protected override void Start()
    {
        base.Start();

        wfPos = GameObject.Find("WorkFieldPos");
        docPos = GameObject.Find("DoctorPos");

        wfPos.SetActive(false);
        docPos.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

		if (!Paused())
        {
            switch (currentStep)
            {
                case TutorialStep.First:
                    currentStep = TutorialStep.Welcome;
                    hintsBox.anchoredPosition = new Vector2(-0.00011874f, 0.00024414f);
                    hintsBox.sizeDelta = new Vector2(788f, 524.9f);
                    UItext.text = "Welkom, in deze oefening ";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PointsExpl;
                        UItext.text = "messege about points of interest with ok button";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PointsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveToTable;
                        UItext.text = "Move to table";

                        player.tutorial_movedTo = false;
                        wfPos.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveToTable:
                    if (player.tutorial_movedTo)
                    {
                        wfPos.SetActive(false);
                        player.tutorial_movedTo = false;

                        currentStep = TutorialStep.MoveBack;
                        UItext.text = "move back to overview with button";

                        player.tutorial_movedBack = false;
                    }
                    break;
                case TutorialStep.MoveBack:
                    if (player.tutorial_movedBack)
                    {
                        player.tutorial_movedBack = false;

                        currentStep = TutorialStep.MoveToDoctor;
                        UItext.text = "Move to doctor";

                        player.tutorial_movedTo = false;
                        docPos.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveToDoctor:
                    if (player.tutorial_movedTo)
                    {
                        docPos.SetActive(false);
                        player.tutorial_movedTo = false;

                        currentStep = TutorialStep.FreeLookExpl;
                        UItext.text = "Explanation about free look with oke button";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.FreeLookExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveWithFreeLook;
                        UItext.text = "move to table using free look (turn off back button)";

                        player.MoveBackButtonObject.SetActive(false);
                        player.tutorial_movedTo = false;
                        wfPos.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveWithFreeLook:
                    if (player.tutorial_movedTo)
                    {
                        wfPos.SetActive(false);

                        currentStep = TutorialStep.Done;
                        UItext.text = "Gefeliciteerd!";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    TutorialEnd();
                    break;
            }
        }
	}
}
