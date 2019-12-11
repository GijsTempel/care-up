using System.Collections.Generic;
using UnityEngine;

public class RandomQuiz
{
    private int frequency;
    private int counter;
    private int selectedStep = 0;
    private ActionManager manager;
    public static bool showQuestion = false;

    public static List<QuizTab.Question> randomQuestionsList;

    private void SelectRandomStep()
    {
        int maxValue = counter + frequency;
        selectedStep = Random.Range(counter, maxValue) - 1;
        counter += frequency;
    }

    private void GetFrequency()
    {
        if (manager == null)
        {
            counter = 0;
            manager = GameObject.FindObjectOfType<ActionManager>();
        }

        int value = manager.StepsList.Count / randomQuestionsList.Count;
        frequency = value > 3 ? value : 4;
    }

    public void NextRandomQuiz()
    {
        if (randomQuestionsList.Count > 0)
        {
            if (counter <= 0 || selectedStep < manager.CorrectStepIndexes.Count)
            {
                GetFrequency();
                SelectRandomStep();
            }
            else if (manager.CorrectStepIndexes.Count == selectedStep)
            {              
                showQuestion = System.Convert.ToBoolean(Random.Range(0, 2));
                SelectRandomStep();
            }           
        }
    }
}
