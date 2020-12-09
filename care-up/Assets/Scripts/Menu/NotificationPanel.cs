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

    private static int Compare(int x, int y)
    {
        if (x == y)
            return 0;
        else if (x > y)
            return -1;
        else
            return 1;
    }

    private void OnEnable()
    {
        RebuildPanel();
        if (PlayerPrefsManager.HasNewNorifications())
            ShowPage(0);
        else
            ShowPage(1);
    }

    public void UpdatePanel()
    {
        RebuildPanel(false, true);
    }

    void RebuildPanel(bool newItems = true, bool oldItems = true)
    {
        if (newItems)
        {
            foreach (Transform child in newNotifContainer.transform)
                GameObject.Destroy(child.gameObject);
            
        }

        if (oldItems)
        {
            foreach (Transform child in notifContainer.transform)
            GameObject.Destroy(child.gameObject);
        }

        if (PlayerPrefsManager.Notifications.Count > 0)
        {
            List<int> notifIDs = new List<int>();
            foreach (int _id in PlayerPrefsManager.Notifications.Keys)
                notifIDs.Add(_id);

            notifIDs.Sort(Compare);

            foreach (int _id in notifIDs)
            {
                if (!PlayerPrefsManager.Notifications[_id].isRead)
                {
                    if (newItems)
                    {
                        GameObject newNotifInst = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/NotificationItem"), newNotifContainer.transform);
                        newNotifInst.GetComponent<NotificationItem>().LoadData(_id);
                    }
                }
                else
                {
                    if (oldItems)
                    {
                        GameObject notifInst = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/NotificationItem"), notifContainer.transform);
                        notifInst.GetComponent<NotificationItem>().LoadData(_id);
                    }
                }
            }
        }

    }

}
