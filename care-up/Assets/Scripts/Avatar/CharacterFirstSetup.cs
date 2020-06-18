using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFirstSetup : MonoBehaviour
{
    public InputField BigNumberHolder;
    public InputField FullName;
    public List<GameObject> tabs;
    public Button NextButton;
    public PlayerAvatar Avatar;
    private int currentChar = 0;
    private int currentTab = 0;
    public GameObject NoBigPopUp;
    private bool DontHaveBIG = false;

    PlayerPrefsManager pref;
    
    void Start()
    {
        pref = GameObject.FindObjectOfType<PlayerPrefsManager>();

        if (pref != null)
        {
            if (!PlayerPrefsManager.firstStart)
            {
                NextButton.transform.Find("Text").GetComponent<Text>().text = "Opslaan";
                FullName.text = pref.fullPlayerName;
                BigNumberHolder.text = pref.bigNumber;
            }
            else
            {
                Invoke("Initialize", 0.01f);
            }
        }
    }

    void Initialize()
    {
        SetCharacter(0);
    }

    public void SetCharacter(int n)
    {
        if (pref != null)
        {
            Avatar.avatarData = PlayerPrefsManager.storeManager.CharacterItems[n].playerAvatar;
            Avatar.UpdateCharacter();
            currentChar = n;
        }
    }

    public void ShowNoBigNum(bool value)
    {
        if (!value && !DontHaveBIG)
            BigNumberHolder.transform.GetComponentInParent<Animator>().SetTrigger("red");
        NoBigPopUp.SetActive(value);
    }
  

    public void IDontHaveBIG()
    {
        DontHaveBIG = true;
        if (FullName.text != "")
            SetTab(1);
        NoBigPopUp.SetActive(false);
        PlayerPrefsManager.SetBIGNumber(BigNumberHolder.text);
    }

    bool CheckFirstTab()
    {
        bool check = true;
        if (BigNumberHolder.text == "")
        {
            if (!DontHaveBIG)
            {
                ShowNoBigNum(true);
                return false;
            }
            //check = false;
        }

        if (FullName.text == "")
        {
            FullName.transform.GetComponentInParent<Animator>().SetTrigger("red");
            check = false;
        } 
        return check;
    }

    public void SetTab(int tab)
    {
        bool check = true;        

        if (currentTab == 0)
        {
            check = CheckFirstTab();
        }

        if (check)
        {
            print(PlayerPrefsManager.firstStart);
            if (!PlayerPrefsManager.firstStart && currentTab == 0)
            {
                tab = -1;
            }
            if (tab >= 0 && tab < tabs.Count)
            {
                foreach (GameObject t in tabs)
                {
                    t.SetActive(false);
                }
                tabs[tab].SetActive(true);
                currentTab = tab;
            }
            else
            {
                // save full name
                PlayerPrefsManager.SetFullName(FullName.text);
                // save big number
                PlayerPrefsManager.SetBIGNumber(BigNumberHolder.text);
                if (PlayerPrefsManager.firstStart)
                {
                    CharacterInfo.SetCharacterCharacteristicsWU(PlayerPrefsManager.storeManager.CharacterItems[currentChar]);
                }
                // set new character scene to be seen and saved info
                DatabaseManager.UpdateField("AccountStats", "CharSceneV2", "true");
                bool goToMainMenu = (DatabaseManager.FetchField("AccountStats", "TutorialCompleted") == "true");

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (PlayerPrefsManager.tutorialOnStart)
                    goToMainMenu = false;
#endif
                if (goToMainMenu)
                {
                    bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
                }
                else
                {
                    bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Tutorial", "scene/scenes_tutorial");
                }
            }
        }
    }
}
