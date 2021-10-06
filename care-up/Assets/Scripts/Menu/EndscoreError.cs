using UnityEngine;
using UnityEngine.UI;

public class EndscoreError : MonoBehaviour
{
    [SerializeField]
    private GameObject errorMessageObject = null;

    [SerializeField]
    private Text errorMessageText = null;

#if UNITY_EDITOR
    private void Start()
    {
        EndScoreManager endScoreManager = GameObject.FindObjectOfType<EndScoreManager>();
        if (endScoreManager != null)
        {
            int quizesPast = endScoreManager.quizQuestionsTexts.Count + endScoreManager.quizWrongIndexes.Count;
            string message = null;

            if (QuizTab.totalQuizesCount > quizesPast)
            {
                message = $"Passed {quizesPast} quiz steps from total {QuizTab.totalQuizesCount}.";

                if (errorMessageObject != null && errorMessageText != null)
                {
                    errorMessageObject.SetActive(true);
                    errorMessageText.text = message;
                }
            }
        }
    }
#endif
}
