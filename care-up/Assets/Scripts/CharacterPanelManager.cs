using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> characters;

    [SerializeField]
    private GameObject previousButton, nextButton, buyButton, adjustButton;

    [SerializeField]
    private Text currencyText;

    private CharacterCreationScene characterCreation;
    private CharacterCreationScene.CharGender gender;

    private static StoreManager storeManager = PlayerPrefsManager.storeManager;
    private List<CharacterItem> parameters = storeManager.CharacterItems;

    private int index = 0;
    private int storeitemIndex;
    private GameObject mainCharacter;

    private void Start()
    {
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
        previousButton?.GetComponent<Button>().onClick.AddListener(PreviousStep);
        nextButton?.GetComponent<Button>().onClick.AddListener(NextStep);

        buyButton?.GetComponent<Button>().onClick.AddListener(BuyCharacter);
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

    private void NextStep()
    {
        index -= 2;
        SetCharacters(characters);
    }

    private void PreviousStep()
    {
        index -= 4;
        SetCharacters(characters);
    }

    private (bool purchased, int price) SetCurrentItem(ref int index)
    {
        if (index < 0)
            index = parameters.Count - 1;

        else if (index >= parameters.Count)
            index = 0;

        gender = parameters[index].gender == "Female" ? CharacterCreationScene.CharGender.Female : CharacterCreationScene.CharGender.Male;
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
        yield return new WaitForSeconds(4f);
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

