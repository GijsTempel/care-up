using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelectionScene : MonoBehaviour {
    
    public void GoToLevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }
	
}
