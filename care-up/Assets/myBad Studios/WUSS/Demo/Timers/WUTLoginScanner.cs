using UnityEngine;
using MBS;

public class WUTLoginScanner : MonoBehaviour {

	public GameObject DemoTimerPrefab;
	public Canvas canvas;

	void Start() =>	WULogin.onLoggedIn += StartTimerDemo;
	
	void StartTimerDemo(object data)
	{
		GameObject go = Instantiate(DemoTimerPrefab);
		go.transform.SetParent (canvas.transform, false);
		Destroy (gameObject);
	}

}
