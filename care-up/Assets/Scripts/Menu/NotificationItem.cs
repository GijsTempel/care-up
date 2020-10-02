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
        star.SetActive(!notif.isRead);

        System.DateTime notifDate = UnixTimeStampToDate(notif.createdTime);
        string sDate = notifDate.Day.ToString() + "." + notifDate.Month.ToString() + "." + notifDate.Year.ToString();
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

    public System.DateTime UnixTimeStampToDate(long unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
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
            //---------------------------------------------------------------
        }
    }

}
