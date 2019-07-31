using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Sprite TutorialImage { get; set; }
    public float MagnifierX { get; set; }
    public float MagnifierY { get; set; }
}

public class GameTutorialManager : MonoBehaviour
{
    public GameObject magnifier;
    public Image tutImage;
    public GameObject magnifierImage;

    public Vector2 magnifierPos;

    private Text title;
    private Text description;
    private GameObject previousButton;
    private GameObject nextButton;
    private int index = 0;

    private List<TutorialStep> tutorialSteps;

    private Vector2 firstPressPosition;
    private Vector2 secondPressPosition;
    private Vector2 currentSwipe;

    private void Awake()
    {
        LoadInfo();
    }

    private void Start()
    {
        Initialize();
    }  

    private void Update()
    {
        ManageSwipeGestures();
        Rect m_rect = magnifier.GetComponent<RectTransform>().rect;
        Rect i_rect = tutImage.GetComponent<RectTransform>().rect;
        Rect mi_rect = magnifierImage.GetComponent<RectTransform>().rect;

        float _scale = ((i_rect.height/2)/300f);
        magnifier.GetComponent<RectTransform>().localScale = new Vector3(_scale,_scale,1f);
        magnifier.GetComponent<RectTransform>().anchoredPosition = magnifierPos * new Vector2(i_rect.width, -i_rect.height);
        magnifierImage.GetComponent<RectTransform>().anchoredPosition = 
        (magnifierPos - new Vector2(0.5f,0.5f)) * new Vector2(-mi_rect.width, mi_rect.height);
    }

    private void Initialize()
    {
        title = GameObject.Find("TutorialCanvas/Canvas/Background/Title/Text").GetComponent<Text>();
        description = GameObject.Find("TutorialCanvas/Canvas/Background/Description/Text").GetComponent<Text>();

        previousButton = GameObject.Find("TutorialCanvas/Canvas/Background/MiddleGroup/MiddleGroupHolder/PreviousStep/Button");
        nextButton = GameObject.Find("TutorialCanvas/Canvas/Background/MiddleGroup/MiddleGroupHolder/NextStep/Button");

        previousButton.GetComponent<Button>().onClick.AddListener(PreviousStep);
        nextButton.GetComponent<Button>().onClick.AddListener(NextStep);

        UpdateTutorialStep();
    }

    private void LoadInfo()
    {
        tutorialSteps = new List<TutorialStep>();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/GameTutorial");
        XmlDocument xmlFile = new XmlDocument();

        xmlFile.LoadXml(textAsset.text);

        XmlNodeList steps = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode step in steps)
        {
            float _x = 0f;
            float _y = 0f;
            if (step.Attributes["magnifier_x"] != null)
                float.TryParse(step.Attributes["magnifier_x"].Value, out _x);

            if (step.Attributes["magnifier_y"] != null)
                float.TryParse(step.Attributes["magnifier_y"].Value, out _y);
            
            tutorialSteps.Add(new TutorialStep
            {
                Title = step.Attributes["title"].Value,
                Description = step.Attributes["description"].Value,
                TutorialImage = Resources.Load<Sprite>($"Sprites/TutorialImages/{step.Attributes["image"].Value}"),
                MagnifierX = _x,
                MagnifierY = _y
            });
        }
    }

    private void NextStep()
    {
        ++index;

        previousButton.SetActive(true);

        if (index == tutorialSteps.Count - 1)
        {
            nextButton.SetActive(false);
        }

        UpdateTutorialStep();
    }

    private void PreviousStep()
    {
        --index;

        nextButton.SetActive(true);

        if (index == 0)
        {
            previousButton.SetActive(false);
        }

        UpdateTutorialStep();
    }

    private void UpdateTutorialStep()
    {
        title.text = tutorialSteps[index].Title;
        description.text = tutorialSteps[index].Description;
        tutImage.sprite = tutorialSteps[index].TutorialImage;
        magnifierImage.GetComponent<Image>().sprite = tutorialSteps[index].TutorialImage;
        magnifierPos = new Vector2(tutorialSteps[index].MagnifierX, tutorialSteps[index].MagnifierY);

    }

    private void ManageSwipeGestures()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            currentSwipe = new Vector2(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);
            currentSwipe.Normalize();

            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (nextButton.activeInHierarchy)
                    NextStep();
            }
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (previousButton.activeInHierarchy)
                    PreviousStep();
            }
        }
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                firstPressPosition = new Vector2(touch.position.x, touch.position.y);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                secondPressPosition = new Vector2(touch.position.x, touch.position.y);
                currentSwipe = new Vector3(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);
                currentSwipe.Normalize();

                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (nextButton.activeInHierarchy)
                        NextStep();
                }
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (previousButton.activeInHierarchy)
                        PreviousStep();
                }
            }
        }
    }

    private void MagnifierEffect()
    {
        // Vector3 mPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        // mPos.z = 0;
        // //magnifier.GetComponent<RectTransform>().localPosition = (mPos - new Vector3(0.5f,0.5f,0))* 1000;
        // mPos.y -= 1;
        // magnifier.GetComponent<RectTransform>().anchoredPosition = mPos * 1000;

        // mPos.x *= -1;
        // mPos.y *= -1;
        // Vector3 _mPos = magnifier.GetComponent<RectTransform>().anchoredPosition;
        // _mPos.x = (-_mPos.x) * 4;
        // _mPos.y = (1 - (_mPos.y * 4)) - 1892;
        // magnifImage.GetComponent<RectTransform>().anchoredPosition = _mPos;
    }
}
