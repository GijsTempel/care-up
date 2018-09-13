using UnityEngine;
using UnityEngine.SceneManagement;

public class goBackToMenu : MonoBehaviour{
    public void GoBack() => SceneManager.LoadScene( "menu" );
}
