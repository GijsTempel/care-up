using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemStoreGroup : MonoBehaviour
{
    public List<Button> TabButtons;
    public List<GameObject> TabContainers;
    private PlayerAvatar mainAvatar;
    private Text buyBtnText = default;
    private GameObject buyBtn = default,
                        buyBtnCoin = default,
                        productItem = default;

    private Sprite buyBtnSprite = default;
    public void SwitchTab(int value)
    {
        foreach(Button b in TabButtons)
        {
            b.interactable = true;
        }
        TabButtons[value].interactable = false;
        foreach(GameObject g in TabContainers)
        {
            g.SetActive(false);
        }
        TabContainers[value].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchTab(0);
    }


    private void InitializeElements()
    {
        mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();
        buyBtnText = buyBtn.transform.Find("Text").GetComponent<Text>();
        buyBtnCoin = buyBtn.transform.Find("Coin").gameObject;
        buyBtnSprite = buyBtn.GetComponent<Image>().sprite;

        productItem = Resources.Load<GameObject>("NecessaryPrefabs/ProductPanel");

    }
}
