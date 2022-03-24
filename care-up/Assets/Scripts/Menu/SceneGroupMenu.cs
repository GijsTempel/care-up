using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;



public class SceneGroupMenu : MonoBehaviour
{
    private struct SceneGroupDataStruct
    {
        public string id;
        public string name;
        public string icon;
    }
    public SearchBar searchBar;
    public GameObject sceneList;
    public Animation SGPagePanelAnimation;
    int numberOfPages = 5;
    int currentPage = 0;
    public GameObject sceneGroupPagePanel;
    List<SceneGroupPageButton> sceneGroupPageButtons = new List<SceneGroupPageButton>();
    List<SceneGroupDataStruct> sceneGroupsData = new List<SceneGroupDataStruct>();
    public List<GameObject> SceneGroupButtonPanes = new List<GameObject>();
    List<SceneGroupButton> SceneGroupButtons = new List<SceneGroupButton>();
    float SceneListShowTimeout = 0f;
    public void SwitchPage(int nextPage)
    {
        Debug.Log(nextPage);
        SetActivePageButton(nextPage);
        
        if (nextPage > currentPage)
        {
            SGPagePanelAnimation.Play("SceneGroupLeft");
        }
        else if (nextPage < currentPage)
        {
            SGPagePanelAnimation.Play("SceneGroupRight");
        }
        currentPage = nextPage;
        UpdatePage();
    }

    public void ShowSceneList(bool toShow)
    {
        if (!toShow)
        {
            searchBar.ClearSearch();
            searchBar.ClearGroup();
            SceneListShowTimeout = 0.01f;
        }
        else
        {
            sceneList.SetActive(true);
        }
    }


    private void Update()
    {
        if (SceneListShowTimeout > 0)
        {
            SceneListShowTimeout -= Time.deltaTime;
            if (SceneListShowTimeout < 0)
            {
                sceneList.SetActive(false);
            }
        }
    }
    void UpdatePage()
    {
        for (int i = 0; i < 3; i++)
        {
            int currentGroup = currentPage * 3 + i;
            SceneGroupButtonPanes[i].SetActive(currentGroup < sceneGroupsData.Count);
            if (currentGroup < sceneGroupsData.Count)
                SceneGroupButtons[i].SetButtonData(currentGroup, sceneGroupsData[currentGroup].name, "", 0);
        }
    }

    void LoadGroupData()
    {
        sceneGroupsData.Clear();
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/SceneGroups");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList _sceneGroupsInfo = xmlFile.FirstChild.NextSibling.ChildNodes;
        SceneGroupDataStruct other_sceneGroupData = new SceneGroupDataStruct();
        
        foreach (XmlNode s in _sceneGroupsInfo)
        {
            SceneGroupDataStruct _sceneGroupData = new SceneGroupDataStruct();

            _sceneGroupData.name = (s.Attributes["name"].Value);
            if (s.Attributes["icon"] != null)
                _sceneGroupData.icon = (s.Attributes["icon"].Value);
            else
                _sceneGroupData.icon = "";
            if (s.Attributes["id"] != null)
                _sceneGroupData.id = (s.Attributes["id"].Value);
            else
                _sceneGroupData.id = "";
            if (_sceneGroupData.id == "o")
            {
                other_sceneGroupData.icon = _sceneGroupData.icon;
                other_sceneGroupData.id = _sceneGroupData.id;
                other_sceneGroupData.name = _sceneGroupData.name;
            }
            else
                sceneGroupsData.Add(_sceneGroupData);
        }
        sceneGroupsData.Add(other_sceneGroupData);
        numberOfPages = sceneGroupsData.Count / 3 + ((sceneGroupsData.Count % 3 > 0) ? 1 : 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGroupData();
        Object SGPageButtonPrefab = Resources.Load<GameObject>("NecessaryPrefabs/UI/SGPageButton");
        for (int i = 0; i < numberOfPages; i++)
        {
            GameObject packageButton = Instantiate(SGPageButtonPrefab, sceneGroupPagePanel.transform) as GameObject;
            packageButton.GetComponent<SceneGroupPageButton>().pageID = i;
            sceneGroupPageButtons.Add(packageButton.GetComponent<SceneGroupPageButton>());
        }
        SetActivePageButton(0);
        foreach(GameObject p in SceneGroupButtonPanes)
        {
            SceneGroupButtons.Add(p.transform.GetChild(0).GetComponent<SceneGroupButton>());
        }
        SwitchPage(0);
    }

    public void NextPage(int _dir)
    {
        int newPage = currentPage + _dir;
        if (newPage >= 0 && newPage < numberOfPages)
        {
            SwitchPage(newPage);
        }
    }
    private void Awake()
    {
        ShowSceneList(false);
    }

    void SetActivePageButton(int activeButtonID)
    {
        Color aColor = Color.white;
        Color bColor = Color.white;
        bColor.a = 0.3f;
        for(int i = 0; i < sceneGroupPageButtons.Count; i++)
        {
            ColorBlock cb = sceneGroupPageButtons[i].GetComponent<Button>().colors;
            if (activeButtonID == i)
                cb.normalColor = aColor;
            else
                cb.normalColor = bColor;
            sceneGroupPageButtons[i].GetComponent<Button>().colors = cb;
        }
    }
    public void OpenGroup(int _id)
    {
        searchBar.SearchByGroup(sceneGroupsData[_id].id);
        ShowSceneList(true);
    }
}
