using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MuteSoundButton : MonoBehaviour {

    public Sprite normal;
    public Sprite muted;

    private Image selfImage;
    
	void Start () {

        Button selfBtn = GetComponent<Button>();
        selfBtn.onClick.AddListener(ToggleMuteButton);

        selfImage = GetComponent<Image>();

        selfImage.sprite = (AudioListener.volume == 0.0f) ? muted : normal;
	}

    public void ToggleMuteButton()
    {
        if (AudioListener.volume != 0.0f)
        {
            AudioListener.volume = 0.0f;
            selfImage.sprite = muted;
        }
        else
        {
            PlayerPrefsManager manager = FindObjectOfType<PlayerPrefsManager>();
            if (manager != null)
            {
                AudioListener.volume = manager.Volume;
                selfImage.sprite = normal;
            }
            else
            {
                Debug.LogWarning("Cannot unmute without Preferences. Start from the first scene.");
            }
        }
    }
}
