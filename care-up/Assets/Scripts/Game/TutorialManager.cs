using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
/// <summary>
/// Handles entire tutorial scene step by step.
/// </summary>
public class TutorialManager : MonoBehaviour {

    public bool sequenceCompleted = false;
    public GameObject hintsPrefab;

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
    private Text UItext;
    
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
    private GameObject particleHint_alt;

	void Start () {
        particleHint = GameObject.Find("ParticleHint");
        particleHint.SetActive(false);
        particleHint_alt = GameObject.Find("ParticleHint (1)");
        particleHint_alt.SetActive(false);

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

        Transform ui = GameObject.Find("UI").transform;
        Instantiate(hintsPrefab, ui);

        UItext = GameObject.Find("hints").GetComponent<Text>();
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
                        UItext.text = "Beweeg de muis om rond te kijken door de kamer.";
                    }
                    else
                    {
                        controls.keyPreferences.SetAllLocked(true);
                        player.tutorial_movementLock = true;
                        UItext.text = "Welkom bij Care-Up";
                        SetPauseTimer(3.0f);
                    }
                    break;
                case TutorialStep.LookAround:
                    if ( player.tutorial_totalLookAround > 30.0f )
                    {
                        actionManager.Points += 1;
                        currentStep = TutorialStep.WalkAround;
                        player.tutorial_movementLock = false;
                        UItext.text = "Gebruik de W, A, S, D toesten om te lopen ";
                    }
                    break;
                case TutorialStep.WalkAround:
                    if ( player.tutorial_totalMoveAround > 50.0f )
                    {
                        actionManager.Points += 1;
                        currentStep = TutorialStep.WalkToTable;
                        particleHint.transform.position = tableTrigger.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Beweeg naar de tafel door W, A, S, D en de muis te gebruiken";
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
                        UItext.text = "Oplichtende voorwerpen met een hand icoon kunnen worden gebruikt door ernaar te kijken en te drukken op de linkermuisknop. Laten we onze handen wassen met de hygienepomp";
                    }
                    break;
                case TutorialStep.UseHandCleaner:
                    if ( handCleaner.tutorial_used )
                    {
                        currentStep = TutorialStep.ExamineRecords;
                        patientRecords.gameObject.SetActive(true);
                        particleHint.transform.position = patientRecords.transform.position;
                        UItext.text = "Goed! Sommige objecten moeten worden gecontroleerd zoals de clientgegevens. Wanneer je een vergrootglas ziet boven een voorwerp, betekend dit dat je hem kunt bekijken door te klikken op de linkermuisknop. Bekijk de clientgegevens nu.";
                    }
                    break;
                case TutorialStep.ExamineRecords:
                    if ( patientRecords.tutorial_picked )
                    {
                        currentStep = TutorialStep.CloseRecords;
                        particleHint.SetActive(false);
                        UItext.text = "We zijn nu in het voorwerpen overzicht. Na het controleren kun je met de 'Q' knop het object terugleggen";
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
                        UItext.text = "Sommige opgelichtende voorwerpen kun je oppakken. Dit doe je door naar het voorwerp te kijken en op linkermuisknop te drukken.";
                    }
                    break;
                case TutorialStep.PickItem:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickAnotherItem;
                        UItext.text = "Je kunt nog een voorwerp oppakken door ernaar te kijken en op de linkermuisknop te drukken";
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedLeft)
                        {
                            actionManager.Points += 1;
                            UItext.text = "Wanneer je handen leeg zijn en je pakt iets op. Zal het altijd in je linkerhand verschijnen.";
                            SetPauseTimer(5.0f);
                        }
                    }
                    break;
                case TutorialStep.PickAnotherItem:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.DroppingExplanation;
                        UItext.text = "Voorwerpen die je niet meer nodig hebt of niet meer gebruikt kun je weg zetten. Dit doe je de 'SHIFT' + 'Q' of 'SHIFT' + 'E' in te drukken. ";
                        SetPauseTimer(5.0f);
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedRight)
                        {
                            particleHint.SetActive(false);
                            actionManager.Points += 1;
                            UItext.text = "Wanneer je in je linkerhand al een voorwerp vast hebt zal het tweede voorwerp altijd in de rechterhand komen";
                            SetPauseTimer(5.0f);
                        }
                    }
                    break;
                case TutorialStep.DroppingExplanation:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.DropItem;
                        controls.keyPreferences.LeftDropKey.locked = false;
                        UItext.text = "Laten we het voorwerp in onze linkerhand terugzetten door op 'SHIFT'+ 'Q' te drukken.";
                    }
                    break;
                case TutorialStep.DropItem:
                    if ( handsInventory.tutorial_droppedLeft )
                    {
                        currentStep = TutorialStep.DropAnotherItem;
                        controls.keyPreferences.RightDropKey.locked = false;
                        UItext.text = "Laten we hetzelfde doen met onze rechterhand door op 'SHIFT' + 'E' te drukken.";
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
                        particleHint.SetActive(true);
                        UItext.text = "Laten we beide voorwerpen weer oppakken";
                    }
                    break;
                case TutorialStep.PickBothItems:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight )
                    {
                        currentStep = TutorialStep.WalkAway;
                        player.tutorial_movementLock = false;
                        particleHint.SetActive(false);
                        UItext.text = "Laten we nu een stuk van de tafel af lopen door middel van de W, A, S, D toetsen.";
                    }
                    break;
                case TutorialStep.WalkAway:
                    if ( Vector3.Distance(player.transform.position, tableTrigger.position) > 5.0f )
                    {
                        currentStep = TutorialStep.DropBothItems;
                        UItext.text = "Heel goed, laten we nu weer beide voorwerpen laten vallen door 'SHIFT' + 'Q' en 'SHIFT' + 'E'.";
                    }
                    break;
                case TutorialStep.DropBothItems:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickBothItemsAgain;
                        UItext.text = "Laten we de voorwerpen weer oppakken";
                        particleHint.transform.position = syringe.transform.position;
                        particleHint_alt.transform.position = needle.transform.position;
                        particleHint.SetActive(true);
                        particleHint_alt.SetActive(true);
                    }
                    else
                    {
                        if ( handsInventory.tutorial_droppedLeft && handsInventory.tutorial_droppedRight)
                        {
                            handsInventory.tutorial_pickedLeft =
                                handsInventory.tutorial_pickedRight = false;
                            UItext.text = "Voorwerpen kun je terugzetten/ laten vallen waar je wilt. Echter krijg je minpunten door voorwerpen naast de tafel te laten vallen";
                            SetPauseTimer(5.0f);
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
                        particleHint.SetActive(false);
                        particleHint_alt.SetActive(false);
                        UItext.text = "Sommige voorwerpen kun je combineren. Dit kun je doen door op de 'R' toets te drukken als je twee voorwerpen in je handen hebt die te combineren zijn. laten we de naald combineren met onze spuit zodat we straks het medicijn kunnen opzuigen";
                    }
                    break;
                case TutorialStep.CombineItems:
                    if ( handsInventory.tutorial_combined )
                    {
                        currentStep = TutorialStep.UseHint;
                        controls.keyPreferences.GetHintKey.locked = false;
                        medicine.gameObject.SetActive(true);
                        UItext.text = "Mocht je in het spel niet weten wat de volgende stap is dan kun je drukken op de spatiebalk voor een hint. Dit kost je echter wel punten.";
                    }
                    break;
                case TutorialStep.UseHint:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickMedicine;
                        controls.keyPreferences.pickObjectView.locked = false;
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        UItext.text = "Nu de spuit klaar is voor gebruik. Laten we het medicijn oppakken.";
                    }
                    else {
                        if (actionManager.tutorial_hintUsed)
                        {
                            UItext.text = "Je zult hardop denken over de volgende stap. Daarnaast worden voorwerpen, die nodig zijn voor de volgende stap, opgelicht.";
                            SetPauseTimer(5.0f);
                        }
                    }
                    break;
                case TutorialStep.PickMedicine:
                    if ( handsInventory.tutorial_pickedLeft || handsInventory.tutorial_pickedRight)
                    {
                        currentStep = TutorialStep.CombineItemsAgain;
                        handsInventory.tutorial_combined = false;
                        UItext.text = "Super! Om het medicijn op te zuigen moeten we de spuit combineren met het medicijn. Probeer dit nu.";
                    }
                    break;
                case TutorialStep.CombineItemsAgain:
                    if ( handsInventory.tutorial_combined )
                    {
                        currentStep = TutorialStep.MoveToPatient;
                        particleHint.transform.position = patientTrigger.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Sommige voorwerpen kunnen gebruikt worden in de omgeving zoals bijvoorbeeld de client. Laten we dit nu proberen. Beweeg dicht genoeg bij de client.";
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
                        UItext.text = "We kunnen nu de spuit gebruiken op de client. Druk op 'Q'om een voorwerp in je linkerhand te gebruiken en op 'E' om een voorwerp in je rechterhand te gebruiken. Druk op de knop die hoort bij de hand die de spuit vast heeft.";
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
                        UItext.text = "Bij acties met meerdere handelingen verschijnt een keuze cirkel. Hieruit dien je de juiste keuze te kiezen om verder te gaan met de actie.";
                    }
                    else
                    {
                        UItext.text = "Bij acties met meerdere handelingen verschijnt een keuze cirkel. Hieruit dien je de juiste keuze te kiezen om verder te gaan met de actie. In deze training is het goede antwoord groen gemarkeerd.";
                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if ( sequenceCompleted )
                    {
                        currentStep = TutorialStep.TutorialEnd;
                        player.tutorial_movementLock = false;
                        controls.keyPreferences.SetAllLocked(true);
                        UItext.text = "Goed gedaan. Dit was de uitleg over Care-Up. Succes bij je eerste BIG-handeling";
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

   /* void OnGUI()
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
    }*/
}
