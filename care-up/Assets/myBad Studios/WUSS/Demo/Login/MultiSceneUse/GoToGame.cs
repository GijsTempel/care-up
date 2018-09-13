using UnityEngine;
using UnityEngine.SceneManagement;
using MBS;

public class GoToGame : MonoBehaviour {

    void Start()
    {
        WULogin.onLoggedIn += GoToGameScene;
        WULogin.onResumeGame += ResumeGame;
    }
    void GoToGameScene( CML response ) => SceneManager.LoadScene("gamescene");
	void ResumeGame() => SceneManager.LoadScene("gamescene");
}
