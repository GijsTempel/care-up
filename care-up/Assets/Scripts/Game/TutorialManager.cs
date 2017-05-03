using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// Handles entire tutorial scene step by step.
/// </summary>
public class TutorialManager : MonoBehaviour {

    public bool sequenceCompleted = false;

    public enum TutorialStep
    {
        StartTutorial,
        LookAround,
        WalkAround,
        WalkToTable,
        UseHandCleaner,
        ExamineRecords,
        CloseRecords,
        PickItem,
        PickAnotherItem,
        DroppingExplanation,
        DropItem,
        DropAnotherItem,
        PickBothItems,
        WalkAway,
        DropBothItems,
        PickBothItemsAgain,
        CombineItems,
        UseHint,
        PickMedicine,
        CombineItemsAgain,
        MoveToPatient,
        UseOnHand,
        SequenceExplanation,
        CompleteSequence,
        TutorialEnd,
        None
    }

    private TutorialStep currentStep = TutorialStep.StartTutorial;
    private bool pauseEnabled = false;
    private float pauseTimer = 0.0f;
    private string UItext = "";
    
    private RigidbodyFirstPersonController player;
    private ActionManager actionManager;
    private HandsInventory handsInventory;
    private Controls controls;
    private UsableObject handCleaner;
    private ExaminableObject patientRecords;
    private GameObject syringe;
    private GameObject needle;
    private ExaminableObject medicine;
    private Transform tableTrigger;
    private Transform patientTrigger;

    private GameObject particleHint;

	void Start () {
        particleHint = GameObject.Find("ParticleHint");
        particleHint.SetActive(false);

        GameObject gameLogic = GameObject.Find("GameLogic");
        actionManager = gameLogic.GetComponent<ActionManager>();
        handsInventory = gameLogic.GetComponent<HandsInventory>();
        controls = gameLogic.GetComponent<Controls>();

        player = GameObject.Find("Player").GetComponent<RigidbodyFirstPersonController>();
        handCleaner = GameObject.Find("HandCleaner").GetComponent<UsableObject>();
        patientRecords = GameObject.Find("PatientRecords").GetComponent<ExaminableObject>();
        medicine = GameObject.Find("Medicine").GetComponent<ExaminableObject>();
        syringe = GameObject.Find("Syringe");
        needle = GameObject.Find("AbsorptionNeedle");

        medicine.gameObject.SetActive(false);
        syringe.SetActive(false);
        needle.SetActive(false);

        tableTrigger = GameObject.Find("__tableTrigger").transform;
        patientTrigger = GameObject.Find("__patientTrigger").transform;

        handsInventory.dropPenalty = false;
	}
	
	void Update () {
		if ( pauseEnabled && pauseTimer > 0.0f)
        {
            pauseTimer -= Time.deltaTime;
        }
        else
        {
            switch (currentStep)
            {
                case TutorialStep.StartTutorial:
                    if ( TimerElapsed() )
                    {
                        currentStep = TutorialStep.LookAround;
                        UItext = "Gebruik de muis om rond te kijken door de kamer.";
                    }
                    else
                    {
                        controls.keyPreferences.SetAllLocked(true);
                        player.tutorial_movementLock = true;
                        UItext = "Welkom bij Care-Up";
                        SetPauseTimer(3.0f);
                    }
                    break;
                case TutorialStep.LookAround:
                    if ( player.tutorial_totalLookAround > 30.0f )
                    {
                        actionManager.Points += 1;
                        currentStep = TutorialStep.WalkAround;
                        player.tutorial_movementLock = false;
                        UItext = "Gebruik de W, A, S, D toesten om te lopen ";
                    }
                    break;
                case TutorialStep.WalkAround:
                    if ( player.tutorial_totalMoveAround > 50.0f )
                    {
                        actionManager.Points += 1;
                        currentStep = TutorialStep.WalkToTable;
                        particleHint.transform.position = tableTrigger.transform.position;
                        particleHint.SetActive(true);
                        UItext = "Beweeg naar de tafel door W, A, S, D en de muis te gebruiken";
                    }
                    break;
                case TutorialStep.WalkToTable:
                    if ( Vector3.Distance(tableTrigger.position, player.transform.position) < 1.0f)
                    {
                        actionManager.Points += 1;
                        tableTrigger.gameObject.SetActive(false);
                        player.tutorial_movementLock = true;
                        currentStep = TutorialStep.UseHandCleaner;
                        controls.keyPreferences.mouseClickLocked = false;
                        controls.keyPreferences.mouseClickKey.locked = false;
                        controls.keyPreferences.closeObjectView.locked = false;
                        particleHint.transform.position = handCleaner.transform.position;
                        UItext = "Oplichtende voorwerpen met een hand icoon kunnen worden gebruikt door ernaar te kijken en te drukken op de linker muisknop. Laten we onze handen wasen met de hygiene pomp";
                    }
                    break;
                case TutorialStep.UseHandCleaner:
                    if ( handCleaner.tutorial_used )
                    {
                        currentStep = TutorialStep.ExamineRecords;
                        patientRecords.gameObject.SetActive(true);
                        particleHint.transform.position = patientRecords.transform.position;
                        UItext = "Goed! Sommige objecten moeten worden gecontroleerd zoals de clientgegevens. Wanneer je een vergrootglas ziet boven een voorwerp, betekend dit dat je hem kunt bekijken door te killen op de linker muisknop. Bekijk de clientgegevens nu.";
                    }
                    break;
                case TutorialStep.ExamineRecords:
                    if ( patientRecords.tutorial_picked )
                    {
                        currentStep = TutorialStep.CloseRecords;
                        particleHint.SetActive(false);
                        UItext = "We zijn nu in het voorwerpen overzicht. Na het controleren kun je met de 'Q' knop het object terugleggen";
                    }
                    break;
                case TutorialStep.CloseRecords:
                    if ( patientRecords.tutorial_closed )
                    {
                        actionManager.Points += 1;
                        currentStep = TutorialStep.PickItem;
                        syringe.SetActive(true);
                        needle.SetActive(true);
                        particleHint.transform.position = Vector3.Lerp(syringe.transform.position, needle.transform.position, 0.5f);
                        particleHint.SetActive(true);
                        UItext = "Sommige opgelichtende voorwerpen kun je oppakken. Dit doe je door naar het voorwerp te kijken en op linker muisknop te drukken.";
                    }
                    break;
                case TutorialStep.PickItem:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickAnotherItem;
                        UItext = "Je kunt nog een voorwerp oppakken door ernaar te kijken en op de linker muisknop te drukken";
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedLeft)
                        {
                            actionManager.Points += 1;
                            UItext = "Wanneer je handen leeg zijn en je pakt iets op. Zal het altijd in je linker hand verschijnen.";
                            SetPauseTimer(3.0f);
                        }
                    }
                    break;
                case TutorialStep.PickAnotherItem:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.DroppingExplanation;
                        UItext = "Voorwerpen die je niet meer nodig hebt of niet meer gebruikt kun je weg zetten. Dit doe je de 'SHIFT' + 'Q' of 'SHIFT' + 'E' in te drukken. ";
                        SetPauseTimer(3.0f);
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedRight)
                        {
                            particleHint.SetActive(false);
                            actionManager.Points += 1;
                            UItext = "Wanner je in je linkerhand al een voorwerp vast hebt zal het tweede voorwerp altijd in de rechterhand komen";
                            SetPauseTimer(3.0f);
                        }
                    }
                    break;
                case TutorialStep.DroppingExplanation:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.DropItem;
                        controls.keyPreferences.LeftDropKey.locked = false;
                        UItext = "Laten we het voorwerp in onze linkerhand terugzetten door op 'SHIFT'+ 'Q' te drukken.";
                    }
                    break;
                case TutorialStep.DropItem:
                    if ( handsInventory.tutorial_droppedLeft )
                    {
                        currentStep = TutorialStep.DropAnotherItem;
                        controls.keyPreferences.RightDropKey.locked = false;
                        UItext = "Laten we hetzelfde doen met onze rechter hand door op 'SHIFT' = 'E' te drukken.";
                    }
                    break;
                case TutorialStep.DropAnotherItem:
                    if ( handsInventory.tutorial_droppedRight )
                    {
                        actionManager.Points += 1;
                        currentStep = TutorialStep.PickBothItems;
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight =
                            handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        UItext = "Laten we beide voorwerpen weer oppakken";
                    }
                    break;
                case TutorialStep.PickBothItems:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight )
                    {
                        currentStep = TutorialStep.WalkAway;
                        player.tutorial_movementLock = false;
                        UItext = "Laten we nu een stuk van de tafel af lopen door middel van de W,A,S,D toetsen.";
                    }
                    break;
                case TutorialStep.WalkAway:
                    if ( Vector3.Distance(player.transform.position, tableTrigger.position) > 5.0f )
                    {
                        currentStep = TutorialStep.DropBothItems;
                        UItext = "Heel goed, laten we nu weer beide voorwerpen laten vallen door 'SHIFT' + 'Q' en 'SHIFT' + 'E'.";
                    }
                    break;
                case TutorialStep.DropBothItems:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickBothItemsAgain;
                        UItext = "Laten we de voorwerpen weer oppakken";
                    }
                    else
                    {
                        if ( handsInventory.tutorial_droppedLeft && handsInventory.tutorial_droppedRight)
                        {
                            handsInventory.tutorial_pickedLeft =
                                handsInventory.tutorial_pickedRight = false;
                            UItext = "Voorwerpen kun je terugzetten/ laten vallen waar je wilt. Echter krijg je minpunten door voorwerpen naast de tafel te laten vallen";
                            SetPauseTimer(3.0f);
                        }
                    }
                    break;
                case TutorialStep.PickBothItemsAgain:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight)
                    {
                        currentStep = TutorialStep.CombineItems;
                        controls.keyPreferences.CombineKey.locked = false;
                        controls.keyPreferences.LeftDropKey.locked = true;
                        controls.keyPreferences.RightDropKey.locked = true;
                        UItext = "Some items can be combined into one. Combining can be done bij pressing “R” if both items in your hand are combinable. Lets combine the needle with our syringe so we can absorbe medicine.";
                    }
                    break;
                case TutorialStep.CombineItems:
                    if ( handsInventory.tutorial_combined )
                    {
                        currentStep = TutorialStep.UseHint;
                        controls.keyPreferences.GetHintKey.locked = false;
                        medicine.gameObject.SetActive(true);
                        UItext = "If you do not know what step you need to preform next, you can press the sapce bar to get a hit. Try this now";
                    }
                    break;
                case TutorialStep.UseHint:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickMedicine;
                        controls.keyPreferences.pickObjectView.locked = false;
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        UItext = "Now that we have the syringe prepared lets pick up the medicine";
                    }
                    else {
                        if (actionManager.tutorial_hintUsed)
                        {
                            UItext = "You will think out loud about the next step. Also objects needed for the next step will be highlighted.";
                            SetPauseTimer(3.0f);
                        }
                    }
                    break;
                case TutorialStep.PickMedicine:
                    if ( handsInventory.tutorial_pickedLeft || handsInventory.tutorial_pickedRight)
                    {
                        currentStep = TutorialStep.CombineItemsAgain;
                        handsInventory.tutorial_combined = false;
                        UItext = "Great, to absorbe the medicine lets combine the medicine with our syringe by pressing “R”";
                    }
                    break;
                case TutorialStep.CombineItemsAgain:
                    if ( handsInventory.tutorial_combined )
                    {
                        currentStep = TutorialStep.MoveToPatient;
                        particleHint.transform.position = patientTrigger.transform.position;
                        particleHint.SetActive(true);
                        UItext = "Items can be used on other object like a patient. Let’s try this now. Move close enough to the patient.";
                    }
                    break;
                case TutorialStep.MoveToPatient:
                    if ( Vector3.Distance(player.transform.position, patientTrigger.position) < 1.0f)
                    {
                        patientTrigger.gameObject.SetActive(false);
                        player.tutorial_movementLock = true;
                        currentStep = TutorialStep.UseOnHand;
                        controls.keyPreferences.LeftUseKey.locked = false;
                        controls.keyPreferences.RightUseKey.locked = false;
                        particleHint.transform.position = GameObject.Find("Patient").transform.position;
                        UItext = "now you can see an use icon on the patient. Depending on which hand hold your object. You can press “E” or “Q” to use your object. Try this now.";
                    }
                    break;
                case TutorialStep.UseOnHand:
                    if ( handsInventory.tutorial_itemUsedOn )
                    {
                        particleHint.SetActive(false);
                        currentStep = TutorialStep.SequenceExplanation;

                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayTutorialAnimationSequence("Injection", target);
                    }
                    break;
                case TutorialStep.SequenceExplanation:
                    if ( TimerElapsed() )
                    {
                        currentStep = TutorialStep.CompleteSequence;
                        sequenceCompleted = false;
                        PlayerAnimationManager.SequenceTutorialLock(false);
                        UItext = "";
                    }
                    else
                    {
                        UItext = "Sometimes an animation sequence will start to show moe difficult actions. A choice cirkel appears. You have to chose te right awnser to continue the animation. For training purposes the right anwsers are shown in green.";
                        SetPauseTimer(3.0f);
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if ( sequenceCompleted )
                    {
                        currentStep = TutorialStep.TutorialEnd;
                        player.tutorial_movementLock = false;
                        controls.keyPreferences.SetAllLocked(true);
                        UItext = "Great this concludes our tutorial. Good luck!";
                    }
                    break;
                case TutorialStep.TutorialEnd:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.None;
                        GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().TutorialCompleted = true;
                        actionManager.OnUseAction("__tutorialEnd");
                        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("Injection");
                    }
                    else
                    {
                        SetPauseTimer(3.0f);
                    }
                    break;
                default:
                    break;
            }
        }
	}

    void SetPauseTimer(float value)
    {
        if (!pauseEnabled)
        {
            pauseEnabled = true;
            pauseTimer = value;
        }
    }

    bool TimerElapsed()
    {
        if (pauseEnabled && pauseTimer <= 0.0f)
        {
            pauseEnabled = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnGUI()
    {
        GUIStyle style = GUI.skin.GetStyle("Label");
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 30;
        style.normal.textColor = Color.white;

        string displayText = UItext;
        if (pauseEnabled)
        {
            displayText += "\nPause time: " + pauseTimer;
        }

        GUI.Label(new Rect(0, 0, Screen.width, Screen.height),
            displayText, style);
    }
}
