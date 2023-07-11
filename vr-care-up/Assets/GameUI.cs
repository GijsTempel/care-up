using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{

    public enum ItemControlButtonType
    {
        None,
        Combine,
        DecombineLeft,
        DecombineRight,
        NoTargetLeft,
        NoTargetRight,
        ZoomLeft,
        ZoomRight,
        DropLeft,
        DropRight,
        MoveLeft,
        MoveRight,
        Records,
        Prescription,
        Ipad,
        General,
        PaperAndPen,
        GeneralBack,
        RecordsBack,
        PrescriptionBack,
        MessageTabBack,
        Close,
        TalkBubble,
    }

    public enum MoveControlButtonType
    {
        None,
        MoveA,
        MoveB,
        MoveC,
        MoveD
    }

    public GameUI.ItemControlButtonType buttonToBlink;
    public GameUI.MoveControlButtonType moveButtonToBlink;
    public bool prescriptionButtonBlink;
    public bool recordsButtonBlink;
    public bool paperAndPenButtonblink;
    public bool dropLeftBlink = false;
    public bool dropRightBlink = false;
    public List<string> reqPlaces = new List<string>();

    public void UpdateButtonsBlink()
    {
        //+++++++++++++++++++++++++++++++++++++++++
    }


    public void UpdateHintPanel(List<ActionManager.StepData> subTasks, float UpdateHintDelay = 0f)
    {
        //+++++++++++++++++++++++++++++++++++++++++
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
