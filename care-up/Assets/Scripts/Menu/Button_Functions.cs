using System.Collections;
using System.Collections.Generic;
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

}
