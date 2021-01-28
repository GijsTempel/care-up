using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndScoreWrongStepDescr : MonoBehaviour, IPointerEnterHandler
{ 
    public string text;
    public bool wrong;
    
    private Color redish = new Color(0.5f, 0.0f, 0.0f, 1.0f);
    private Color greyish = new Color(1f, 1f, 1f, 1.0f);

    private static Button_Functions sounds;

    void Start()
    {
        GetComponent<Text>().color = wrong ? Color.red : Color.white;

        if (sounds == null)
        {
            sounds = GameObject.FindObjectOfType<Button_Functions>();
            if (sounds == null) Debug.LogWarning("No sounds for UI?");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sounds != null)
        {
            sounds.OnButtonHover();
        }

        foreach (Text text in transform.parent.GetComponentsInChildren<Text>())
        {
            text.color = text.GetComponent<EndScoreWrongStepDescr>().wrong ? redish : greyish;
        }

        GetComponent<Text>().color = wrong ? Color.red : Color.white;
        GameObject.Find("StepDescription").GetComponent<Text>().text = text;
    }
}
