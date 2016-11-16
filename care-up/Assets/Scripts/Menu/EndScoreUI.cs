using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndScoreUI : MonoBehaviour {

	public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }

}
