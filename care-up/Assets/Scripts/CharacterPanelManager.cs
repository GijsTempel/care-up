﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buyButton = default(GameObject),
                       adjustButton = default(GameObject);

    [SerializeField]
    private Text currencyText = default(Text);
    private List<GameObject> checkMarks = new List<GameObject>();

    private static StoreManager storeManager = PlayerPrefsManager.storeManager;
    private SimpleGestureController gestureController = new SimpleGestureController();
    private UMP_Manager uMP_Manager;
    private LoadCharacterScene loadCharacter;
    private CharacterСarrousel сarrousel;

    public List<GameObject> platforms;

    public void Adjust()
    {
        uMP_Manager.ChangeWindow(9);
        loadCharacter.LoadCharacter();
    }

    public void Scroll(int dir)
    {
        int nextChar = CharacterСarrousel.CurrentCharacter + dir;
        if (nextChar >= 0 && nextChar < storeManager.CharacterItems.Count)
            CharacterСarrousel.nextTurnDir = dir;
        enabled = true;
    }

    public void SetStoreInfo(int platformIndex, int characterIndex)
    {
        checkMarks[platformIndex].SetActive(storeManager.CharacterItems[characterIndex].purchased);

        buyButton.transform.GetChild(0).GetComponent<Text>().text = storeManager.CharacterItems[characterIndex].price.ToString();

        if (storeManager.CharacterItems[characterIndex].purchased)
            adjustButton.SetActive(true);
        else
            adjustButton.SetActive(false);
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        gestureController.ManageSwipeGestures(() => Scroll(1), () => Scroll(-1));
    }

    private void Initialize()
    {
        uMP_Manager = GameObject.FindObjectOfType<UMP_Manager>();
        loadCharacter = GameObject.FindObjectOfType<LoadCharacterScene>();

        сarrousel = GameObject.FindObjectOfType<CharacterСarrousel>();

        currencyText.text = storeManager.Currency.ToString();
        buyButton?.GetComponent<Button>().onClick.AddListener(BuyCharacter);

        foreach (GameObject platform in platforms)
        {
            checkMarks.Add(platform.transform.Find("checkMark").gameObject);
        }
    }

    private void BuyCharacter()
    {
        if (storeManager.PurchaseCharacter(CharacterСarrousel.CurrentCharacter))
        {
            adjustButton.SetActive(true);
            currencyText.text = storeManager.Currency.ToString();
        }
        else
        {
            PurchaseFail();
        }
    }

    private void PurchaseFail()
    {
        uMP_Manager.ShowDialog(8);
    }

    //private void SetCharacters(List<GameObject> items)
    //{
    //    foreach (GameObject item in items)
    //    {
    //        characterCreation.Initialize(item);

    //        (bool purchased, int price) = SetCurrentItem(ref index);

    //        item.transform.parent.Find("Checkmark").gameObject.SetActive(parameters[index].purchased);

    //        if (item.transform.parent.name == "CenterGuy")
    //        {
    //            storeitemIndex = index;
    //            // buyButton.transform.GetChild(0).GetComponent<Text>().text = price.ToString();

    //            if (parameters[index].purchased)
    //                adjustButton.SetActive(true);
    //            else
    //                adjustButton.SetActive(false);
    //        }

    //        index++;
    //    }
    //}

    //public void SetDefaultCharacters(List<GameObject> items)
    //{
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        characterCreation.Initialize(items[i]);

    //        gender = parameters[i].gender == "Female" ? PlayerAvatarData.Gender.Female : PlayerAvatarData.Gender.Male;
    //        characterCreation.SetCurrent(gender, parameters[i].headType, parameters[i].bodyType, parameters[i].glassesType);
    //    }
    //}

    //private (bool purchased, int price) SetCurrentItem(ref int index)
    //{
    //    if (index < 0)
    //        index = parameters.Count - 1;

    //    else if (index >= parameters.Count)
    //        index = 0;

    //    gender = parameters[index].gender == "Female" ? PlayerAvatarData.Gender.Female : PlayerAvatarData.Gender.Male;
    //    characterCreation.SetCurrent(gender, parameters[index].headType, parameters[index].bodyType, parameters[index].glassesType);

    //    return (parameters[index].purchased, parameters[index].price);
    //}

    //private IEnumerator SetAnimation()
    //{
    //    yield return new WaitForSeconds(0.6f);
    //    SetAnimationTrigger(0, "idle1");

    //    yield return new WaitForSeconds(1);
    //    SetAnimationTrigger(2, "idle1");
    //}

    //private IEnumerator ResetScale()
    //{
    //    yield return new WaitForSeconds(5.5f);
    //    mainCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
    //}

    //private void SetAnimationTrigger(int index, string name)
    //{
    //    characters[index].GetComponent<Animator>().SetTrigger(name);
    //}
}