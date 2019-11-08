using UnityEngine;

public class Button_Functions : MonoBehaviour {

	public AudioClip buttonHover;
	public AudioClip buttonClick;
	private AudioSource source;

	void Awake()
	{
		source = GetComponent<AudioSource> ();
	}
	public void OnButtonHover()
	{
		source.PlayOneShot (buttonHover, 0.5F);
	}
	public void OnButtonClick()
	{
		source.PlayOneShot (buttonClick, 0.15F);
	}

    public void OnBuyButtonClick()
    {
        string link = "https://careup.nl/protocollen-voorbehouden-en-risicovolle-handelingen/";
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            Application.ExternalEval("window.open(\"" + link + "\",\"_blank\")");
            #pragma warning restore CS0618 // Type or member is obsolete
        }
        else
        {
            Application.OpenURL(link);
        }
    }
}
