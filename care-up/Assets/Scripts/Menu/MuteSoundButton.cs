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
        PlayerPrefsManager manager = FindObjectOfType<PlayerPrefsManager>();
        if (manager != null)
        {
            if (manager.Volume > 0f)
            {
                manager.Volume = 0f;
                selfImage.sprite = muted;
            }
            else
            {
                manager.Volume = 1f;
                selfImage.sprite = normal;
            }
            AudioListener.volume = manager.Volume;
        }
        else
        {
            if (AudioListener.volume > 0.0f)
            {
                AudioListener.volume = 0.0f;
                selfImage.sprite = muted;
            }
            else
            {
                AudioListener.volume = 1f;
                selfImage.sprite = normal;
            }
        }
    }
}
