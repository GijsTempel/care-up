using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SearchBar : MonoBehaviour
{
    private bool flag;
    public GameObject listContent = null;
    public List<GameObject> sceneObjects = new List<GameObject>();    

    public void Input()
    {
        flag = true;
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

                foreach (GameObject scene in sceneObjects)
                {
                    string sceneName = null;

                    if (GameObject.FindObjectOfType<LeaderBoard>().lead.activeSelf)
                        sceneName = scene.transform.Find("Text").GetComponent<Text>().text.Replace(" ", "");
                    else
                        sceneName = scene.GetComponent<LevelButton>().displayName.Replace(" ", "");

                    string searchText = transform.Find("SearchBarText").GetComponent<Text>().text;
                    bool sceneMatch = FuzzyMatcher.FuzzyMatch(sceneName.Replace(" ", ""), searchText.Replace(" ", ""));

                    if (sceneMatch)
                        noMatch = false;

                    scene.SetActive(sceneMatch);
                }

                foreach (GameObject scene in sceneObjects)
                {
                    if (noMatch)
                        scene.SetActive(true);
                }
            }       

            flag = false;
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
