using Unity.VisualScripting;
using UnityEngine;

public class HelpCharacterManager : MonoBehaviour
{
    public void CorrectAction()
    {
        // if (GetComponent<Animation>() != null)
        GetComponent<Animator>().SetTrigger("Yes");
    }

    public void WrongAction()
    {
        // if (GetComponent<Animation>() != null)
        GetComponent<Animator>().SetTrigger("No");
    }
}
