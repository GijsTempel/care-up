using UnityEngine;
using System.Collections;

/// <summary>
/// Narrator class, that plays audiofile as playing 'thinks' about smth
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Narrator : MonoBehaviour {

    private static AudioSource[] sources;
    private static AudioSource hintSrc;

    void Start() {

        if (sources == null)
        {
            sources = gameObject.GetComponents<AudioSource>();

            if (sources.Length == 0)
            {
                Debug.LogError("No AudioSource on narrator found");
            }
            else
            {
                hintSrc = sources[0];
            }
        }
    }

    /// <summary>
    /// Plays sound clip
    /// </summary>
    /// <param name="sound">sound name</param>
    /// <returns>True if played</returns>
    public static bool PlaySound(string sound, float volume = 1.0f)
    {
        foreach (AudioSource src in sources)
        {
            if (src.isPlaying || src == hintSrc)
            {
                continue;
            }
            else
            {
                if (sound == "WrongAction")
                {
                    sound = "WA1-1";
                }

                AudioClip clip = Resources.Load<AudioClip>("Audio/" + sound);
                if (clip == null)
                {
                    Debug.LogWarning("No audio clip " + sound + " found!");
                }
                else
                {
                    src.PlayOneShot(clip, volume);
                }
                return true;
            }
        }

        Debug.LogWarning("No available AudioSources! Add more to Narrator object.");
        return false;
    }

    /// <summary>
    /// Plays sound clip
    /// </summary>
    /// <param name="sound">sound name</param>
    /// <returns>True if played</returns>
    public static bool PlaySound(AudioClip sound, float volume = 1.0f)
    {
        foreach (AudioSource src in sources)
        {
            if (src.isPlaying || src == hintSrc)
            {
                continue;
            }
            else
            {
                src.PlayOneShot(sound, volume);
                return true;
            }
        }

        Debug.LogWarning("No available AudioSources! Add more to Narrator object.");
        return false;
    }

    // checks if hint already playing to avoid multiple at same time
    public static bool PlayHintSound(string sound, float volume = 1.0f)
    {

        if (hintSrc.isPlaying)
        {
            Debug.Log("Hint already playing");
            return false;
        }
        else
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + sound);
            if (clip == null)
            {
                Debug.LogWarning("No audio clip " + sound + " found!");
            }
            else
            {
                hintSrc.PlayOneShot(clip, volume);
            }

            return true;
        }
    }
}
