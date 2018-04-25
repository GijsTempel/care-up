using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
/// <summary>
/// Handles entire tutorial scene step by step.
/// </summary>
public class TutorialManager : MonoBehaviour {

    public GameObject hintsPrefab;

    [HideInInspector]
    public bool sequenceCompleted = false;

    [HideInInspector]
    public bool movementLock = false;
    [HideInInspector]
    public bool mouseClickLocked = false;
    [HideInInspector]
    public bool closeObjectViewLocked = false;
    [HideInInspector]
    public bool leftDropKeyLocked = false;
    [HideInInspector]
    public bool rightDropKeyLocked = false;
    [HideInInspector]
    public bool combineKeyLocked = false;
    [HideInInspector]
    public bool getHintKeyLocked = false;
    [HideInInspector]
    public bool pickObjectViewKeyLocked = false;
    [HideInInspector]
    public bool leftUseKeyLocked = false;
    [HideInInspector]
    public bool rightUseKeyLocked = false;

    public enum TutorialStep
    {
        StartTutorial,             // 0
        //LookAround,                // step 1
        //WalkAround,                // step 2 rip walking
        WalkToTable,               // step 3
        UseHandCleaner,            // step 4
        UseTable,                  // step 5
        ExamineRecords,            // step 5
        CloseRecords,              // step 5
        DropRecords,
        ExaminePrescription,
        ClosePrescription,
        DropPrescription,
        ExamineMedicine,           // step 5
        CloseMedicine,             // inserted
        DropMedicine1,
        Overview1,
        ExplainTalking,
        MoveToDoctor,
        UseDoctor,                 // inserted
        Overview2,
        TalkDoubleCheck,           // inserted
        PickAlcohol,               // step 6
        PickCloth,                 // step 7
        CombineAlcoholCloth,       // step 8
        DroppingExplanation,       // step 9
        DropItem,                  // step 9
        DropAnotherItem,           // step 9

        //PickBothItems,             // step 9
        //WalkAway,                  // step 10
        //DropBothItems,             // step 10
        //PickBothItemsAgain,        // step 10
        //DropBothItemAway,          // inserted
        CombineDesinfMedicine,     // step 11

        DropClothMedicine,         // step 12
        PickSyringeAbNeedleCap,    // step 13
        CombineSyringeAbNeedleCap, // step 14
        CombineTakeOffAbCap,       // step 15
        //DropAbsorptionCap,         // step 16
        CombineSyringeAbNeedleMed, // step 17
        DropMedicine,              // step 18
        UseSyringeWithMed,         // step 19

        DecombineAbsSyringe,
        UseNeedleOnTrashAbs,

        /*
        PickUpAbsorptionCap,       // step 20
        CombinePutOnAbCap,         // step 21
        CombineTakeOffAbNeedle,    // step 22
        UseAbNeedleOnTrash,        // step 23
        */

        PickInjectionNeedle,       // step 24
        CombineSyringeInjNeedle,   // step 25
        //CombineTakeOffInjCap,    // step 26 : should not take cap off
        //DropInjectionCap,        // step 27 : it's done in sequence
        
        Overview3,
        MoveToPatient,             // step 
        TalkToPatient,             // step 28 

        UseOnHand,                 // step 29
        SequenceExplanation,       // step 30
        CompleteSequence,          // step 30

        Overview4,
        DecombineInjSyringe,
        UseNeedleOnTrashInj,

        /*
        CombinePutOnInjCap,        // step 31
        CombineTakeOffInjNeedle,   // step 32
        UseInjNeedleOnTrash,       // step 33
        */

        DropSyringe,               // step 34
       
        UseWorkField,              // step 35
        UseHygienePump,            // step 36
        UsePaperAndPen,            // step 37

        TutorialEnd,
        None
    }

    private TutorialStep currentStep = TutorialStep.StartTutorial;
    private bool pauseEnabled = false;
    private float pauseTimer = 0.0f;
    private Text UItext;
    
    private PlayerScript player;
    private ActionManager actionManager;
    private HandsInventory handsInventory;
    private Controls controls;
    private UsableObject handCleaner;
    private WorkField workField;
    private ExaminableObject patientRecords;
    private ExaminableObject prescriptionForm;
    private GameObject syringe;
    //private GameObject needle;
    private ExaminableObject medicine;
    private PickableObject alcohol;
    private PickableObject cloth;
    private UsableObject paperNPen;
    private InjectionPatient patient;
    private PersonObject doctor;
    private GameObject absorptionNeedle;
    private GameObject injectionNeedle;

    private Transform tableTrigger;
    private Transform patientTrigger;

    private GameObject particleHint;
    private GameObject particleHint_alt;

    [HideInInspector]
    public bool needleTrashed = false;

    [HideInInspector]
    public string itemToPick = "";
    [HideInInspector]
    public string itemToPick2 = "";
    [HideInInspector]
    public string itemToDrop = "";
    [HideInInspector]
    public string itemToDrop2 = "";

    private GameObject endPanel;

    public bool TutorialEnding
    {
        get { return endPanel.activeSelf; }
    }

    void Awake () {
        particleHint = GameObject.Find("ParticleHint");
        particleHint.SetActive(false);
        particleHint_alt = GameObject.Find("ParticleHint (1)");
        particleHint_alt.SetActive(false);

        GameObject gameLogic = GameObject.Find("GameLogic");
        actionManager = gameLogic.GetComponent<ActionManager>();
        handsInventory = gameLogic.GetComponent<HandsInventory>();
        controls = gameLogic.GetComponent<Controls>();

        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        handCleaner = GameObject.Find("HandCleaner").GetComponent<UsableObject>();
        workField = GameObject.Find("WorkField").GetComponent<WorkField>();
        patientRecords = GameObject.Find("PatientRecords").GetComponent<ExaminableObject>();
        prescriptionForm = GameObject.Find("PrescriptionForm").GetComponent<ExaminableObject>();

        Transform interactables = GameObject.Find("Interactable Objects").transform;
        medicine = interactables.Find("Medicine").GetComponent<ExaminableObject>();
        syringe = interactables.Find("Syringe").gameObject;
        //needle = interactables.Find("AbsorptionNeedle").gameObject;
        alcohol = interactables.Find("Alcohol").GetComponent<PickableObject>();
        cloth = interactables.Find("Cloth").GetComponent<PickableObject>();
        paperNPen = interactables.Find("PaperAndPen").GetComponent<UsableObject>();

        patient = GameObject.FindObjectOfType<InjectionPatient>();
        doctor = GameObject.Find("Doctor").GetComponent<PersonObject>();
        if (doctor == null)
        {
            doctor = GameObject.Find("Doctor").transform.parent.GetComponent<PersonObject>();
        }

        tableTrigger = GameObject.Find("__tableTrigger").transform;
        patientTrigger = GameObject.Find("__patientTrigger").transform;

        handsInventory.dropPenalty = false;

        Transform ui = GameObject.Find("UI").transform;
        Instantiate(hintsPrefab, ui);

        UItext = GameObject.Find("hints").GetComponent<Text>();

        endPanel = GameObject.Find("TutorialDonePanel");
	}

    private void Start()
    {
        endPanel.SetActive(false);
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
                        currentStep = TutorialStep.WalkToTable;
                        player.tutorial_movementLock = movementLock = false;
                        particleHint.transform.position = tableTrigger.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Ieder protocol start in een overzicht. In het overzicht zie je interessante objecten. Je kunt naar interessante objecten toe bewegen door te klikken op het interessante object of persoon te selecteren. Klik nu op het werkveld om hiernaar toe te bewegen.  ";
                    }
                    else
                    {
                        SetAllKeysLocked(true);
                        controls.keyPreferences.mouseClickLocked = mouseClickLocked = false;
                        controls.keyPreferences.mouseClickKey.locked = false;
                        player.tutorial_movementLock = movementLock = true;
                        UItext.text = "Welkom bij Care Up. Je hebt gekozen om het spel te leren. Heel goed. Laten we beginnen. ";
                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.WalkToTable:
                    if ( Vector3.Distance(tableTrigger.position, player.transform.position) < 2f)
                    {
                        AddPointWithSound();
                        tableTrigger.gameObject.SetActive(false);
                        player.tutorial_movementLock = movementLock = true;
                        currentStep = TutorialStep.UseHandCleaner;
                        controls.keyPreferences.closeObjectView.locked = closeObjectViewLocked = false;
                        particleHint.transform.position = handCleaner.transform.position;
                        UItext.text = "Laten we nu met objecten gaan interacteren. Om er achter te komen wat een object is, kun je er overheen bewegen. Je kunt op een object drukken om het object te gebruiken of op te pakken. Klik nu op de handhyiëne pomp om de pomp te gebruiken.";
                    }
                    break;
                case TutorialStep.UseHandCleaner:
                    if (handCleaner.tutorial_used)
                    {
                        currentStep = TutorialStep.UseTable;
                        particleHint.transform.position = workField.transform.position;
                        UItext.text = "Heel goed! Doe nu hetzelfde met het werkveld. Klik op het werkveld om het werkveld schoon te maken.";
                    }
                    break;
                case TutorialStep.UseTable:
                    if (workField.tutorial_used)
                    {
                        currentStep = TutorialStep.ExamineRecords;
                        particleHint.transform.position = patientRecords.transform.position;
					UItext.text = "Sommige objecten dien je te controleren. Denk hierbij aan de toedienlijst, de cliëntgevens en het medicijn. Laten we als eerste de cliëntgegevens controleren. Klik op de cliëntgegevens om deze op te pakken. Klik vervolgens nogmaals op de cliëntgegevens. Dit opent het optiemenu. Kies in het optiemenu voor de optie 'Controleren' om de 'Bekijk modus' te openen.";
                        patientRecords.tutorial_picked = false;
                        itemToPick = "PatientRecords";
                    }
                    break;
                case TutorialStep.ExamineRecords:
                    if ( patientRecords.tutorial_picked )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.CloseRecords;
                        particleHint.SetActive(false);
                        UItext.text = "Dit is de 'Bekijk modus'. Hier kun je objecten beter bekijken en controleren. Je kunt de 'Bekijk modus' van een object sluiten door te klikken op de knop 'Sluiten'. Probeer nu de 'Bekijk modus' sluiten. ";
                    }
                    break;
                case TutorialStep.CloseRecords:
                    if ( patientRecords.tutorial_closed )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.DropRecords;
					UItext.text = "Erg goed. Leg nu de cliëntgegevens terug op het werkveld door op de cliëntgegevens in je hand te drukken. Vervolgens kies je voor de optie 'Terugleggen'";
                        handsInventory.tutorial_droppedLeft = false;
                        itemToDrop = "PatientRecords";
                    }
                    break;
                case TutorialStep.DropRecords:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.ExaminePrescription;
                        particleHint.SetActive(true);
                        particleHint.transform.position = prescriptionForm.transform.position;
                        UItext.text = "De cliëntgegevens zijn gecontroleerd. Laten wij dit nu vergelijken met de toedienlijst door deze ook te controleren. Klik op de toedienlijst om hem op te pakken, klik nogmaals op de toedienlijst en kies vervolgens de optie 'Controleren'.";
                        prescriptionForm.tutorial_picked = false;
                        itemToPick = "PrescriptionForm";
                    }
                    break;
                case TutorialStep.ExaminePrescription:
                    if (prescriptionForm.tutorial_picked)
                    {
                        currentStep = TutorialStep.ClosePrescription;
                        particleHint.SetActive(false);
                        UItext.text = "Door de 'Bekijk modus' te openen van een object controleer je het object ook meteen. De toedienlijst is nu gecontroleerd net zoals de cliëntgegevens. Sluit nu de 'Bekijk modus' door te klikken op de 'Sluiten' knop.";
                    }
                    break;
                case TutorialStep.ClosePrescription:
                    if (prescriptionForm.tutorial_closed)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.DropPrescription;
					UItext.text = "Leg nu de toedienlijst terug op het werkveld door op de toedienlijst in je hand te klikken. Vervolgens kies je voor de optie 'Terugleggen'";
                        handsInventory.tutorial_droppedLeft = false;
                        itemToDrop = "PrescriptionForm";
                    }
                    break;
                case TutorialStep.DropPrescription:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.ExamineMedicine;
                        particleHint.transform.position = medicine.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Controleer nu het medicijn door hem op te pakken, er nogmaals in je hand op te klikken en in het optiemenu te kiezen voor de optie 'Controleren'.";
                        itemToPick = "Medicine";
                    }
                    break;
                case TutorialStep.ExamineMedicine:
                    if (medicine.tutorial_picked)
                    {
                        currentStep = TutorialStep.CloseMedicine;
					UItext.text = "Goed, het medicijn is gecontroleerd omdat we de 'Bekijk modus' hebben geopend. Sluit nu de 'Bekijk modus' door op 'Sluiten' te drukken.";
                    }
                    break;
                case TutorialStep.CloseMedicine:
                    if (medicine.tutorial_closed)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.DropMedicine1;
					UItext.text = "Leg nu het medicijn weer terug op het werkveld door op het medicijn in je hand te klikken. Vervolgens kies je voor de optie 'Terugleggen'";
                        handsInventory.tutorial_droppedLeft = false;
                        itemToDrop = "Medicine";
                    }
                    break;
                case TutorialStep.DropMedicine1:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.Overview1;
                        UItext.text = "Nu we alles hebben gecontroleerd is het van belang om een dubbele controle uit te voeren. Dit kan jouw collega doen. Druk op de 'Terug naar overzicht' knop om het werkveld te verlaten. ";
                        player.tutorial_movedBack = false;
                        player.tutorial_movementLock = movementLock = false;
                        particleHint.SetActive(false);
                    }
                    break;
                case TutorialStep.Overview1:
                    if (player.tutorial_movedBack)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.MoveToDoctor;
                        UItext.text = "Beweeg nu naar je collega toe door op je collega te klikken";
                        particleHint.SetActive(true);
                        particleHint.transform.position = doctor.transform.position;
                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveToDoctor:
                    if (player.tutorial_movedTo)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.ExplainTalking;
                        UItext.text = "Je kunt met mensen praten door op hen te klikken als je dichtbij staat";
                        doctor.tutorial_used = false;
                    }
                    break;
                case TutorialStep.ExplainTalking:
                    if (doctor.tutorial_used)
                    {
                        currentStep = TutorialStep.UseDoctor;
                        doctor.tutorial_talked = false;
                        particleHint.SetActive(false);
                        AddPointWithSound();
                        UItext.text = "Zodra je hebt geklikt op de persoon in kwestie, opent er een keuzemenu. Hierin kun je aangeven wat je aan een persoon wilt vragen. Vraag nu aan je collega om een dubbele controle door op de juiste keuze te klikken.";
                    }
                    break;
                case TutorialStep.UseDoctor:
                    if (doctor.tutorial_talked)
                    {
                        currentStep = TutorialStep.Overview2;
                        UItext.text = "Super! De dubbele controle is uitgevoerd. Druk op de 'Terug naar overzicht' om je collega te verlaten. ";
                        player.tutorial_movedBack = false;
                    }
                    break;
                case TutorialStep.Overview2:
                    if (player.tutorial_movedBack)
                    {

                        AddPointWithSound();
                        currentStep = TutorialStep.TalkDoubleCheck;
                        UItext.text = "Wanneer je een gesprek aangaat met je collega of een cliënt, opent er een dialoog scherm. Hier kun je een keuze maken tussen verschillende gespreksonderwerpen. Vraag nu aan je collega om de dubbele controle door de keuze te selecteren en op linkermuisknop te drukken.";
                    }
                    break;
                case TutorialStep.TalkDoubleCheck:
                    if (doctor.tutorial_talked)
                    {
                        particleHint.transform.position = alcohol.transform.position;
                        particleHint.SetActive(true);
                        currentStep = TutorialStep.PickAlcohol;
                        UItext.text = "Naast het gebruiken en bekijken van voorwerpen kunnen sommige voorwerpen worden opgepakt. Beweeg eerst naar het werkveld door erop te klikken. Probeer daarna de alcohol op te pakken door erop te klikken en te kiezen voor de optie 'Oppakken'.";
                        itemToPick = "Alcohol";
                        handsInventory.tutorial_pickedLeft = false;
                    }
                    break;
                case TutorialStep.PickAlcohol:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickCloth;
                        particleHint.transform.position = cloth.transform.position;
                        UItext.text = "Pak nu het gaasje op door op het gaasje te klikken. Kies daarna voor de optie 'Oppakken'.";
                        itemToPick = "Cloth";
                        handsInventory.tutorial_pickedRight = false;
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedLeft)
                        {
                            handsInventory.tutorial_pickedLeft = false;
                            AddPointWithSound();
                            UItext.text = "Wanneer je handen leeg zijn en je pakt iets op, zal het altijd in je linkerhand verschijnen.";
                            SetPauseTimer(5.0f);
                            particleHint.SetActive(false);
                        }
                    }
                    break;
                case TutorialStep.PickCloth:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.CombineAlcoholCloth;
                        controls.keyPreferences.CombineKey.locked = combineKeyLocked = false;
                        UItext.text = "Sommige voorwerpen kun je met elkaar combineren. Dit kan bijvoorbeeld met het gaasje en alcohol om zo het gaasje te desinfecteren. Voorwerpen combineren doe je door op de alcohol of het gaasje te klikken en dan te kiezen voor de optie 'Gebruiken met..' en vervolgens op het voorwerp in de andere hand te klikken.";
                        itemToPick = "";
                        SetPauseTimer(5.0f);
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedRight)
                        {
                            handsInventory.tutorial_pickedRight = false;
                            particleHint.SetActive(false);
                            AddPointWithSound();
                            UItext.text = "Wanneer je in je linkerhand al een voorwerp vast hebt zal het tweede voorwerp altijd in de rechterhand komen";
                            SetPauseTimer(5.0f);
                        }
                    }
                    break;
                case TutorialStep.CombineAlcoholCloth:
                    if (handsInventory.tutorial_combined)
                    {
                        currentStep = TutorialStep.DroppingExplanation;
                        handsInventory.tutorial_combined = false;
                        UItext.text = "Voorwerpen kun je terugleggen op het werkveld door op het voorwerp die je in je hand hebt te drukken en te kiezen voor de optie 'Terugleggen'.";
                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.DroppingExplanation:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.DropItem;
                        controls.keyPreferences.LeftDropKey.locked = leftDropKeyLocked = false;
                        UItext.text = "Het gaasje is nu gedesinfecteerd. Voorwerpen kun je terugleggen op het werkveld door op het voorwerp die je in je hand hebt te drukken en te kiezen voor de optie 'Terugleggen'. ";
                        itemToDrop = "Alcohol";
                        handsInventory.tutorial_droppedLeft = false;
                    }
                    break;
                case TutorialStep.DropItem:
                    if ( handsInventory.tutorial_droppedLeft )
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_droppedLeft = false;
                        currentStep = TutorialStep.DropAnotherItem;
                        controls.keyPreferences.RightDropKey.locked = rightDropKeyLocked = false;
                        UItext.text = "Probeer nu hetzelfde met het voorwerp in je andere hand. Klik op het object en kies de optie 'Terugleggen'.";
                        itemToDrop = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.DropAnotherItem:
                    if ( handsInventory.tutorial_droppedRight )
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.CombineDesinfMedicine;
                        controls.keyPreferences.pickObjectView.locked = pickObjectViewKeyLocked = false;
                        UItext.text = "Probeer nu het gedesïnfecteerde gaasje te combineren met het medicijn in flacon. Pak beide voorwerpen door één voor één erop te klikken en te kiezen voor de optie 'Oppakken'. Klik daarna op een van de voorwerpen in je handen en kies voor de optie 'Gebruiken met..' en daarna op het voorwerp in de andere hand. ";

                        particleHint.SetActive(true);
                        particleHint.transform.position = medicine.transform.position;

                        itemToPick = "Medicine";
                        itemToPick2 = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.CombineDesinfMedicine:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.DropClothMedicine;
                        UItext.text = "De rubberen dop van de flacon is nu gedesinfecteerd. Leg nu beide voorwerpen terug door op het voorwerp te klikken en de optie 'Terugleggen' te kiezen.";

                        itemToDrop2 = "Medicine";
                        itemToDrop = "DesinfectionCloth";

                        particleHint.SetActive(false);

                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                    }
                    break;
                case TutorialStep.DropClothMedicine:
                    if ( handsInventory.tutorial_droppedLeft || handsInventory.tutorial_droppedRight)
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.PickSyringeAbNeedleCap;
					UItext.text = "Pak nu de spuit op en de opzuignaald door op de voorwerpen te klikken en de optie 'Oppakken' te kiezen.";

                        itemToPick = "Syringe";
                        itemToPick2 = "AbsorptionNeedle";

                        particleHint.transform.position = syringe.transform.position;
                        particleHint_alt.transform.position = GameObject.Find("AbsorptionNeedle").transform.position;
                        particleHint.SetActive(true);
                        particleHint_alt.SetActive(true);
                    }
                    break;
                case TutorialStep.PickSyringeAbNeedleCap:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight)
                    {
                        AddPointWithSound();
                        particleHint.SetActive(false);
                        particleHint_alt.SetActive(false);

                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.CombineSyringeAbNeedleCap;
                        UItext.text = "Combineer nu de opzuignaald met de spuit door op een van de voorwerpen te drukken en dan te kiezen voor de optie 'Gebruiken met..' en daarna te klikken op het voorwerp in de andere hand. ";
                    }
                    break;
                case TutorialStep.CombineSyringeAbNeedleCap:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineTakeOffAbCap;
                        UItext.text = "Naast het combineren van voorwerpen kunnen sommige voorwerpen ook uit elkaar gehaald worden. Dit kun je doen wanneer je een vrije hand hebt en in je andere hand een voorwerp. Probeer nu de veiligheidsdop van de opzuignaald af te halen, klik op het voorwerp in je hand en kies de optie 'Scheiden'.";
                    }
                    break;
                case TutorialStep.CombineTakeOffAbCap:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineSyringeAbNeedleMed;
                        UItext.text = "Probeer nu het medicijn op te zuigen in de spuit. Zorg dat je het medicijn en de spuit vast hebt in je handen. Klik hierna op het medicijn of de spuit en kies voor de optie 'Gebruiken met..' en klik daarna op het voorwerp in de andere hand.";

                        itemToPick = "Medicine";

                        particleHint.transform.position = medicine.transform.position;
                        particleHint.SetActive(true);
                    }
                    break;
                case TutorialStep.CombineSyringeAbNeedleMed:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.DropMedicine;
                        UItext.text = "Het medicijn is nu opgezogen in de spuit. Leg het medicijn terug op het werkveld door op het medicijn in je hand te klikken en vervolgens te kiezen voor de optie 'Terugleggen'.";

                        itemToDrop = "Medicine";

                        controls.keyPreferences.LeftUseKey.locked =
                            controls.keyPreferences.RightUseKey.locked =
                            leftUseKeyLocked = rightUseKeyLocked = false;

                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;

                        particleHint.SetActive(false);
                    }
                    break;
                case TutorialStep.DropMedicine:
                    if (handsInventory.tutorial_droppedLeft || handsInventory.tutorial_droppedRight)
                    {
                        AddPointWithSound();

                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.UseSyringeWithMed;
                        UItext.text = "Sommige voorwerpen die je in je hand hebt kun je gebruiken. Dit betekent dat je een actie kunt uitvoeren met het voorwerp in je hand. Probeer de spuit te ontluchten, dit doe je door de spuit in je hand te nemen. Je andere hand dient vrij te zijn om acties op een voorwerp uit te voeren. Klik vervolgens op de spuit en kies voor de optie 'Gebruiken'.";

                        syringe = handsInventory.LeftHandEmpty() ?
                            handsInventory.RightHandObject : handsInventory.LeftHandObject;
                    }
                    break;
                case TutorialStep.UseSyringeWithMed:
                    if (syringe.GetComponent<PickableObject>().tutorial_usedOn)
                    {
                        syringe.GetComponent<PickableObject>().tutorial_usedOn = false;

                        currentStep = TutorialStep.DecombineAbsSyringe;
                        UItext.text = "decombine";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.DecombineAbsSyringe:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.UseNeedleOnTrashAbs;
                        UItext.text = "Voorwerpen kun je ook gebruiken met andere voorwerpen. Probeer nu de opzuignaald te verwijderen met de naaldcontainer. Zorg ervoor dat je de spuit in je hand hebt en dat de andere hand vrij is. Klik vervolgens op de spuit en kies voor de optie 'Gebruiken met..'. Klik vervolgens op de naaldcontainer. Je kunt de actie 'Gebruiken met..' annuleren door te klikken op 'Annuleren'";

                        particleHint.transform.position = GameObject.Find("NeedleCup").transform.position;
                        particleHint.SetActive(true);
                    }
                    break;
                case TutorialStep.UseNeedleOnTrashAbs:
                    if (absorptionNeedle == null)
                    {
                        absorptionNeedle = GameObject.Find("AbsorptionNeedleNoCap");
                    }
                    if (absorptionNeedle != null && absorptionNeedle.GetComponent<PickableObject>().tutorial_usedOn)
                    {
                        absorptionNeedle.GetComponent<PickableObject>().tutorial_usedOn = false;
                        currentStep = TutorialStep.PickInjectionNeedle;
                        UItext.text = "Nu het medicijn is opgezogen en de opzuignaald is weggegooid. Is het tijd om de injectienaald op de spuit te zetten. Zorg ervoor dat je de spuit en de injectienaald in je handen hebt . Pak de voorwerpen door erop te klikken en te kiezen voor de optie 'Oppakken'.";

                        itemToPick = "InjectionNeedle";
                        particleHint.transform.position = GameObject.Find("InjectionNeedle").transform.position;
                    }
                    break;
                case TutorialStep.PickInjectionNeedle:
                    if (handsInventory.tutorial_pickedLeft || handsInventory.tutorial_pickedRight)
                    {
                        AddPointWithSound();
                        particleHint.SetActive(false);

                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.CombineSyringeInjNeedle;
                        UItext.text = "Combineer nu de injectienaald met de spuit om de spuit klaar te maken voor injecteren. Zorg dat je de spuit en injectienaald in je handen hebt en druk op een van de twee voorwerpen. Kies vervolgens de optie 'Gebruiken met..' en klik daarna op het voorwerp in de andere hand. ";
                    }
                    break;
                case TutorialStep.CombineSyringeInjNeedle:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        player.tutorial_movedBack = false;
                        currentStep = TutorialStep.Overview3;
                        UItext.text = "De injectiespuit is klaar voor gebruik. Ga terug naar het overzicht door op de 'Terug naar overzicht' knop te drukken ";
                        player.tutorial_movementLock = movementLock = false;
                    }
                    break;
                case TutorialStep.Overview3:
                    if (player.tutorial_movedBack)
                    {
                        player.tutorial_movedBack = false;
                        currentStep = TutorialStep.MoveToPatient;
                        UItext.text = "Nu we terug in het overzicht zijn kunnen we de cliënt aanklikken om naar hem toe te bewegen. Druk op de cliënt";

                        patientTrigger.gameObject.SetActive(true);
                        particleHint.transform.position = patientTrigger.transform.position;
                        particleHint.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveToPatient:
                    if ( Vector3.Distance(player.transform.position, patientTrigger.position) < 1.0f)
                    {
                        AddPointWithSound();

                        patientTrigger.gameObject.SetActive(false);

                        currentStep = TutorialStep.TalkToPatient;
                        particleHint.transform.position = GameObject.Find("Patient").transform.position;
                        UItext.text = "Vraag aan de cliënt of hij zijn mouw omhoog wilt doen. Open het dialoogscherm door op de client te klikken. Kies vervolgens 'Zou u uw mouw omhoog willen doen?'. ";
                    }
                    break;
                case TutorialStep.TalkToPatient:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;
                        currentStep = TutorialStep.UseOnHand;
                        UItext.text = "Gebruik nu de injectiespuit op de cliënt. Doe dit net zoals je hebt gedaan bij het weggooien van de opzuignaald in de naaldcontainer. Klik op de injectiespuit, kies de optie 'Gebruiken met...', en klik vervolgens op de patiënt.";

                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOnHand:
                    if ( handsInventory.tutorial_itemUsedOn )
                    {
                        particleHint.SetActive(false);
                        currentStep = TutorialStep.SequenceExplanation;
                    }
                    break;
                case TutorialStep.SequenceExplanation:
                    if ( TimerElapsed() )
                    {
                        currentStep = TutorialStep.CompleteSequence;
                        sequenceCompleted = false;
                        PlayerAnimationManager.SequenceTutorialLock(false);
                        UItext.text = "In deze instructie zijn de juiste keuzes aangegeven in het groen. Doorloop nu de verschillende stappen om het injecteren uit te voeren door de juiste keuzes te selecteren.";
                    }
                    else
                    {
                        UItext.text = "Ingewikkelde handelingen die uit meerdere stappen bestaan openen het 'actie keuze menu'. De handeling start automatisch. Zodra er een belangrijke stap is aangebroken, pauzeert het spel. Er verschijnen keuzes in het beeld en het is aan jou om de juiste keuze te selecteren. Zodra je de goede keuze maakt zal de handeling verder worden uitgevoerd. Dit herhaalt zich tot de handeling is afgerond.";
                        SetPauseTimer(7.0f);
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if ( sequenceCompleted )
                    {
                        currentStep = TutorialStep.Overview4;
                        player.tutorial_movementLock = movementLock = false;

                        UItext.text = "Goed gedaan! Het injecteren is nu klaar en je kunt opruimen. Om terug te gaan naar het werkveld klik je weer op de 'Terug naar het overzicht' knop en vervolgens op het werkveld.";
                    }
                    break;
                case TutorialStep.Overview4:
                    {
                        if (player.tutorial_movedBack)
                        {
                            player.tutorial_movedBack = false;

                            currentStep = TutorialStep.DecombineInjSyringe;
                            UItext.text = "decombine";
                            
                            handsInventory.tutorial_combined = false;
                        }
                    }
                    break;
                case TutorialStep.DecombineInjSyringe:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.UseNeedleOnTrashInj;
                        UItext.text = "Verwijder nu de injectienaald door de spuit te gebruiken met de naaldcontainer. Zorg ervoor dat je de spuit in je hand hebt en dat de andere hand vrij is. Klik vervolgens op de spuit en kies voor de optie 'Gebruiken met..' Klik vervolgens op de naaldcontainer. Je kunt de actie 'Gebruiken met..' annuleren door op de knop 'Annuleren' te klikken.";

                        particleHint.transform.position = GameObject.Find("NeedleCup").transform.position;
                        particleHint.SetActive(true);
                    }
                    break;
                case TutorialStep.UseNeedleOnTrashInj:
                    if (injectionNeedle == null)
                    {
                        injectionNeedle = GameObject.Find("InjectionNeedleNoCap");
                    }
                    if (injectionNeedle != null && injectionNeedle.GetComponent<PickableObject>().tutorial_usedOn)
                    {
                        injectionNeedle.GetComponent<PickableObject>().tutorial_usedOn = false;

                        currentStep = TutorialStep.DropSyringe;
                        UItext.text = "Leg nu de spuit terug op het werkveld door op de spuit te klikken en te kiezen voor de optie 'Terugleggen'. ";

                        itemToDrop = "Syringe";
                        particleHint.SetActive(false);
                    }
                    break;
                case TutorialStep.DropSyringe:
                    if (handsInventory.Empty())
                    {
                        AddPointWithSound();

                        currentStep = TutorialStep.UseWorkField;
                        UItext.text = "Ruim de spullen op en maak het werkveld schoon. Dit doe je door op het werkveld te klikken en daarna te kiezen voor de optie 'Gebruiken'. ";

                        workField.tutorial_used = false;

                        particleHint.SetActive(true);
                        particleHint.transform.position = workField.transform.position;
                    }
                    break;
                case TutorialStep.UseWorkField:
                    if (workField.tutorial_used)
                    {
                        currentStep = TutorialStep.UseHygienePump;
                        UItext.text = "Desinfecteer altijd je handen na het uitvoeren van een medische handeling. Doe dit door te kijken op de hygiënepomp te klikken en daarna te kiezen voor de optie 'Gebruiken'. ";

                        handCleaner.tutorial_used = false;
                        particleHint.transform.position = handCleaner.transform.position;
                    }
                    break;
                case TutorialStep.UseHygienePump:
                    if (handCleaner.tutorial_used)
                    {
                        currentStep = TutorialStep.UsePaperAndPen;
                        UItext.text = "Zoals je ziet verschijnt er een pen en een papier op de tafel. Deze kun je gebruiken om je bevindingen op te schrijven. Doe dit door op het papier te klikken en daarna te kiezen voor de optie 'Gebruiken'.";

                        particleHint.transform.position = paperNPen.transform.position;
                        paperNPen.tutorial_used = false;
                    }
                    break;
                case TutorialStep.UsePaperAndPen:
                    if (paperNPen.tutorial_used)
                    {
                        particleHint.SetActive(false);

                        currentStep = TutorialStep.None;
                        endPanel.SetActive(true);
                        player.enabled = false;
                        GameObject.FindObjectOfType<RobotManager>().enabled = false;
                    }
                    break;
                default:
                    break;
            }
        }
	}

    public void EndButtonClick()
    {
        //actionManager.OnUseAction("__tutorialEnd");
        GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().tutorialCompleted = true;
        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("Menu");
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

    private void SetAllKeysLocked(bool value)
    {
        mouseClickLocked = value;
        closeObjectViewLocked = value;
        leftDropKeyLocked = value;
        rightDropKeyLocked = value;
        combineKeyLocked = value;
        getHintKeyLocked = value;
        pickObjectViewKeyLocked = value;
        leftUseKeyLocked = value;
        rightUseKeyLocked = value;
        controls.keyPreferences.SetAllLocked(value);
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
     
    private void AddPointWithSound()
    {
        actionManager.UpdatePoints(1);
        ActionManager.CorrectAction();
    }
}
