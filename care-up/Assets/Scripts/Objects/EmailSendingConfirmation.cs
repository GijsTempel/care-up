using UnityEngine;

public class EmailSendingConfirmation : MonoBehaviour
{
    [SerializeField]
    private GameObject popUpObject = null;  

    public void Confirmation()
    {
        if (SendEndScoreButton.EmailSent)
        {
            popUpObject.SetActive(true);
            Debug.Log("Success pop-up. End score e-mail sent succesfully");
        }

        Debug.Log("End score e-mail not sent");
    }
}
