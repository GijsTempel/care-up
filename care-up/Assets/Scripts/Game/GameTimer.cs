using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles counting time for scene.
/// </summary>
public class GameTimer : MonoBehaviour {

    private Text timerText;

    private float currentTime = 0.0f;
    private bool set = false;
	
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value < 0 ? 0 : value; }
    }

    public void SetTextObject(Text t)
    {
        timerText = t;
        set = true;
    }

    void Update()
    {
        if (!set) return;

        currentTime += Time.deltaTime;

        if (timerText.gameObject.activeSelf)
        {
            timerText.text = string.Format("{0}:{1:00}", (int)currentTime / 60, (int)currentTime % 60);
        }
    }
    
    public void Reset()
    {
        currentTime = 0.0f;
    }
}
