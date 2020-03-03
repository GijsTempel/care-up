using UnityEngine;

/// <summary>
/// Narrator class, that plays audiofile as playing 'thinks' about smth
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Narrator : MonoBehaviour
{
    private static AudioSource[] sources;
    private static AudioSource playerSource;
    private static string currentAudioClip;

    private static AudioSource additionalPlayerSource;

    void Start()
    {
        if (sources == null)
        {
            sources = gameObject.GetComponents<AudioSource>();

            if (sources.Length == 0)
            {
                Debug.LogError("No AudioSource on narrator found");
            }
            else
            {
                currentAudioClip = sources[0].name;
                playerSource = sources[0];
            }

            additionalPlayerSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            if (sources[sources.Length - 1] == null)
            {
                sources = gameObject.GetComponents<AudioSource>();

                if (sources.Length == 0)
                {
                    Debug.LogError("No AudioSource on narrator found");
                }
                else
                {
                    playerSource = sources[0];
                }
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
        playerSource.mute = false;
        if (sound == "WrongAction")
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/WA1-1");
            if (clip == null)
            {
                Debug.LogWarning("No audio clip " + sound + " found!");
            }
            else
            {
                currentAudioClip = "WA1-1";
                PlayDialogueSound(clip);
            }
            return true;
        }

        foreach (AudioSource src in sources)
        {
            if (src.isPlaying || src == playerSource)
            {
                continue;
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
                    src.PlayOneShot(clip, volume);
                }
                return true;
            }
        }

        Debug.LogWarning("No available AudioSources!");
        return false;
    }

    /// <summary>
    /// Plays sound clip
    /// </summary>
    /// <param name="sound">sound name</param>
    /// <returns>True if played</returns>
    public static bool PlayDialogueSound(AudioClip sound, float volume = 1.0f)
    {
        if (sound != null)
        {
            if (playerSource.isPlaying && (currentAudioClip == "WA1-1") && (sound.name != "WA1-1"))
            {
                playerSource.mute = true;
                additionalPlayerSource.PlayOneShot(sound, volume);
                return true;
            }
        }
        if (playerSource.isPlaying)
        {
            Debug.LogWarning("Player is already saying something.");
            return false;
        }
        else
        {
            if (sound != null)
            {
                currentAudioClip = sound.name;
            }
            playerSource.PlayOneShot(sound, volume);
            return true;
        }
    }

    // checks if hint already playing to avoid multiple at same time
    public static bool PlayHintSound(string sound, float volume = 1.0f)
    {
        playerSource.mute = false;

        if (playerSource.isPlaying)
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
                currentAudioClip = sound;
                playerSource.PlayOneShot(clip, volume);
            }

            return true;
        }
    }
}
