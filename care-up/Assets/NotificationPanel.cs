using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NotificationPanel : MonoBehaviour
{
    public List<Button> pageButtons;
    public List<GameObject> pages;
    public GameObject newNotifContainer;
    public GameObject notifContainer;

    // Start is called before the first frame update

    public void ShowPage(int pageNum)
    {
        foreach(GameObject page in pages)
        {
            page.SetActive(false);
        }
        pages[pageNum].SetActive(true);
        foreach(Button pageButton in pageButtons)
        {
            pageButton.interactable = true;
        }
        pageButtons[pageNum].interactable = false;

    }

    void Start()
    {
        if (PlayerPrefsManager.Notifications.Count > 0)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
