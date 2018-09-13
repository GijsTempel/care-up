using UnityEngine;
using UnityEngine.UI;
using MBS;

public class WUTDemo : MonoBehaviour {
	
	public WUTimer
		Energy,
		Stamina,
		Lives;

	public Text
		energy_text,
		stamina_text,
		lives_text;

	// Use this for initialization
	void Start () => Energy.onTimerEvent += OnTimerResponse;

	//You can also receive updates when the timer has made contact with the server (usually that means a timer has reached 0)
	//in this instance I am using the first server response to trigger showing my stats
	void OnTimerResponse(int points)
	{
		Energy.onTimerEvent -= OnTimerResponse;
		InvokeRepeating ("ShowValues", 0f, 1f);
	}

	void ShowValues()
	{
		energy_text.text	= $"{Energy.Value}/{Energy.ValueBounds}\n({Energy.FormattedTimer})";
		stamina_text.text	= $"{Stamina.Value}/{Stamina.ValueBounds}\n({Stamina.FormattedTimer})";
		lives_text.text		= $"{Lives.Value}/{Lives.ValueBounds}";
	}

	public void DepleteEnergy() => Energy.SpendPoints(1); 
	public void DepleteStamina() => Stamina.SpendPoints(1); 
	public void GainEnergy() => Energy.GivePoints(1); 
	public void IncreaseMaxEnergy() => Energy.UpdateMaxPoints(1); 
}
