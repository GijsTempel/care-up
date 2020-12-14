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
        string messageToShow = notif.message;
        int lines = notif.message.Split('\n').Length;
        if (notif.message.Length > 120 || lines > 2)
        {
            messageToShow = notif.message.Substring(0, 120) + "...";
        }
        messageObj.GetComponent<Text>().text = messageToShow;
        authorObj.GetComponent<Text>().text = notif.author;
        star.SetActive(!notif.isRead);

        string sDate = notif.GetCreatedTimeString();
        dateObj.GetComponent<Text>().text = sDate;
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
        MarkAsRead();
    }

    public void MarkAsRead()
    {
        if (!isRead)
        {
            isRead = true;
            star.SetActive(false);
            PlayerPrefsManager.Notifications[notifID].isRead = true;
            GameObject.FindObjectOfType<NotificationPanel>().UpdatePanel();
            long unixTimestamp = (long)(System.DateTime.Now.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
            //Insert update to database
            DatabaseManager.PushCANotification(notifID, PlayerPrefsManager.Notifications[notifID]);
        }
        GameObject.FindObjectOfType<NotificationPanel>().ShowFullMessage(PlayerPrefsManager.Notifications[notifID]);

    }

}
