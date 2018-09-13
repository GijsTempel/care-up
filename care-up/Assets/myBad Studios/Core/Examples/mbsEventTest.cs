using UnityEngine;
using MBS;

/// <summary>
/// If you are new to the concept of using events and delegates you can learn more about them
/// in the Learn section of my website: www.guild.site/learn/
/// </summary>
public class mbsEventTest : MonoBehaviour {

	MBSEventHandler KeyResponder;

	string[] keys = new string[]{"w","a","s","d"};
	int key_presses = 0;

	void Start()
	{
		KeyResponder += RespondToKeyPress;
	}

	void OnDestroy()
	{
		KeyResponder -= RespondToKeyPress;
	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			foreach(string key in keys)
			if ( Input.GetKeyDown(key) )
			{
				MBSEvent my_event_data = new MBSEvent( ++key_presses);
				my_event_data.details[0].Set("key", key);

				KeyResponder(this, my_event_data);
			}
		}
	}

	void RespondToKeyPress(object source, MBSEvent data)
	{
		CMLData details = data.details[0];
		string ext = "th";
		int counter = details.Int();
		switch(counter)
		{
		case 1: ext = "st"; break;
		case 2: ext = "nd"; break;
		case 3: ext = "rd"; break;
		}
		Debug.Log(string.Format("The player pressed the {0} key which is the {1}{2} time a valid key was pressed", details.String("key"), counter, ext ) );
	}
	
}