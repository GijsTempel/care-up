using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Handles counting time for scene.
/// </summary>
public class GameTimer : MonoBehaviour {

    private Text timerText;

    private float currentTime = 0.0f;
	
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value < 0 ? 0 : value; }
    }

    private void Start()
    {
        timerText = Camera.main.transform.Find("UI (1)").Find("RobotUI").Find("GeneralTab").Find("Timer").Find("Panel").Find("Time").GetComponent<Text>();
    }

    void Update ()
    {
        currentTime += Time.deltaTime;

        timerText.text = string.Format("{0}:{1:00}", (int)currentTime / 60, (int)currentTime % 60);
    }
    
    public void Reset()
    {
        currentTime = 0.0f;
    }
}
