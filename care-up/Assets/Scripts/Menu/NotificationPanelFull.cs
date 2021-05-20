using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NotificationPanelFull : MonoBehaviour
{
    public Text authorText;
    public Text messageText;
    public Text titleText;
    public Text dateText;
    public ScrollRect messageScroll;

    public void ShowMessage(PlayerPrefsManager.CANotifications _notif)
    {
        gameObject.SetActive(true);
        messageText.text = _notif.message;
        authorText.text = _notif.author;
        titleText.text = _notif.title;
        dateText.text = _notif.GetCreatedTimeString();
        StartCoroutine("ResetScroll");
    }

    IEnumerator ResetScroll()
    {
        yield return new WaitForSeconds(0.1f);
        messageScroll.verticalScrollbar.value = 3f;            
        yield return null;
    }

    public void HideMessage()
    {
        gameObject.SetActive(false);
    }
}
