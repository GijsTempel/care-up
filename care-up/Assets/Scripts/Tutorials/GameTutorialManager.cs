using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Sprite TutorialImage { get; set; }
}

public class GameTutorialManager : MonoBehaviour
{
    public GameObject magnifier;
    public GameObject magnifImage;

    private Text title;
    private Text description;
    private Image image;
    private GameObject previousButton;
    private GameObject nextButton;
    private int index = 0;

    private Sprite[] tutorialImages;
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

    private void MagnifierEffect()
    {
        Vector3 mPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mPos.z = 0;
        //magnifier.GetComponent<RectTransform>().localPosition = (mPos - new Vector3(0.5f,0.5f,0))* 1000;
        mPos.y -= 1;
        magnifier.GetComponent<RectTransform>().anchoredPosition = mPos * 1000;

        mPos.x *= -1;
        mPos.y *= -1;
        Vector3 _mPos = magnifier.GetComponent<RectTransform>().anchoredPosition;
        _mPos.x = (-_mPos.x) * 4;
        _mPos.y = (1 - (_mPos.y * 4)) - 1892;
        magnifImage.GetComponent<RectTransform>().anchoredPosition = _mPos;
    }

    private void Update()
    {
       ManageSwipeGestures();
        //MagnifierEffect();
    }

    private void Initialize()
    {
        print("Initialize");
        title = GameObject.Find("TutorialCanvas/Canvas/Background/Title/Text").GetComponent<Text>();
        description = GameObject.Find("TutorialCanvas/Canvas/Background/Description/Text").GetComponent<Text>();
        image = GameObject.Find("TutorialCanvas/Canvas/Background/MiddleGroup/MiddleGroupHolder/StepImageContainer/Image/Mask/Image").GetComponent<Image>();

        previousButton = GameObject.Find("TutorialCanvas/Canvas/Background/MiddleGroup/MiddleGroupHolder/PreviousStep/Button");
        nextButton = GameObject.Find("TutorialCanvas/Canvas/Background/MiddleGroup/MiddleGroupHolder/NextStep/Button");

        previousButton.GetComponent<Button>().onClick.AddListener(PreviousStep);
        nextButton.GetComponent<Button>().onClick.AddListener(NextStep);
    }

    private void LoadInfo()
    {
        tutorialSteps = new List<TutorialStep>();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/GameTutorial");
        XmlDocument xmlFile = new XmlDocument();

        xmlFile.LoadXml(textAsset.text);

        XmlNodeList steps = xmlFile.FirstChild.NextSibling.ChildNodes;

        tutorialImages = Resources.LoadAll<Sprite>("Sprites/TutorialImages");

        int i = 0;

        foreach (XmlNode step in steps)
        {
            if (i < tutorialImages.Length)
            {
                tutorialSteps.Add(new TutorialStep
                {
                    Title = step.Attributes["title"].Value,
                    Description = step.Attributes["description"].Value,
                    TutorialImage = tutorialImages[i]
                });

                i++;
            }
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
        image.sprite = tutorialSteps[index].TutorialImage;
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
}
