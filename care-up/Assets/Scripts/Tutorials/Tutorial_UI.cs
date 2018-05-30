using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_UI : TutorialManager
{
    public enum TutorialStep
    {
        First,
        Welcome,
        RobotIntro,
        OpenRobotUI,
        TimeExpl,
        PointsExpl,
        PercentExpl,
        TabsExpl,
        PressGeneral,
        GeneralExpl,
        GeneralBack,
        PressChecklist,
        ChecklistExpl,
        ChecklistBack,
        PressMessageCenter,
        MessageCenterExpl,
        OpenMessage,
        CloseMessageCenter,
        CloseRobotUI,
        DevHintsExpl,
        OpenExtraHints,
        CloseHints,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    Cheat_CurrentAction hintsUI;

    protected override void Start()
    {
        base.Start();

        hintsUI = GameObject.FindObjectOfType<Cheat_CurrentAction>();
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
                    UItext.text = "Welcome, this is UI tutorial!";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.RobotIntro;
                        UItext.text = "This is Olivia. She exists.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.RobotIntro:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenRobotUI;
                        UItext.text = "We have robotUI! It does a lot, open it.";
                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        currentStep = TutorialStep.TimeExpl;
                        UItext.text = "Here you can see how slow are you.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TimeExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PointsExpl;
                        UItext.text = "Here you can see how much points have u earned";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PointsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PercentExpl;
                        UItext.text = "Here you can see in percents how much of this level have u completed.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PercentExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.TabsExpl;
                        UItext.text = "And here are different tabs that open a lot of info";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TabsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PressGeneral;
                        UItext.text = "Let's press General Tab to open it!";
                        RobotUITabs.tutorial_generalOpened = false;
                    }
                    break;
                case TutorialStep.PressGeneral:
                    if (RobotUITabs.tutorial_generalOpened)
                    {
                        currentStep = TutorialStep.GeneralExpl;
                        UItext.text = "General tab is for.. ? something";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.GeneralExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.GeneralBack;
                        UItext.text = "Let's close the tab by clicking back button";
                    }
                    break;
                case TutorialStep.GeneralBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressChecklist;
                        UItext.text = "Let's now open checklist tab";
                        RobotUITabs.tutorial_checkListOpened = false;
                    }
                    break;
                case TutorialStep.PressChecklist:
                    if (RobotUITabs.tutorial_checkListOpened)
                    {
                        currentStep = TutorialStep.ChecklistExpl;
                        UItext.text = "Checklist shows a list of all steps you need to perform in a scene! there might be none in tutorial tho";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.ChecklistExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.ChecklistBack;
                        UItext.text = "Let's close this too";
                    }
                    break;
                case TutorialStep.ChecklistBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        currentStep = TutorialStep.PressMessageCenter;
                        UItext.text = "Oh wow, we got new message! Let's open and read it!";
                        GameObject.FindObjectOfType<RobotUIMessageTab>().NewMessage("New message!",
                            "Here is your first ingame e-mail message. You will see here a lot of useful information about your actions during the game!",
                             RobotUIMessageTab.Icon.Info);
                        RobotUITabs.tutorial_messageCenterOpened = false;
                    }
                    break;
                case TutorialStep.PressMessageCenter:
                    if (RobotUITabs.tutorial_messageCenterOpened)
                    {
                        currentStep = TutorialStep.MessageCenterExpl;
                        UItext.text = "This is your e-mail app. We'll send you some messages during the game to help you out!";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.MessageCenterExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenMessage;
                        UItext.text = "Let's look what message have u got. Click on it.";
                        RobotUIMessageTab.tutorial_messageOpened = false;
                    }
                    break;
                case TutorialStep.OpenMessage:
                    if (RobotUIMessageTab.tutorial_messageOpened)
                    {
                        currentStep = TutorialStep.CloseMessageCenter;
                        UItext.text = "What a nice message! Now let's close message center.";
                        RobotUITabs.tutorial_back = false;
                    }
                    break;
                case TutorialStep.CloseMessageCenter:
                    if (RobotUITabs.tutorial_back)
                    {
                        currentStep = TutorialStep.CloseRobotUI;
                        UItext.text = "Let's now close robotUI completely, we done with it";
                    }
                    break;
                case TutorialStep.CloseRobotUI:
                    if (player.tutorial_robotUI_closed)
                    {
                        currentStep = TutorialStep.DevHintsExpl;
                        UItext.text = "These are hints for current step, very useful!";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.DevHintsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenExtraHints;
                        UItext.text = "And there's even more info on current step! Open it!";
                        hintsUI.tutorial_extraOpened = false;
                    }
                    break;
                case TutorialStep.OpenExtraHints:
                    if (hintsUI.tutorial_extraOpened)
                    {
                        currentStep = TutorialStep.CloseHints;
                        UItext.text = "Wow, that was cool, now close it.";
                        hintsUI.tutorial_extraClosed = false;
                    }
                    break;
                case TutorialStep.CloseHints:
                    if (hintsUI.tutorial_extraClosed)
                    {
                        currentStep = TutorialStep.Done;
                        UItext.text = "That completes UI tutorial, glhf";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    endPanel.SetActive(true);
                    player.enabled = false;
                    GameObject.FindObjectOfType<RobotManager>().enabled = false;
                    foreach (InteractableObject o in GameObject.FindObjectsOfType<InteractableObject>())
                    {
                        o.Reset();
                        o.enabled = false;
                    }
                    break;
            }
        }
    }
}
