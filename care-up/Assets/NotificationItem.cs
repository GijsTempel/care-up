using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NotificationItem : MonoBehaviour
{
    public int notifID = -1;
    public GameObject star;
    public bool isRead = false;
    public GameObject titleObj;
    public GameObject messageObj;
    public GameObject dateObj;
    public GameObject authorObj;


    public void LoadData(int _id)
    {
        PlayerPrefsManager.CANotifications notif = PlayerPrefsManager.GetNotificationByID(_id);
        notifID = _id;
        titleObj.GetComponent<Text>().text = notif.title;
        messageObj.GetComponent<Text>().text = notif.message;
        authorObj.GetComponent<Text>().text = notif.author;
        star.SetActive(notif.isRead);
    }

    public void Start()
    {
        if (isRead)
        {
            star.SetActive(false);
        }
    }
    public void Clicked()
    {
        Debug.Log("AAAAAAAAAAAAAAA");
        MarkAsRead();
    }

    public void MarkAsRead()
    {
        isRead = true;
        star.SetActive(false);
    }

}
