using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimatedFingerHint : MonoBehaviour
{
    public bool toMove = false;
    bool waveHi = true;
    bool toWave = false;
    bool shown = false;
    public RectTransform FingerHolder;
    public RectTransform TargetPointer;
    GameObject targetUI_Element = null;
    public Animator fingerAnimator;
    float moveSpeed = 500f;
    Vector3 moveTarget = new Vector3();
    GameUI gameUI;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Invoke("FindTarget", 0.3f);
    }


    void FindTarget()
    {
        if (!gameObject.activeSelf)
            return;
        if (waveHi)
        {
            waveHi = false;
            fingerAnimator.SetTrigger("show");
            fingerAnimator.SetTrigger("hi");
        }
        if (gameUI.DropLeftBlink || gameUI.DropRightBlink)
        {
            GameUI.ItemControlButtonType bType = GameUI.ItemControlButtonType.DropLeft;
            if (gameUI.DropRightBlink)
                bType = GameUI.ItemControlButtonType.DropRight;

            MoveToControlButton(bType);
        }
        else if (!gameUI.LevelEnded && (gameUI.recordsButtonBlink || gameUI.prescriptionButtonBlink || gameUI.paperAndPenButtonblink))
        {
            GameUI.ItemControlButtonType bType = GameUI.ItemControlButtonType.Records;
            if (gameUI.prescriptionButtonBlink)
                bType = GameUI.ItemControlButtonType.Prescription;
            else if (gameUI.paperAndPenButtonblink)
                bType = GameUI.ItemControlButtonType.PaperAndPen;
            MoveToControlButton(bType);
        }
        else if (gameUI.moveButtonToBlink != GameUI.ItemControlButtonType.None)
        {
            GameUI.ItemControlButtonType bType = GameUI.ItemControlButtonType.MoveLeft;
            if (gameUI.moveButtonToBlink == GameUI.ItemControlButtonType.MoveRight)
                bType = GameUI.ItemControlButtonType.MoveRight;
            MoveToControlButton(bType);
        }
        else if (gameUI.buttonToBlink == GameUI.ItemControlButtonType.ZoomLeft || gameUI.buttonToBlink == GameUI.ItemControlButtonType.ZoomRight)
        {
            MoveToControlButton(gameUI.buttonToBlink);
        }
        else if (gameUI.buttonToBlink == GameUI.ItemControlButtonType.DecombineLeft
            || gameUI.buttonToBlink == GameUI.ItemControlButtonType.DecombineRight
            || gameUI.buttonToBlink == GameUI.ItemControlButtonType.Combine
            || gameUI.buttonToBlink == GameUI.ItemControlButtonType.NoTargetLeft
            || gameUI.buttonToBlink == GameUI.ItemControlButtonType.NoTargetRight)
             
        {
            MoveToControlButton(gameUI.buttonToBlink);
        }
    }

    void MoveToControlButton(GameUI.ItemControlButtonType buttonType)
    {
        RectTransform point = null;
        foreach (ItemControlButton b in GameObject.FindObjectsOfType<ItemControlButton>())
        {
            if (b.buttonType == buttonType)
            {
                point = b.GetComponent<RectTransform>();
                if (b.transform.Find("fingerPosition"))
                    point =  b.transform.Find("fingerPosition").GetComponent<RectTransform>();

                MoveTo(point.position);
                break;
            }
        }
    }

    void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        moveTarget = FingerHolder.position;
    }
    void ShowHand(bool toShow, bool showEffect = false, bool _toWave = false)
    {
        if (toShow)
        {
            if (!FingerHolder.gameObject.activeSelf)
            {
                FingerHolder.gameObject.SetActive(true);
                toMove = true;
            }
            if (showEffect)
            {
                toMove = false;
                fingerAnimator.SetTrigger("show");
                if (_toWave)
                    fingerAnimator.SetTrigger("hi");
            }
        }
        else
        {
            FingerHolder.gameObject.SetActive(false);
            toMove = false;
        }
    }

    public void MoveTo(Vector3 point)
    {
        Vector3 scr_center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        FingerHolder.position = scr_center;
        fingerAnimator.SetTrigger("fly");
        toMove = true;
        Vector2 pos;
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            point, canvas.worldCamera, out pos);
        moveTarget = canvas.transform.TransformPoint(pos);
        TargetPointer.position = moveTarget;
        float dist = Vector3.Distance(FingerHolder.position, moveTarget);
        moveSpeed = dist * 1f;
    }

    public void clearTarget()
    {
        targetUI_Element = null;
    }

    public void startMove()
    {

    }

    void Update()
    {
        if (toMove )
        {
             FingerHolder.position = Vector3.MoveTowards(FingerHolder.position, moveTarget, Time.deltaTime * moveSpeed);
        }
        if (Input.GetMouseButtonDown(0) && false)
        {
            fingerAnimator.SetTrigger("fly");
            toMove = true;
            Vector2 pos;
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, 
                Input.mousePosition, canvas.worldCamera, out pos);
            moveTarget = canvas.transform.TransformPoint(pos);
            TargetPointer.position = moveTarget;
            float dist = Vector3.Distance(FingerHolder.position, moveTarget);
            print(Input.mousePosition);
            moveSpeed = dist * 1f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            ShowHand(!FingerHolder.gameObject.activeSelf, true, toWave);
            Debug.Log("Pressed secondary button.");
        }

        if (Input.GetMouseButtonDown(2))
        {
            fingerAnimator.SetTrigger("hi");

            //Debug.Log("Pressed middle click.");
            //toWave = !toWave;
            //print(toWave);
        }
    }
}
