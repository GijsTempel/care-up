using UnityEngine;

public class RobotUITabInfo : RobotUITabs
{
    [HideInInspector]
    public bool tutorial_fullscreen = false;
    [HideInInspector]
    public bool tutorial_listButton = false;
    [HideInInspector]
    public bool tutorial_changedPDF = false;

    private RectTransform selectedButton;
    private RectTransform deselectButton;

    private bool set = false;
    private float timer = 0.0f;

    private float initButtonWidth = 0;
    private float initButtonHeight = 0;
    public GameObject ItemList;
    public GameObject ItemListButton;

    public void FullScreenPDF()
    {
        // never used?
        //GameObject FullScreenPDF_UI = 
        Instantiate(Resources.Load("PDFFullScreen/PDFFullScreen"));
        //as GameObject;

        tutorial_fullscreen = true;
    }

    protected override void Start()
    {
        base.Start();

        Transform t = transform.Find("InfoDynamicCanvas").Find("ItemList").Find("Scroll View")
            .Find("Viewport").Find("Content").GetChild(0);
        if (t != null)
        {
            OnItemButtonClick(t.GetComponent<RectTransform>());
        }

        //SwitchItemList(true);
        // generate buttons
        // done in playerspawn
    }


    public void ToggleItemList()
    {
        if (ItemListButton.GetComponent<Animator>() != null && ItemListButton.GetComponent<Animator>().isActiveAndEnabled)
        
          
        ItemListButton.GetComponent<Animator>().SetTrigger("BlinkStop");
        ItemList.SetActive(!ItemList.activeSelf);

        tutorial_listButton = true;
    }

    public void SwitchItemList(bool value)
    {
        ItemList.SetActive(value);
    }


    private void Update()
    {
        if (set)
        {
            timer += Time.deltaTime;
            float maxTime = 0.3f;

            if (selectedButton != null)
            {
                float sX = (timer / maxTime) * 45.0f;
                selectedButton.sizeDelta = new Vector2(initButtonWidth + sX, initButtonHeight);
            }

            if (deselectButton != null)
            {
                float dX = ((maxTime - timer) / maxTime) * 45.0f;
                deselectButton.sizeDelta = new Vector2(initButtonWidth + dX, initButtonHeight);
            }

            if (timer >= maxTime)
            {
                timer = 0.0f;
                set = false;
            }
        }
    }

    public void OnItemButtonClick(RectTransform caller)
    {
        if (selectedButton != caller)
        {
            if (selectedButton != null)
            {
                deselectButton = selectedButton;
                deselectButton.GetComponent<RobotUIInfoButton>().Toggle(false);
            }

            selectedButton = caller;
            selectedButton.GetComponent<RobotUIInfoButton>().Toggle(true);

            set = true;

            initButtonWidth = selectedButton.sizeDelta.x;
            initButtonHeight = selectedButton.sizeDelta.y;
        }
        ToggleItemList();

        tutorial_changedPDF = true;
    }

    protected override void SetTabActive(bool value)
    {
        base.SetTabActive(value);

        if (selectedButton != null)
        {
            selectedButton.GetComponent<RobotUIInfoButton>().Toggle(value);
        }
    }
}
