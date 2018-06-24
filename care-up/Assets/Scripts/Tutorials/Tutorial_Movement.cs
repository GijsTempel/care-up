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
                    UItext.text = "Welkom! In deze leermodule zal je leren hoe je door de omgeving van Care Up kunt bewegen.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        hintsBox.anchoredPosition = new Vector2(0f, 0f);
                        hintsBox.sizeDelta = new Vector2(788f, 524.9f);
                        currentStep = TutorialStep.PointsExpl;
                        UItext.text = "Binnen Care Up kun je bewegen door de omgeving door te klikken op interessante objecten. Interessante objecten kun je ontdekken door met de muis over objecten heen te bewegen. Op tablet of telefoon staan de namen van de interessante objecten boven de objecten weergegeven. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PointsExpl:
                    if (nextButtonClicked)
                    {
                        hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        currentStep = TutorialStep.MoveToTable;
                        UItext.text = "Probeer nu naar het werkveld te bewegen. Dit kun je doen door op het werkveld te klikken.";

                        player.tutorial_movedTo = false;
                        wfPos.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveToTable:
                    if (player.tutorial_movedTo)
                    {
                        wfPos.SetActive(false);
                        player.tutorial_movedTo = false;

                        hintsBox.anchoredPosition = new Vector2(0f, 0f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        currentStep = TutorialStep.MoveBack;
                        UItext.text = "Je ziet dat we nu naar het werkveld zijn verplaatst. Je kunt op ieder moment weer terug keren naar de beginpositie. Dit noemen wij het overzicht. Keer terug naar het overzicht door te drukken op de knop 'Terug naar overzicht' rechtsboven in het scherm. ";

                        player.tutorial_movedBack = false;
                    }
                    break;
                case TutorialStep.MoveBack:
                    if (player.tutorial_movedBack)
                    {
                        player.tutorial_movedBack = false;
                        hintsBox.anchoredPosition = new Vector2(0f, -196f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        currentStep = TutorialStep.MoveToDoctor;
                        UItext.text = "Heel goed. Probeer nu richting je collega te bewegen door op haar te klikken.";

                        player.tutorial_movedTo = false;
                        docPos.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveToDoctor:
                    if (player.tutorial_movedTo)
                    {
                        docPos.SetActive(false);
                        player.tutorial_movedTo = false;
                        hintsBox.anchoredPosition = new Vector2(-0.00011874f, 0.00024414f);
                        hintsBox.sizeDelta = new Vector2(788f, 524.9f);
                        currentStep = TutorialStep.FreeLookExpl;
                        UItext.text = "Binnen Care Up kun je ook om je heen kijken. Om je heen kijken kun je doen door (Computer) De linkermuisknop in te drukken en ingedrukt te houden. Beweeg vervolgens de muis om rond te kijken. Laat de linkermuisknop los om te stoppen met rondkijken. (Mobiel/Tablet) Kijk op de Mobiel/Tablet rond door met je vinger over het beeld te 'swipen'. Klik hierbij niet op interessante objecten. (Werkveld, cliënt, college etc.) ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.FreeLookExpl:
                    if (nextButtonClicked)
                    {

                        hintsBox.anchoredPosition = new Vector2(-338f, -265.64f);
                        hintsBox.sizeDelta = new Vector2(-338f, 246f);
                        currentStep = TutorialStep.MoveWithFreeLook;
                        UItext.text = "Beweeg vanaf je collega direct naar het werkveld zonder terug te keren naar het overzicht.";

                        player.MoveBackButtonObject.SetActive(false);
                        player.tutorial_movedTo = false;
                        wfPos.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveWithFreeLook:
                    if (player.tutorial_movedTo)
                    {
                        wfPos.SetActive(false);
                        hintsBox.anchoredPosition = new Vector2(513f, -291f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        currentStep = TutorialStep.Done;
                        UItext.text = "Gefeliciteerd! Je weet nu hoe je kunt rond bewegen binnen Care Up.";
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
