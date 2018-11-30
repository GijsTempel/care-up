using UnityEngine;
using MBS;

public class WUDataDemo2 : MonoBehaviour {

	public Rect area;
	public GUISkin the_skin;

	bool ready = false;

	// Use this for initialization
	void Start () {
		area.x = (Screen.width - area.width) / 2;
		area.y = (Screen.height - area.height) / 2;
	}
	
	void OnGUI()
	{
		if (!(WULogin.logged_in && ready)) return;

		GUI.skin = the_skin;
		GUI.Window(0, area, DrawWindow, "");
	}

	void PrintResponse(CML response)
	{
		print(response.ToString());
	}

	void DrawWindow(int id)
	{
		if (GUILayout.Button("Add some shared data"))
		{
			CMLData data = new CMLData();
			data.Set("Field_1", "Value 1");
			data.Set("Field_2", "Value 2");
			WUData.UpdateSharedCategory("Category1", data, WPServer.GameID, response: PrintResponse);
			data.Set("Field_3", "Value 3");
			data.Set("Field_4", "Value 4");
			WUData.UpdateSharedCategory("Category2", data, WPServer.GameID, response: PrintResponse);
		}
		
		if (GUILayout.Button("Fetch a single shaared field"))
			WUData.FetchSharedField("Field_1", "Category1", PrintResponse, WPServer.GameID);
		
		if (GUILayout.Button("Fetch a shared category"))
			WUData.FetchSharedCategory("Category1", PrintResponse, WPServer.GameID);
		
		if (GUILayout.Button("Fetch all shared data"))
			WUData.FetchAllSharedInfo(PrintResponse, WPServer.GameID);
		
		if (GUILayout.Button("Remove single shared field"))
			WUData.RemoveSharedField("Field_1", "Category1", PrintResponse, WPServer.GameID);
		
		if (GUILayout.Button("Remove shared category"))
			WUData.RemoveSharedCategory("Category1", PrintResponse, WPServer.GameID);
	}
	
}
