using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SendEndScoreButton : MonoBehaviour {

    public void EndScoreSendMailResults()
    {
        string topic = "Care Up accreditatie aanvraag";
        string content = "Completed scene: " + GameObject.FindObjectOfType<PlayerPrefsManager>().currentSceneVisualName + "\n";
        content += "Username: " + MBS.WULogin.username + "\n";
        content += "E-mail: " + MBS.WULogin.email + "\n";

        Text text = GameObject.Find("Interactable Objects/Canvas/Send_Score/GameObject (1)/Username/Text").GetComponent<Text>();

        content += "big- of registratienummer:" + text.text;

        PlayerPrefsManager.__sendMail(topic, content);
        Debug.Log("E-mail verzonden");
    }
}
