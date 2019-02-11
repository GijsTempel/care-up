using UnityEngine;

public class ButtonBlinking : MonoBehaviour {

    public void StartBlinking()
    {
        GetComponent<Animator>().SetTrigger("BlinkStart");
    }

    public void StopBlinking()
    {
        GetComponent<Animator>().SetTrigger("BlinkStop");
    }    
}
