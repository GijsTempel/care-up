﻿using System.Collections;
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
                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.LookAround:
                    if ( player.tutorial_totalLookAround > 30.0f )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.WalkAround;
                        player.tutorial_movementLock = movementLock = false;
                        UItext.text = "Gebruik de W, A, S, D toesten om te lopen ";
                    }
                    break;
                case TutorialStep.WalkAround:
                    if ( player.tutorial_totalMoveAround > 50.0f )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.WalkToTable;
                        particleHint.transform.position = tableTrigger.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Beweeg naar de tafel door W, A, S, D en de muis te gebruiken";
                    }
                    break;
                case TutorialStep.WalkToTable:
                    if ( Vector3.Distance(tableTrigger.position, player.transform.position) < 1.0f)
                    {
                        AddPointWithSound();
                        tableTrigger.gameObject.SetActive(false);
                        player.tutorial_movementLock = movementLock = true;
                        currentStep = TutorialStep.UseHandCleaner;
                        controls.keyPreferences.closeObjectView.locked = closeObjectViewLocked = false;
                        particleHint.transform.position = handCleaner.transform.position;
                        UItext.text = "Use Hand Cleaner";
                    }
                    break;
                case TutorialStep.UseHandCleaner:
                    if (handCleaner.tutorial_used)
                    {
                        currentStep = TutorialStep.UseTable;
                        particleHint.transform.position = workField.transform.position;
                        UItext.text = "Use WorkField";
                    }
                    break;
                case TutorialStep.UseTable:
                    if (workField.tutorial_used)
                    {
                        currentStep = TutorialStep.ExamineRecords;
                        particleHint.transform.position = patientRecords.transform.position;
                        UItext.text = "Examine records";
                        patientRecords.tutorial_picked = false;
                    }
                    break;
                case TutorialStep.ExamineRecords:
                    if ( patientRecords.tutorial_picked )
                    {
                        currentStep = TutorialStep.CloseRecords;
                        particleHint.SetActive(false);
                        UItext.text = "Close examine";
                    }
                    break;
                case TutorialStep.CloseRecords:
                    if ( patientRecords.tutorial_closed )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.ExamineMedicine;
                        particleHint.transform.position = medicine.transform.position;
                        particleHint.SetActive(true);
                        UItext.text = "Examine Medicine";
                    }
                    break;
                case TutorialStep.ExamineMedicine:
                    if (medicine.tutorial_picked)
                    {
                        currentStep = TutorialStep.PickAlcohol;
                        particleHint.transform.position = alcohol.transform.position;
                        UItext.text = "Pick Alcohol";
                        itemToPick = "Alcohol";
                    }
                    break;
                case TutorialStep.PickAlcohol:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickCloth;
                        particleHint.transform.position = cloth.transform.position;
                        UItext.text = "Pick cloth";
                        itemToPick = "Cloth";
                    }
                    else
                    {
                        if (handsInventory.tutorial_pickedLeft)
                        {
                            handsInventory.tutorial_pickedLeft = false;
                            AddPointWithSound();
                            UItext.text = "Wanneer je handen leeg zijn en je pakt iets op. Zal het altijd in je linkerhand verschijnen.";
                            SetPauseTimer(5.0f);
                        }
                    }
                    break;
                case TutorialStep.PickCloth:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.CombineAlcoholCloth;
                        controls.keyPreferences.CombineKey.locked = combineKeyLocked = false;
                        UItext.text = "Combine alcohol and cloth";
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
                        UItext.text = "Dropping expl";
                    }
                    break;
                case TutorialStep.DroppingExplanation:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.DropItem;
                        controls.keyPreferences.LeftDropKey.locked = leftDropKeyLocked = false;
                        UItext.text = "Drop item Q";
                        itemToDrop = "Alcohol";
                    }
                    break;
                case TutorialStep.DropItem:
                    if ( handsInventory.tutorial_droppedLeft )
                    {
                        handsInventory.tutorial_droppedLeft = false;
                        currentStep = TutorialStep.DropAnotherItem;
                        controls.keyPreferences.RightDropKey.locked = rightDropKeyLocked = false;
                        UItext.text = "Drop another E";
                        itemToDrop = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.DropAnotherItem:
                    if ( handsInventory.tutorial_droppedRight )
                    {
                        AddPointWithSound();
                        currentStep = TutorialStep.PickBothItems;
                        handsInventory.tutorial_droppedRight = false;
                        particleHint.SetActive(true);
                        UItext.text = "Pick both back";

                        itemToPick = "Alcohol";
                        itemToPick2 = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.PickBothItems:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight )
                    {
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.WalkAway;
                        player.tutorial_movementLock = movementLock = false;
                        particleHint.SetActive(false);
                        UItext.text = "Walk away";
                    }
                    break;
                case TutorialStep.WalkAway:
                    if ( Vector3.Distance(player.transform.position, tableTrigger.position) > 5.0f )
                    {
                        currentStep = TutorialStep.DropBothItems;
                        UItext.text = "Now drop both";
                        itemToDrop = "Alcohol";
                        itemToDrop2 = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.DropBothItems:
                    if (TimerElapsed())
                    {
                        currentStep = TutorialStep.PickBothItemsAgain;
                        UItext.text = "Pick em";
                        particleHint.transform.position = syringe.transform.position;
                        particleHint_alt.transform.position = needle.transform.position;
                        particleHint.SetActive(true);
                        particleHint_alt.SetActive(true);

                        itemToPick = "Alcohol";
                        itemToPick2 = "DesinfectionCloth";
                    }
                    else
                    {
                        if ( handsInventory.tutorial_droppedLeft && handsInventory.tutorial_droppedRight)
                        {
                            handsInventory.tutorial_droppedLeft =
                                handsInventory.tutorial_droppedRight = false;
                            UItext.text = "blabla pause";
                            SetPauseTimer(5.0f);
                        }
                    }
                    break;
                case TutorialStep.PickBothItemsAgain:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.CombineDesinfMedicine;
                        controls.keyPreferences.CombineKey.locked = combineKeyLocked = false;
                        controls.keyPreferences.pickObjectView.locked = pickObjectViewKeyLocked = false;
                        particleHint.SetActive(false);
                        particleHint_alt.SetActive(false);
                        UItext.text = "Combine desinfected cloth and medicine";

                    }
                    break;
                case TutorialStep.CombineDesinfMedicine:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.DropClothMedicine;
                        UItext.text = "Drop cloth and medicine on table";

                        itemToDrop2 = "Medicine";
                        itemToDrop = "DesinfectionCloth";
                    }
                    break;
                case TutorialStep.DropClothMedicine:
                    if ( handsInventory.tutorial_droppedLeft && handsInventory.tutorial_droppedRight)
                    {
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.PickSyringeAbNeedleCap;
                        UItext.text = "Pick up syringe and capped absorption needle";

                        itemToPick = "Syringe";
                        itemToPick2 = "AbsorptionNeedle";
                    }
                    break;
                case TutorialStep.PickSyringeAbNeedleCap:
                    if ( handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.CombineSyringeAbNeedleCap;
                        UItext.text = "Combine syringe and capped absorption needle";
                    }
                    break;
                case TutorialStep.CombineSyringeAbNeedleCap:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineTakeOffAbCap;
                        UItext.text = "Decombine syringe with capped needle";
                    }
                    break;
                case TutorialStep.CombineTakeOffAbCap:
                    if ( handsInventory.tutorial_combined )
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.DropAbsorptionCap;
                        UItext.text = "Drop absorption needle cap";

                        itemToDrop = "SyringeAbsorptionCap";
                    }
                    break;
                case TutorialStep.DropAbsorptionCap:
                    if (handsInventory.tutorial_droppedLeft || handsInventory.tutorial_droppedRight)
                    {
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.CombineSyringeAbNeedleMed;
                        UItext.text = "Combine syringe with needle and medicine";

                        itemToPick = "Medicine";
                    }
                    break;
                case TutorialStep.CombineSyringeAbNeedleMed:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.DropMedicine;
                        UItext.text = "Drop medicine";

                        itemToDrop = "Medicine";

                        controls.keyPreferences.LeftUseKey.locked =
                            controls.keyPreferences.RightUseKey.locked =
                            leftUseKeyLocked = rightUseKeyLocked = false;

                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                    }
                    break;
                case TutorialStep.DropMedicine:
                    if (handsInventory.tutorial_droppedLeft || handsInventory.tutorial_droppedRight)
                    {
                        handsInventory.tutorial_droppedLeft =
                            handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.UseSyringeWithMed;
                        UItext.text = "Vent the syringe by pressing Q/E";

                        syringe = handsInventory.LeftHandEmpty() ?
                            handsInventory.RightHandObject : handsInventory.LeftHandObject;
                    }
                    break;
                case TutorialStep.UseSyringeWithMed:
                    if (syringe.GetComponent<PickableObject>().tutorial_usedOn)
                    {
                        syringe.GetComponent<PickableObject>().tutorial_usedOn = false;
                        currentStep = TutorialStep.PickUpAbsorptionCap;
                        UItext.text = "Pick up absorption needle cap";

                        itemToPick = "SyringeAbsorptionCap";
                    }
                    break;
                case TutorialStep.PickUpAbsorptionCap:
                    if (handsInventory.tutorial_pickedLeft || handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.CombinePutOnAbCap;
                        UItext.text = "Combine syringe and cap to put back on cap";
                    }
                    break;
                case TutorialStep.CombinePutOnAbCap:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineTakeOffAbNeedle;
                        UItext.text = "Decombine syringe with needle to take off capped needle";
                    }
                    break;
                case TutorialStep.CombineTakeOffAbNeedle:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.UseAbNeedleOnTrash;
                        UItext.text = "Use capped needle on trashcan";
                    }
                    break;
                case TutorialStep.UseAbNeedleOnTrash:
                    if (needleTrashed)
                    {
                        needleTrashed = false;
                        currentStep = TutorialStep.PickInjectionNeedle;
                        UItext.text = "Pick up capped injection needle";

                        itemToPick = "InjectionNeedle";
                    }
                    break;
                case TutorialStep.PickInjectionNeedle:
                    if (handsInventory.tutorial_pickedLeft || handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedLeft =
                            handsInventory.tutorial_pickedRight = false;
                        currentStep = TutorialStep.CombineSyringeInjNeedle;
                        UItext.text = "Combine Syringe+Injection Needle";
                    }
                    break;
                case TutorialStep.CombineSyringeInjNeedle:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.MoveToPatient;
                        UItext.text = "Move to the patient";
                        patientTrigger.gameObject.SetActive(true);
                    }
                    break;
                case TutorialStep.MoveToPatient:
                    if ( Vector3.Distance(player.transform.position, patientTrigger.position) < 1.0f)
                    {
                        patientTrigger.gameObject.SetActive(false);
                        player.tutorial_movementLock = movementLock = true;
                        currentStep = TutorialStep.TalkToPatient;
                        controls.keyPreferences.LeftUseKey.locked = leftUseKeyLocked = false;
                        controls.keyPreferences.RightUseKey.locked = rightUseKeyLocked = false;
                        particleHint.transform.position = GameObject.Find("Patient").transform.position;
                        UItext.text = "Talk to patient";
                    }
                    break;
                case TutorialStep.TalkToPatient:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;
                        currentStep = TutorialStep.UseOnHand;
                        UItext.text = "Use syringe on patient";

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
                        UItext.text = "do the sequence";
                    }
                    else
                    {
                        UItext.text = "explaining";
                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if ( sequenceCompleted )
                    {
                        currentStep = TutorialStep.CombinePutOnInjCap;
                        player.tutorial_movementLock = movementLock = false;
                        UItext.text = "Put injection needle cap on";

                        itemToPick = "SyringeInjectionCap";
                    }
                    break;
                case TutorialStep.CombinePutOnInjCap:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.CombineTakeOffInjNeedle;
                        UItext.text = "Take off injection needle";
                    }
                    break;
                case TutorialStep.CombineTakeOffInjNeedle:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.UseInjNeedleOnTrash;
                        UItext.text = "Trash capped injection needle";
                    }
                    break;
                case TutorialStep.UseInjNeedleOnTrash:
                    if (needleTrashed)
                    {
                        needleTrashed = false;
                        currentStep = TutorialStep.DropSyringe;
                        UItext.text = "Drop syringe";

                        itemToDrop = "Syringe";
                    }
                    break;
                case TutorialStep.DropSyringe:
                    if (handsInventory.Empty())
                    {
                        currentStep = TutorialStep.UseWorkField;
                        UItext.text = "Use work field";

                        workField.tutorial_used = false;
                    }
                    break;
                case TutorialStep.UseWorkField:
                    if (workField.tutorial_used)
                    {
                        currentStep = TutorialStep.UseHygienePump;
                        UItext.text = "Use Hygiene Pump";

                        handCleaner.tutorial_used = false;
                    }
                    break;
                case TutorialStep.UseHygienePump:
                    if (handCleaner.tutorial_used)
                    {
                        currentStep = TutorialStep.UsePaperAndPen;
                        UItext.text = "Use paper and pen";
                    }
                    break;
                case TutorialStep.UsePaperAndPen:
                    if (paperNPen.tutorial_used)
                    {
                        currentStep = TutorialStep.TutorialEnd;
                        UItext.text = "Goed gedaan. Dit was de uitleg over Care-Up. Succes bij je eerste BIG-handeling";
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
