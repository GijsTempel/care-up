using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SearchBar : MonoBehaviour
{
    private bool flag;
    
    public GameObject listContent = null;
    public List<GameObject> sceneObjects = new List<GameObject>();
    public GameObject ScenesListPanel = null;
    string searchGroup = "";
    public void Input()
    {
        flag = true;
        if (ScenesListPanel != null)
            ScenesListPanel.SetActive(true);
    }

    public void SearchByGroup(string _searchGroup)
    {
        searchGroup = _searchGroup;
        flag = true;
        if (ScenesListPanel != null)
            ScenesListPanel.SetActive(true);
    }

    public void ClearSearch()
    {
        GetComponent<InputField>().text = "";
    }

    public void ClearGroup()
    {
        searchGroup = "";
    }

    void Update()
    {
        if (flag)
        {
            if (sceneObjects.Count != 0)
            {
                if (sceneObjects[0] == null)
                {
                    transform.Find("SearchBarText").GetComponent<Text>().text = "";
                    FillList();
                }                  
            }

            if (sceneObjects.Count == 0)
            {
                if (listContent != null)                
                    FillList();
            }

            else
            {
                bool noMatch = true;
                List<LevelButton> visibleDemoButtons = new List<LevelButton>();
                for (int i = 0; i < sceneObjects.Count; i++)
                {
                    GameObject scene = sceneObjects[i];
                    string sceneName = null;

                    if (GameObject.FindObjectOfType<LeaderBoard>().lead.activeSelf)
                        sceneName = scene.transform.Find("Text").GetComponent<Text>().text.Replace(" ", "");
                    else
                        sceneName = scene.GetComponent<LevelButton>().displayName.Replace(" ", "");
                    
                    string searchText = transform.Find("SearchBarText").GetComponent<Text>().text;
                    bool sceneMatch = true;
                    if (searchText != "")
                        sceneMatch = FuzzyMatcher.FuzzyMatch(sceneName.Replace(" ", ""), searchText.Replace(" ", ""));
                    if (searchGroup != "")
                    {
                        if (!scene.GetComponent<LevelButton>().inGroups.Contains(searchGroup))
                        {
                            sceneMatch = false;
                            noMatch = false;
                        }
                    }


                    if (sceneMatch)
                        noMatch = false;

                    scene.SetActive(sceneMatch);
                    if (sceneMatch && !scene.GetComponent<LevelButton>().isFree)
                        visibleDemoButtons.Add(scene.GetComponent<LevelButton>());
                }
                if (noMatch)
                {
                    foreach (GameObject scene in sceneObjects)
                    {
                        scene.SetActive(false);
                    }
                }
                if (visibleDemoButtons.Count > 0)
                {
                    for (int i = 0; i < visibleDemoButtons.Count; i++)
                    {
                        Debug.Log(visibleDemoButtons[i].displayName);
                        if (visibleDemoButtons.Count == 1)
                        {
                            visibleDemoButtons[i].ShowDemoMarks(0);
                        }
                        else
                        {
                            if (i == 0)
                            {
                                visibleDemoButtons[i].ShowDemoMarks(1);
                            }
                            else if (i == visibleDemoButtons.Count - 1)
                            {
                                visibleDemoButtons[i].ShowDemoMarks(3);
                            }
                            else
                                visibleDemoButtons[i].ShowDemoMarks(2);
                        }
                    }
                }
                flag = false;
            }       
        }
    }




    void FillList()
    {
        sceneObjects.Clear();

        foreach (Transform child in listContent.transform)
        {
            sceneObjects.Add(child.gameObject);
        }
    }
}
