using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditonalInfoButton : MonoBehaviour
{
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Scenes_Medication_Safety")
        {
            gameObject.SetActive(false);
        }
    }
}
