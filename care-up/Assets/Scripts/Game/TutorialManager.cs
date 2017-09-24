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
        LookAround,                // step 1
        WalkAround,                // step 2
        WalkToTable,               // step 3
        UseHandCleaner,            // step 4
        UseTable,                  // step 5
        ExamineRecords,            // step 5
        CloseRecords,              // step 5
        ExamineMedicine,           // step 5
        CloseMedicine,             // inserted
        UseDoctor,                 // inserted
        TalkDoubleCheck,           // inserted
        PickAlcohol,               // step 6
        PickCloth,                 // step 7
        CombineAlcoholCloth,       // step 8
        DroppingExplanation,       // step 9
        DropItem,                  // step 9
        DropAnotherItem,           // step 9
        PickBothItems,             // step 9
        WalkAway,                  // step 10
        DropBothItems,             // step 10
        PickBothItemsAgain,        // step 10
        DropBothItemAway,          // inserted
        CombineDesinfMedicine,     // step 11

        DropClothMedicine,         // step 12
        PickSyringeAbNeedleCap,    // step 13
        CombineSyringeAbNeedleCap, // step 14
        CombineTakeOffAbCap,       // step 15
        DropAbsorptionCap,         // step 16
        CombineSyringeAbNeedleMed, // step 17
        DropMedicine,              // step 18
        UseSyringeWithMed,         // step 19
        PickUpAbsorptionCap,       // step 20
        CombinePutOnAbCap,         // step 21
        CombineTakeOffAbNeedle,    // step 22
        UseAbNeedleOnTrash,        // step 23

        PickInjectionNeedle,       // step 24
        CombineSyringeInjNeedle,   // step 25
        //CombineTakeOffInjCap,    // step 26 : should not take cap off
        //DropInjectionCap,        // step 27 : it's done in sequence
        
        MoveToPatient,             // step 
        TalkToPatient,             // step 28 

        UseOnHand,                 // step 29
        SequenceExplanation,       // step 30
        CompleteSequence,          // step 30

        CombinePutOnInjCap,        // step 31
        CombineTakeOffInjNeedle,   // step 32
        UseInjNeedleOnTrash,       // step 33
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
    
    private RigidbodyFirstPersonController player;
    private ActionManager actionManager;
    private HandsInventory handsInventory;
    private Controls controls;
    private UsableObject handCleaner;
    private WorkField workField;
    private ExaminableObject patientRecords;
    private GameObject syringe;
    private GameObject needle;
    private ExaminableObject medicine;
    private PickableObject alcohol;
    private PickableObject cloth;
    private UsableObject paperNPen;
    private InjectionPatient patient;
    private PersonObject doctor;

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
        workField = GameObject.Find("WorkField").GetComponent<WorkField>();
        patientRecords = GameObject.Find("PatientRecords").GetComponent<ExaminableObject>();

        Transform interactables = GameObject.Find("Interactable Objects").transform;
        medicine = interactables.Find("Medicine").GetComponent<ExaminableObject>();
        syringe = interactables.Find("Syringe").gameObject;
        needle = interactables.Find("AbsorptionNeedle").gameObject;
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
                        SetAllKeysLocked(true);
                        controls.keyPreferences.mouseClickLocked = mouseClickLocked = false;
                        controls.keyPreferences.mouseClickKey.locked = false;
                        player.tutorial_movementLock = movementLock = true;
                        UItext.text = "Welkom bij Care-Up";
                        SetPauseTimer(2.0f);
                    }
                    break;
                case TutorialStep.LookAround:
                    if ( player.tutorial_totalLookAround > 30.0f )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.WalkAround;
                        player.tutorial_movementLock = movementLock = false;
                        UItext.text = "Gebruik de W, A, S, D / pijltjestoetsen om te lopen ";
                    }
                    break;
                case TutorialStep.WalkAround:
                    if ( player.tutorial_totalMoveAround > 50.0f )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.WalkToTable;
                        particleHint.transform.position = tableTrigger.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Beweeg naar de tafel door W, A, S, D / pijltjestoetsen en de muis te gebruiken";
                    }
                    break;
                case TutorialStep.WalkToTable:
                    if ( Vector3.Distance(tableTrigger.position, player.transform.position) < 0.6f)
                    {
                        AddPointWithSound();
                        tableTrigger.gameObject.SetActive(false);
                        player.tutorial_movementLock = movementLock = true;
                        currentStep = TutorialStep.UseHandCleaner;
                        controls.keyPreferences.closeObjectView.locked = closeObjectViewLocked = false;
                        particleHint.transform.position = handCleaner.transform.position;
                        UItext.text = "Sommige oplichtende voorwerpen met een hand icoon erboven kunnen gebruikt worden door naar het voorwerp te kijken en op de linkermuisknop te drukken. Kijk nu naar de hygiënepomp en druk op de linkermuisknop om handhygiëne toe te passen. ";
                    }
                    break;
                case TutorialStep.UseHandCleaner:
                    if (handCleaner.tutorial_used)
                    {
                        currentStep = TutorialStep.UseTable;
                        particleHint.transform.position = workField.transform.position;
                        UItext.text = "Maak je werkveld schoon. Dit kan door naar het werkveld te kijken en op de linkermuisknop te drukken. Hier zie je ook weer het 'hand' icoon die aangeeft dat je het voorwerp kunt gebruiken.";
                    }
                    break;
                case TutorialStep.UseTable:
                    if (workField.tutorial_used)
                    {
                        currentStep = TutorialStep.ExamineRecords;
                        particleHint.transform.position = patientRecords.transform.position;
                        UItext.text = "Oplichtende voorwerpen met een vergrootglas icoon kunnen bekeken en gecontroleerd worden. Dit geldt voor bijvoorbeeld de clientgegevens of het medicijn. Dit doe je door naar het voorwerp met een vergrootglas icoon te kijken en op de linkermuisknop te drukken.   ";
                        patientRecords.tutorial_picked = false;
                    }
                    break;
                case TutorialStep.ExamineRecords:
                    if ( patientRecords.tutorial_picked )
                    {
                        currentStep = TutorialStep.CloseRecords;
                        particleHint.SetActive(false);
                        UItext.text = "Als je met de linkermuisknop op een voorwerp met een vergrootglas icoon druk, kom je in het voorwerp bekijk modus. Je kunt door te scrollen met je muiswiel om in/uit te zoomen. Je kunt voorwerpen draaien door de linkermuisknop ingedrukt te houden en te bewegen met de muis. Het voorwerp terugleggen doe je met de 'Q' knop. Het bekeken/gecontroleerde voorwerp kun je oppakken met de 'E' knop. Dit kan echter niet met alle voorwerpen. Leg nu de clientgegevens terug door op 'Q' te drukken.   ";
                    }
                    break;
                case TutorialStep.CloseRecords:
                    if ( patientRecords.tutorial_closed )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.ExamineMedicine;
                        particleHint.transform.position = medicine.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Controleer nu het medicijn door er naar te kijken en op de linkermuisknop te drukken.";
                    }
                    break;
                case TutorialStep.ExamineMedicine:
                    if (medicine.tutorial_picked)
                    {
                        currentStep = TutorialStep.CloseMedicine;
                        UItext.text = "Goed, zoals je ziet kun je het medicijn naast terugleggen ook oppakken met de'E'toets. We hebben het medicijn op dit moment nog niet nodig en hoeven hem dus niet op te pakken. Leg nu het medicijn terug door op 'Q' te drukken.";
                    }
                    break;
                case TutorialStep.CloseMedicine:
                    if (medicine.tutorial_closed)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.UseDoctor;
                        particleHint.transform.position = doctor.transform.position;
                        doctor.tutorial_talked = false;
                        player.tutorial_movementLock = movementLock = false;
					    UItext.text = "Loop richting je collega. Je kunt met sommige mensen praten. Mensen waarmee je kunt praten kun je herkennen aan het praat icoon die verschijnt wanneer je naar deze persoon kijkt. Als je wilt praten met deze persoon klik je op de linkermuisknop. Ga nu het gesprek aan met je collega. ";
                        doctor.tutorial_used = false;
                    }
                    break;
                case TutorialStep.UseDoctor:
                    if (doctor.tutorial_used)
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.TalkDoubleCheck;
                        UItext.text = "Wanneer je een gesprek aangaat met je collega of een client, opent er een dialoog scherm. Hier kun je een keuze maken tussen verschillende gespreksonderwerpen. Vraag nu aan je collega om de dubbele controle door de keuze te selecteren en op linkermuisknop te drukken.";
                        
                    }
                    break;
                case TutorialStep.TalkDoubleCheck:
                    if (doctor.tutorial_talked)
                    {
                        particleHint.transform.position = alcohol.transform.position;
                        currentStep = TutorialStep.PickAlcohol;
                        UItext.text = "Naast het gebruiken en bekijken van voorwerpen kunnen sommige voorwerpen worden opgepakt. Oplichtende voorwerpen met een hand en pijl icoon kunnen worden opgepakt. Probeer nu de alcohol op te pakken door te kijken naar de alcohol en op linkermuisknop te drukken.";
                        itemToPick = "Alcohol";
                    }
                    break;
                case TutorialStep.PickAlcohol:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickCloth;
                        particleHint.transform.position = cloth.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Pak nu het gaasje op door ernaar te kijken en op de linkermuisknop te drukken.";
                        itemToPick = "Cloth";
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedLeft)
                        {
                            handsInventory.tutorial_pickedLeft = false;
                            AddPointWithSound();
                            UItext.text = "Wanneer je handen leeg zijn en je pakt iets op. Zal het altijd in je linkerhand verschijnen.";
                            SetPauseTimer(3.0f);
                            particleHint.SetActive(false);
                        }
                    }
                    break;
                case TutorialStep.PickCloth:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.CombineAlcoholCloth;
                        controls.keyPreferences.CombineKey.locked = combineKeyLocked = false;
                        UItext.text = "Sommige voorwerpen kun je met elkaar combineren. Dit kan bijvoorbeeld met het gaasje en alcohol om zo het gaasje te desinfecteren. Voorwerpen combineren doe je door de 'R' knop in te drukken als je twee voorwerpen in je handen hebt. Probeer dit nu met het gaasje en de alcohol. ";
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
                            SetPauseTimer(3.0f);
                        }
                    }
                    break;
                case TutorialStep.CombineAlcoholCloth:
                    if (handsInventory.tutorial_combined)
                    {
                        currentStep = TutorialStep.DroppingExplanation;
                        handsInventory.tutorial_combined = false;
                        UItext.text = "Dropping expl";
                    }
                    break;
                case TutorialStep.DroppingExplanation:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.DropItem;
                        controls.keyPreferences.LeftDropKey.locked = leftDropKeyLocked = false;
                        UItext.text = "Het gaasje is nu gedesinfecteerd. Voorwerpen die je in je hand hebt kun je terugleggen door 'SHIFT' ingedrukt te houden en te drukken op de actietoets van de juiste arm. De 'Q' toets staat voor de actietoets van de linkerhand. Probeer met 'SHIFT' + 'Q' het voorwerp in je linkerhand terug te leggen op het werkveld. ";
                        itemToDrop = "Alcohol";
                    }
                    break;
                case TutorialStep.DropItem:
                    if ( handsInventory.tutorial_droppedLeft )
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_droppedLeft = false;
                        currentStep = TutorialStep.DropAnotherItem;
                        controls.keyPreferences.RightDropKey.locked = rightDropKeyLocked = false;
                        UItext.text = "Probeer nu hetzelfde met de rechterhand. De actietoets van de rechterhand is de 'E' toets. Probeer nu met met 'SHIFT' + 'E' het gaasje in je linkerhand neer te leggen op het werkveld.";
                        itemToDrop = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.DropAnotherItem:
                    if ( handsInventory.tutorial_droppedRight )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.PickBothItems;
                        handsInventory.tutorial_droppedRight = false;
                        UItext.text = "Pak nu de aclohol en het gedesinfecteerde gaasje weer op door naar de voorwerpen te kijken en op de linkermuisknop te drukken.";

                        itemToPick = "Alcohol";
                        itemToPick2 = "DesinfectionCloth";

                        particleHint.transform.position = alcohol.transform.position;
                        particleHint.SetActive(true);
                        particleHint_alt.transform.position = GameObject.Find("DesinfectionCloth").transform.position;
                        particleHint_alt.SetActive(true);
                    }
                    break;
                case TutorialStep.PickBothItems:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight )
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.WalkAway;
                        particleHint.SetActive(false);
                        particleHint_alt.SetActive(false);
                        UItext.text = "Loop met de W, A, S, D / pijltjestoetsen en de muis bij het werkveld vandaan.";
                    }
                    break;
                case TutorialStep.WalkAway:
                    if ( Vector3.Distance(player.transform.position, tableTrigger.position) > 5.0f )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.DropBothItems;
                        UItext.text = "Laat nu beide voorwerpen op de grond vallen door op 'SHIFT' + 'Q' te drukken om het voorwerp in je linkerhand te laten vallen en druk op 'SHIFT' + 'E' om het voorwerp in je rechterhand te laten vallen.";
                        itemToDrop = "Alcohol";
                        itemToDrop2 = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.DropBothItems:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickBothItemsAgain;
                        UItext.text = "Pak nu beide voorwerpen weer van de grond door ernaar te kijken en op de linkermuisknop te drukken.";
                        particleHint.transform.position = alcohol.transform.position;
                        particleHint_alt.transform.position = GameObject.Find("DesinfectionCloth").transform.position;
                        particleHint.SetActive(true);
                        particleHint_alt.SetActive(true);

                        itemToPick = "Alcohol";
                        itemToPick2 = "DesinfectionCloth";
                    }
                    else
                    {
                        if ( handsInventory.tutorial_droppedLeft && handsInventory.tutorial_droppedRight)
                        {
                            AddPointWithSound();
                            handsInventory.tutorial_droppedLeft =
                                handsInventory.tutorial_droppedRight = false;
                            UItext.text = "Voorwerpen kun je overal waar je wilt laten vallen. Echter zorgt het laten vallen van voorwerpen op de grond voor strafpunten. Leg voorwerpen terug op het werkveld door dichtbij het werkveld te gaan staan en naar het werkveld te kijken terwijl je voorwerpen teruglegt om strafpunten te voorkomen.";
                            SetPauseTimer(15.0f);
                        }
                    }
                    break;
                case TutorialStep.PickBothItemsAgain:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight)
                    {
                        AddPointWithSound();
                        particleHint.SetActive(false);
                        particleHint_alt.SetActive(false);
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.DropBothItemAway;
                        UItext.text = "Leg nu beide voorwerpen terug op het werkveld door dichtbij het werkveld te gaan staan. Daarna druk je op 'SHIFT' + 'Q' om het voorwerp in je linkerhand terug te leggen en druk op 'SHIFT' +'E' om het voorwerp in je rechterhand terug te leggen.";
                    }
                    break;
                case TutorialStep.DropBothItemAway:
                    if (Vector3.Distance(player.transform.position, workField.transform.position) > 2.0f)
                    {
                        controls.keyPreferences.RightDropKey.locked = rightDropKeyLocked = true;
                        controls.keyPreferences.LeftDropKey.locked = leftDropKeyLocked = true;
                    }
                    else
                    {
                        controls.keyPreferences.RightDropKey.locked = rightDropKeyLocked = false;
                        controls.keyPreferences.LeftDropKey.locked = leftDropKeyLocked = false;
                    }
                    if ( handsInventory.tutorial_droppedLeft && handsInventory.tutorial_droppedRight)
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.CombineDesinfMedicine;
                        controls.keyPreferences.pickObjectView.locked = pickObjectViewKeyLocked = false;
                        UItext.text = "Probeer nu het gedesinfecteerde gaasje te combineren met het medicijn in flacon. Pak beide voorwerpen op door ernaar te kijken en op de linkermuisknop. Als je beide voorwerpen in je handen hebt, druk je op de 'R' toets om de voorwerpen te combineren.";

                        particleHint.SetActive(true);
                        particleHint.transform.position = medicine.transform.position;
                    }
                    break;
                case TutorialStep.CombineDesinfMedicine:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.DropClothMedicine;
                        UItext.text = "De rubberen dop van de flacon is nu gedesinfecteerd. Leg nu beide voorwerpen terug door 'SHIFT' + 'Q' te drukken om het voorwerp in je linkerhand terug te leggen en op 'SHIFT' + 'E' om het voorwerp in je rechthand terug te leggen.";

                        itemToDrop2 = "Medicine";
                        itemToDrop = "DesinfectionCloth";

                        particleHint.SetActive(false);

                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                    }
                    break;
                case TutorialStep.DropClothMedicine:
                    if ( handsInventory.tutorial_droppedLeft && handsInventory.tutorial_droppedRight)
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.PickSyringeAbNeedleCap;
					UItext.text = "Pak nu de spuit op en de opzuignaald door naar de voorwerpen te kijken en op de linkermuisknop te drukken.";

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
                        UItext.text = "Combineer nu de opzuignaald met de spuit door op de 'R' toets te drukken. ";
                    }
                    break;
                case TutorialStep.CombineSyringeAbNeedleCap:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineTakeOffAbCap;
                        UItext.text = "Naast het combineren van voorwerpen kunnen sommige voorwerpen ook uit elkaar gehaald worden. Dit kun je doen wanneer je een vrije hand hebt en in je andere hand een voorwerp. Druk de 'R' toets in om het voorwerp uit elkaar te halen. Probeer nu de veiligheidsdop van de opzuignaald af te halen door op de 'R' toets te drukken.";                    }
                    break;
                case TutorialStep.CombineTakeOffAbCap:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.DropAbsorptionCap;
                        UItext.text = "Leg de opzuignaald dop terug op het welkveld door naar het werkeld te kijken en op 'SHIFT' + 'Q' te drukken als je de dop in je linkerhand vast hebt en op 'SHIFT + 'E' als je de dop in je rechterhand vast hebt.";

                        itemToDrop = "SyringeAbsorptionCap";
                    }
                    break;
                case TutorialStep.DropAbsorptionCap:
                    if (handsInventory.tutorial_droppedLeft || handsInventory.tutorial_droppedRight)
                    {
                        AddPointWithSound();
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.CombineSyringeAbNeedleMed;
                        UItext.text = "Zorg dat je de spuit in je hand hebt en pak het medicijn op door ernaar te kijken en op de linkermuisknop te druken. Combineer de spuit met het medicijn door op de 'R' toets te drukken.";

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
                        UItext.text = "Het medicijn is nu opgezogen in de spuit. Leg het medicijn terug op het werkveld door op 'SHIFT' + 'Q' te drukken als je het medicijn in je linkerhand vast hebt en op 'SHIFT + 'E' als je het medicijn in je rechterhand vast hebt. ";

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
                        UItext.text = "Sommige voorwerpen die je in je hand hebt kun je gebruiken. Dit doe je door op de actietoets te drukken van de hand waarin je het voorwerp vast houdt. De actietoets voor de linkerhand is de 'Q' toets en voor de rechterhand de 'E' toets. Probeer nu de spuit te ontluchten door op de actieknop te drukken van de hand die de spuit vast heeft. Kijk hierbij NIET naar het werkveld. ";

                        syringe = handsInventory.LeftHandEmpty() ?
                            handsInventory.RightHandObject : handsInventory.LeftHandObject;
                    }
                    break;
                case TutorialStep.UseSyringeWithMed:
                    if (syringe.GetComponent<PickableObject>().tutorial_usedOn)
                    {
                        syringe.GetComponent<PickableObject>().tutorial_usedOn = false;
                        currentStep = TutorialStep.PickUpAbsorptionCap;
                        UItext.text = "De spuit is nu ontlucht. Laten we nu de beschermdop van de opzuignaald terugplaatsen op de spuit. Zorg dat je de spuit in je hand hebt en pak de beschermdop van de opzuignaald op door ernaar te kijken en op de linkermuisknop te drukken.";

                        itemToPick = "SyringeAbsorptionCap";
                        particleHint.transform.position = GameObject.Find("SyringeAbsorptionCap").transform.position;
                        particleHint.SetActive(true);
                    }
                    break;
                case TutorialStep.PickUpAbsorptionCap:
                    if (handsInventory.tutorial_pickedLeft || handsInventory.tutorial_pickedRight)
                    {
                        AddPointWithSound();
                        particleHint.SetActive(false);

                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.CombinePutOnAbCap;
                        UItext.text = "Combineer nu de spuit met de beschermdop door op de 'R' toets te drukken.";
                    }
                    break;
                case TutorialStep.CombinePutOnAbCap:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineTakeOffAbNeedle;
                        UItext.text = "Nu de beschermdop weer op de naald zit. Kan de opzuignaald worden verwijderd. Zorg ervoor dat je een vrije hand hebt en dat je de spuit in je andere hand hebt. Klik daarna op 'R' om de opzuignaald van de spuit af te halen.";
                    }
                    break;
                case TutorialStep.CombineTakeOffAbNeedle:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.UseAbNeedleOnTrash;
                        UItext.text = "Sommige voorwerpen kun je gebruiken op een ander voorwerp. Probeer de gebruikte opzuignaald te gebruiken op de naaldcontainer om de opzuignaald weg te gooien. Dit kun je doen door te kijken naar de naaldcontainer en dan te drukken op de actieknop van de hand die de opzuignaald vast heeft. De actieknop voor de linkerhand is de 'Q' toets en voor de rechterhand de 'E' toets.";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("NeedleCup").transform.position;
                    }
                    break;
                case TutorialStep.UseAbNeedleOnTrash:
                    if (needleTrashed)
                    {
                        needleTrashed = false;
                        currentStep = TutorialStep.PickInjectionNeedle;
                        UItext.text = "Nu het medicijn is opgezogen en de opzuignaald is weggegooid. Is het tijd om de injectienaald op de spuit te zetten. Zorg ervoor dat je de spuit in je handen hebt en de injectie naald. Pak de voorwerpen door ernaar te kijken en op de linkerhmuisknop te drukken.";

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
                        UItext.text = "Combineer nu de injectienaald met de spuit om de spuit klaar te maken voor injecteren. Zorg dat je de spuit en injectienaald in je handen hebt en druk op de combineertoets (R). ";
                    }
                    break;
                case TutorialStep.CombineSyringeInjNeedle:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.MoveToPatient;
                        UItext.text = "De spuit is klaar voor gebruik. Beweeg nu naar de de client te om met hem te praten.";

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

                        player.tutorial_movementLock = movementLock = true;
                        currentStep = TutorialStep.TalkToPatient;
                        particleHint.transform.position = GameObject.Find("Patient").transform.position;
                        UItext.text = "Vraag aan de client of hij zijn mouw omhoog wil doen. Open het dialoogscherm door naar de client te kijken en op de linkermuisknop te drukken.";
                    }
                    break;
                case TutorialStep.TalkToPatient:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;
                        currentStep = TutorialStep.UseOnHand;
                        UItext.text = "Gebruik nu de injectiespuit op de client. Doe dit net zoals je hebt gedaan bij het weggooien van de opzuignaald in de naaldcontainer. Kijk naar de client en druk op de actie toets van de hand die de injectiespuit vast heeft. Druk op de 'Q' voor de linkerhand en 'E' voor de rechterhand.";

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
                        UItext.text = "In deze instructie zijn de juiste keuzes aangegeven in het groen. Doorloop nu de verschillende stappen om het injecteren uit te voeren door de juiste keuzes te selecteren met de muis.";
                    }
                    else
                    {
                        UItext.text = "Ingewikkelde handelingen die uit meerdere stappen bestaan openen het 'actie keuze menu'. De handeling start automatisch. Zodra er een belangrijke stap is aangebroken, pauzeert het spel. Er verschijnen keuzes in het beeld en het is aan jou om de juiste keuze te selecteren. Zodra je de goede keuze maakt zal de handeling verder worden uitgevoerd. Dit herhaalt zich tot de handeling is afgerond.";
                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if ( sequenceCompleted )
                    {
                        currentStep = TutorialStep.CombinePutOnInjCap;
                        player.tutorial_movementLock = movementLock = false;
                        UItext.text = "De client is geinjecteerd. Pak de beschermdop van injectienaald op door ernaar te kijken en op de linkermuisknop te drukken. Combineer nu de beschrmdop van de injectienaald met de spuit door op de 'R'toets te drukken.";

                        itemToPick = "SyringeInjectionCap";
                        particleHint.transform.position = GameObject.Find("SyringeInjectionCap").transform.position;
                        particleHint.SetActive(true);
                    }
                    break;
                case TutorialStep.CombinePutOnInjCap:
                    if (handsInventory.tutorial_combined)
                    {
                        particleHint.SetActive(false);

                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineTakeOffInjNeedle;
                        UItext.text = "Nu de dop weer op de injectienaald zit, is het veilig om de injectienaald van de spuit af te halen. Zorg ervoor dat je de spuit vast hebt en zorg voor een vrije hand. Druk op de 'R' toets om de injectie naald van de spuit af te halen.";
                    }
                    break;
                case TutorialStep.CombineTakeOffInjNeedle:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.UseInjNeedleOnTrash;
                        UItext.text = "Gooi de injectienaald in de naaldcontainer. Doe dit door te kijken naar de naadcontainer en op de actieknop van de juiste hand te drukken. Gebruik actietoets 'Q' voor de linkerhand en ";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("NeedleCup").transform.position;
                    }
                    break;
                case TutorialStep.UseInjNeedleOnTrash:
                    if (needleTrashed)
                    {
                        needleTrashed = false;
                        currentStep = TutorialStep.DropSyringe;
                        UItext.text = "Leg nu de spuit terug op het werkveld door op 'SHIFT' + 'Q' als je de spuit in je linkerhand vast hebt en op 'SHIFT' + 'E' als je de spuit in je rechterhand vast hebt.";

                        itemToDrop = "Syringe";
                        particleHint.SetActive(false);
                    }
                    break;
                case TutorialStep.DropSyringe:
                    if (handsInventory.Empty())
                    {
                        AddPointWithSound();

                        currentStep = TutorialStep.UseWorkField;
                        UItext.text = "Ruim de spullen op en maak het werkveld schoon. Dit doe je door naar het werkveld te kijken en op de 'linkermuisknop' te drukken.";

                        workField.tutorial_used = false;

                        particleHint.SetActive(true);
                        particleHint.transform.position = workField.transform.position;
                    }
                    break;
                case TutorialStep.UseWorkField:
                    if (workField.tutorial_used)
                    {
                        currentStep = TutorialStep.UseHygienePump;
                        UItext.text = "Desinfecteer atijd je handen na het uitvoeren van een medische handeling. Doe dit door te kijken naar de hygienepomp en op de 'linkermuisknop' te drukken.";

                        handCleaner.tutorial_used = false;
                        particleHint.transform.position = handCleaner.transform.position;
                    }
                    break;
                case TutorialStep.UseHygienePump:
                    if (handCleaner.tutorial_used)
                    {
                        currentStep = TutorialStep.UsePaperAndPen;
                        UItext.text = "Zoals je ziet verschijnt een pen en papier op de tafel. Deze kun je gebruiken om je bevindingen op te schrijven. Doe dit door naar het papier te kijken en druk op de 'linkermuisknop'.";

                        particleHint.transform.position = paperNPen.transform.position;
                    }
                    break;
                case TutorialStep.UsePaperAndPen:
                    if (paperNPen.tutorial_used)
                    {
                        particleHint.SetActive(false);

                        currentStep = TutorialStep.TutorialEnd;
                        UItext.text = "Goed gedaan. Dit was de uitleg over Care-Up. Succes en veel pleier bij het oefenen van de BIG-handelingen";
                    }
                    break;
                case TutorialStep.TutorialEnd:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.None;
                        actionManager.OnUseAction("__tutorialEnd");
                        GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().TutorialCompleted = true;
                        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("Menu");
                    }
                    else
                    {
                        SetPauseTimer(5.0f);
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
        actionManager.Points += 1;
        actionManager.PlayAddPointSound();
    }
}
