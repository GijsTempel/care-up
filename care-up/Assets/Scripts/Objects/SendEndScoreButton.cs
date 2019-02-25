using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendEndScoreButton : MonoBehaviour {

    private MBS.WUADisplay achievements;

    public static bool EmailSent { get; set; }

    void Start () {
        achievements = GameObject.Find ("AchievementsDisplayPrefab").GetComponent<MBS.WUADisplay> ();
    }

    public void EndScoreSendMailResults()
    {
        string topic = "Care Up accreditatie aanvraag";
        string content = "Completed scene: " + GameObject.FindObjectOfType<PlayerPrefsManager>().currentSceneVisualName + "\n";
        content += "Username: " + MBS.WULogin.username + "\n";
        content += "E-mail: " + MBS.WULogin.email + "\n";

        Text text = GameObject.Find("Interactable Objects/Canvas/Send_Score/GameObject (1)/Username/Text").GetComponent<Text>();

        content += "Big- of registratienummer:" + text.text + "\n";
        float percent = GameObject.FindObjectOfType<EndScoreManager>().percent;
        content += "Percentage: " + Mathf.FloorToInt(percent * 100f).ToString() + "%\n";

        achievements.UpdateKeys ("StudyPoints", 1);

        PlayerPrefsManager.__sendMail(topic, content);
        Debug.Log("E-mail verzonden");
        EmailSent = true;
    }
}
