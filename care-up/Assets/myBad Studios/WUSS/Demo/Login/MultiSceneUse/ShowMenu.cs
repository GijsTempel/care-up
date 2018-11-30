using UnityEngine;
using MBS;

public class ShowMenu : MonoBehaviour {
	void Start () {
        WUUGLoginGUI gui = FindObjectOfType<WUUGLoginGUI>();
        if ( WULogin.logged_in )
            gui?.ShowPostLoginMenu();
        else
            gui?.ShowLoginMenuScreen();
	}
}
