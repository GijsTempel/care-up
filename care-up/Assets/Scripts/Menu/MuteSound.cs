using System;
using UnityEngine;
using UnityEngine.UI;

public class MuteSound : MonoBehaviour
{
    [SerializeField] Sprite normal = default;
    [SerializeField] Sprite muted = default;
    [SerializeField] Image generalAudioImage = default;
    [SerializeField] Image menuAudioImage = default;

    private PlayerPrefsManager manager;

    void Start()
    {
        manager = FindObjectOfType<PlayerPrefsManager>();
        generalAudioImage.sprite = (AudioListener.volume == 0.0f) ? muted : normal;
        menuAudioImage.sprite = Convert.ToBoolean(manager.MenuAudio) ? normal : muted;
    }

    public void ToggleMuteButton()
    {
        if (manager != null)
        {
            if (manager.Volume > 0f)
            {
                manager.Volume = 0f;
                generalAudioImage.sprite = muted;
            }
            else
            {
                manager.Volume = 1f;
                generalAudioImage.sprite = normal;
            }
            AudioListener.volume = manager.Volume;
        }
        else
        {
            if (AudioListener.volume > 0.0f)
            {
                AudioListener.volume = 0.0f;
                generalAudioImage.sprite = muted;
            }
            else
            {
                AudioListener.volume = 1f;
                generalAudioImage.sprite = normal;
            }
        }
    }

    public void ToggleMuteMenuButton()
    {
        if (manager.GetComponent<AudioSource>().isPlaying)
        {
            manager.GetComponent<AudioSource>().Stop();
            manager.MenuAudio = 0;
            menuAudioImage.sprite = muted;
        }
        else
        {
            manager.GetComponent<AudioSource>().Play();
            manager.MenuAudio = 1;
            menuAudioImage.sprite = normal;
        }
    }
}
