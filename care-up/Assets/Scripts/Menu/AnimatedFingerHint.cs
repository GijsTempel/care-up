using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimatedFingerHint : MonoBehaviour
{
    public bool toMove = false;
    bool waveHi = true;
    //bool toWave = false;
    //bool shown = false;
    public RectTransform FingerHolder;
    public RectTransform TargetPointer;
    //GameObject targetUI_Element = null;
    public Animator fingerAnimator;
    float moveSpeed = 500f;
    Vector3 moveTarget = new Vector3();
    GameUI gameUI;
    Button ButtonToAutoClick = null;
    ActionManager actionManager;
    Camera cam;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Invoke("FindTarget", 0.3f);
    }

    void AutoClickButton()
    {
        if (ButtonToAutoClick != null)
        {
            if (ButtonToAutoClick.gameObject.activeSelf)
                ButtonToAutoClick.onClick.Invoke();
        }
    }

    void FindTarget()
    {
        if (!gameObject.activeSelf)
            return;
        RectTransform ActiveTalkBubblePoint = gameUI.GetActiveTalkBubblePoint();
        bool toTalk = false;
        if (ActiveTalkBubblePoint != null)
        {
            if (ActiveTalkBubblePoint.transform.parent.gameObject.activeSelf)
            {
                toTalk = true;
            }
        }
        if (waveHi) 
        {
            waveHi = false;
            transform.Find("FingerHolder/animFinger/show_sound").GetComponent<AudioSource>().Play();

            fingerAnimator.SetTrigger("show");
            fingerAnimator.SetTrigger("hi");
        }
        else if (toTalk)
        {
            MoveTo(ActiveTalkBubblePoint.position);
        }
        else if (gameUI.DropLeftBlink || gameUI.DropRightBlink)
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
        else if (gameUI.PointToObject != null)
        {
            MoveToGameObject(gameUI.PointToObject);
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
                {
                    if (gameUI.AllowAutoPlay(false))
                        Invoke("AutoClickButton", 1.5f);
                    ButtonToAutoClick = b.GetComponent<Button>();
                    point =  b.transform.Find("fingerPosition").GetComponent<RectTransform>();
                }

                MoveTo(point.position);
                break;
            }
        }
    }

    void Init()
    {
        cam = Camera.main.GetComponent<Camera>();
    }

    void MoveToGameObject(GameObject TargetObject)
    {
        if (cam == null)
            Init();
        GameObject _targetObject = TargetObject;
        if (TargetObject.transform.Find("hl") != null)
            _targetObject = TargetObject.transform.Find("hl").gameObject;
        Vector3 ObjWorldPos = _targetObject.transform.position;
        Vector3 ScreenPos = cam.WorldToScreenPoint(ObjWorldPos);
        if (ScreenPos.x > 0 && ScreenPos.y > 0 && ScreenPos.x < Screen.width && ScreenPos.y < Screen.height)
            MoveTo(ScreenPos);
    }

    void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        actionManager = GameObject.FindObjectOfType<ActionManager>();
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
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            return;
        Vector3 scr_center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        FingerHolder.position = scr_center;
        fingerAnimator.SetTrigger("fly");
        transform.Find("FingerHolder/animFinger/sound").GetComponent<AudioSource>().Play();
        toMove = true;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            point, canvas.worldCamera, out pos);
        moveTarget = canvas.transform.TransformPoint(pos);
        TargetPointer.position = moveTarget;
        float dist = Vector3.Distance(FingerHolder.position, moveTarget);
        moveSpeed = dist * 1f;
    }

    /*public void clearTarget()
    {
        targetUI_Element = null;
    }*/

    void Update()
    {
        if (toMove )
        {
             FingerHolder.position = Vector3.MoveTowards(FingerHolder.position, moveTarget, Time.deltaTime * moveSpeed);
        }
      
    }
}
