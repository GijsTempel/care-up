using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScene_UI_Option : MonoBehaviour {

    public Color selectedColor = Color.green;

    private bool selected = false;

    public string bundleName;
    public string sceneName;
    public string description;
    public string result;
    public Sprite image;
    
    public void SetSelected()
    {
        // turn on
        if (!selected)
        {
            LevelButton levelButton = GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/DialogTestPractice/Panel_UI/Buttons/Start")?.GetComponent<LevelButton>();

            levelButton.sceneName = sceneName;
            levelButton.bundleName = bundleName;

            LevelSelectionScene_UI_Option[] other = transform.parent.GetComponentsInChildren<LevelSelectionScene_UI_Option>();
            foreach (LevelSelectionScene_UI_Option ui in other)
            {
                ui.GetComponent<Image>().color = Color.white;
                ui.selected = false;
            }

            GetComponent<Image>().color = selectedColor;
            selected = true;
        }
    }    
}
