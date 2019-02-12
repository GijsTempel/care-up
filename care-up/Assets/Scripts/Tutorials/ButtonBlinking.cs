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

    public void JoystickStopBlinking()
    {
        GameObject.Find("JoystickKnob").GetComponent<Animator>().SetTrigger("BlinkStop");
        GameObject.Find("JoystickBackground").GetComponent<Animator>().SetTrigger("BlinkStop");
    }
}
