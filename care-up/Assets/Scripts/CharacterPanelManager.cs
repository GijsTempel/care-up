using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUpAvatar;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> characters = default(List<GameObject>);

    [SerializeField]   // commented out never used
    private GameObject //previousButton = default(GameObject),
                       //nextButton = default(GameObject),
                       buyButton = default(GameObject),
                       adjustButton = default(GameObject);

    [SerializeField]
    private Text currencyText = default(Text);

    private CharacterCreationScene characterCreation;
    private PlayerAvatarData.Gender gender;

    private static StoreManager storeManager = PlayerPrefsManager.storeManager;
    private List<CharacterItem> parameters = storeManager.CharacterItems;

    private int index = 0;
    private int storeitemIndex;
    private GameObject mainCharacter;
    private SimpleGestureController gestureController = new SimpleGestureController();

    private UMP_Manager uMP_Manager;
    private LoadCharacterScene loadCharacter;

    public void NextStep()
    {
        index -= 2;
        SetCharacters(characters);
    }

    public void PreviousStep()
    {
        index -= 4;
        SetCharacters(characters);
    }

    public void Adjust()
    {
        if (storeManager.PurchaseCharacter(storeitemIndex))
        {
            uMP_Manager.ChangeWindow(9);
            loadCharacter.LoadCharacter();
        }
    }

    private void Start()
    {
        uMP_Manager = GameObject.FindObjectOfType<UMP_Manager>();
        loadCharacter = GameObject.FindObjectOfType<LoadCharacterScene>();

        mainCharacter = characters[1];

        StartCoroutine(SetAnimation());

        currencyText.text = storeManager.Currency.ToString();

        if (characterCreation == null)
            characterCreation = GameObject.FindObjectOfType<CharacterCreationScene>();

        foreach (GameObject gameObject in characters)
        {
            gameObject.SetActive(true);
        }

        SetCharacters(characters);

        buyButton?.GetComponent<Button>().onClick.AddListener(BuyCharacter);
    }

    private void Update()
    {
        gestureController.ManageSwipeGestures(NextStep, PreviousStep);
    }

    private void SetCharacters(List<GameObject> items)
    {
        foreach (GameObject item in items)
        {
            characterCreation.Initialize(item);

            (bool purchased, int price) = SetCurrentItem(ref index);

            item.transform.parent.Find("Checkmark").gameObject.SetActive(parameters[index].purchased);

            if (item.transform.parent.name == "CenterGuy")
            {
                storeitemIndex = index;
                buyButton.transform.GetChild(0).GetComponent<Text>().text = price.ToString();

                if (parameters[index].purchased)
                    adjustButton.SetActive(true);
                else
                    adjustButton.SetActive(false);
            }

            index++;
        }
    }

    public void SetDefaultCharacters(List<GameObject> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            characterCreation.Initialize(items[i]);

            gender = parameters[i].gender == "Female" ? CharacterCreationScene.CharGender.Female : CharacterCreationScene.CharGender.Male;
            characterCreation.SetCurrent(gender, parameters[i].headType, parameters[i].bodyType, parameters[i].glassesType);
        }
    }

    private (bool purchased, int price) SetCurrentItem(ref int index)
    {
        if (index < 0)
            index = parameters.Count - 1;

        else if (index >= parameters.Count)
            index = 0;

        gender = parameters[index].gender == "Female" ? PlayerAvatarData.Gender.Female : PlayerAvatarData.Gender.Male;
        characterCreation.SetCurrent(gender, parameters[index].headType, parameters[index].bodyType, parameters[index].glassesType);

        return (parameters[index].purchased, parameters[index].price);
    }

    private void BuyCharacter()
    {
        if (storeManager.PurchaseCharacter(storeitemIndex))
        {
            mainCharacter.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            mainCharacter.GetComponent<Animator>().SetTrigger("dance1");
            adjustButton.SetActive(true);
            StartCoroutine(ResetScale());
            currencyText.text = storeManager.Currency.ToString();
        }
        else
        {
            PurchaseFail();
        }
    }

    private IEnumerator SetAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        SetAnimationTrigger(0, "idle1");

        yield return new WaitForSeconds(1);
        SetAnimationTrigger(2, "idle1");
    }

    private IEnumerator ResetScale()
    {
        yield return new WaitForSeconds(5.5f);
        mainCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void SetAnimationTrigger(int index, string name)
    {
        characters[index].GetComponent<Animator>().SetTrigger(name);
    }

    private void PurchaseFail()
    {
        SetAnimationTrigger(1, "sad1");
        StartCoroutine(PopUp());
    }

    private IEnumerator PopUp()
    {
        yield return new WaitForSeconds(2f);
        GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(8);
        SetAnimationTrigger(1, "idle1");
    }
}