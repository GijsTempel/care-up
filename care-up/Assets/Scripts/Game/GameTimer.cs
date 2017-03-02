using UnityEngine;
using System.Collections;

/// <summary>
/// Handles counting time for scene.
/// </summary>
public class GameTimer : MonoBehaviour {

    private float currentTime = 0.0f;
	
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value < 0 ? 0 : value; }
    }

	void Update ()
    {
        currentTime += Time.deltaTime;
	}

    /// <summary>
    /// Draws current time top right corner.
    /// </summary>
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
