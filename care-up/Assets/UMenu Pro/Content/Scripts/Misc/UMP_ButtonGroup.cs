using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UMP_ButtonGroup : MonoBehaviour, IPointerClickHandler
{
    public int GroupID = 0;
    public bool Select = false;

    private Button button;

    void Start()
    {
        if (Select)
        {
            OnSelect();
        }
    }

    public void OnPointerClick(PointerEventData e)
    {
        OnSelect();
    }

    public void OnSelect()
    {
        UMP_ButtonGroup[] all = FindObjectsOfType<UMP_ButtonGroup>();
        foreach (UMP_ButtonGroup b in all)
        {
            if (b.GroupID == GroupID)
            {
                b.UnSelect();
            }
        }
        if (Button.name != "ScoresBTN" && Button.name != "AchievementBTN")
            Button.interactable = false;
    }

    public void UnSelect()
    {
        Button.interactable = true;
    }

    private Button Button
    {
        get
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            return button;
        }
    }
}