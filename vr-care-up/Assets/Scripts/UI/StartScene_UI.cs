using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene_UI : MonoBehaviour
{
    public void OnProtocolSelectBtnPushed(string protocolName)
    {
        SceneManager.LoadSceneAsync(protocolName);
        // fade-in, whynot
        PlayerScript player = GameObject.FindAnyObjectByType<PlayerScript>();
        if (player != null)
        {
            player.fadeAnimator.SetTrigger("in");
        }
    }
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
