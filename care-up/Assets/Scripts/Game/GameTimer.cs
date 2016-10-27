using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {

    private float currentTime = 0.0f;
	
	void Update ()
    {
        currentTime += Time.deltaTime;
	}

    void OnGUI()
    {
        string timeString = string.Format("{0}:{1:00}", (int)currentTime / 60, (int)currentTime % 60);

        GUIStyle style = GUI.skin.GetStyle("Label");
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 40;
        style.normal.textColor = Color.white;
        
        GUI.Label(new Rect(7 * Screen.width / 8, 0, Screen.width / 8,  Screen.height / 10),
        timeString, style);
    }

    public void Reset()
    {
        currentTime = 0.0f;
    }
}
