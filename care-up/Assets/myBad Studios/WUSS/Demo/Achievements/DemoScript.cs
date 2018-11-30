using UnityEngine;
using MBS;

public class DemoScript : MonoBehaviour {

    public string username, password;

    void Start () {
        CMLData credentials = new CMLData();
        credentials.Set( "testserial", username );
        credentials.Set( "123", password );
        WULogin.AttemptToLogin( credentials );
	}	
}
