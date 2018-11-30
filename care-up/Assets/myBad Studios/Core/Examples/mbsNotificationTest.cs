using UnityEngine;
using MBS;

//To use this sample, create a Canvas object in your scene 
//(ideallly set to use Constant Physical Size)
//Next, drag this script onto any object in your scene and drag the Canvas onto this script
//Hit play and enjoy

//In reality you might want to calculate the screen size and determine the notification's
//position using that. For this example I assume an iOS game on iPhone 5 Tall setting and
//hard code my values to work with that setup.

public class mbsNotificationTest : MonoBehaviour {

	public Canvas canvas;

	void Start () {
		//Notifications automatically queue
		ShowNotification ();
		ShowNotification2 ();
		ShowNotification ();
	}
	
	void ShowNotification()
	{
		MBSNotification.SpawnInstance(
			canvas, 
			new Vector2(200f, -80f), 
			new Vector2(-10f, -80f), 
			"Notification Header", 
			"This is the notification text");
	}
	
	void ShowNotification2()
	{
		MBSNotification.SpawnInstance(
			canvas, 
			new Vector2(0f, 100f), 
			new Vector2(0f, -80f), 
			"Notification Header", 
			"This is a differnt notification from the first one");
	}
	

}