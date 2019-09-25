using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUpAvatar;


public class CharacterFirstSetup : MonoBehaviour
{
    public InputField BigNumberHolder;
    public InputField FullName;
    int currentTab = 0;
    public List<GameObject> tabs;
    public Button NextButton;
    public PlayerAvatar Avatar;
    int currentChar = 0;

    PlayerPrefsManager pref;
    
    // Start is called before the first frame update
    void Start()
    {
        pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (pref != null)
        {
            if (!pref.firstStart)
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

    bool CheckFirstTab()
    {
        bool check = true;
        if (BigNumberHolder.text == "")
        {
            BigNumberHolder.transform.GetComponentInParent<Animator>().SetTrigger("red");
            check = false;
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
            if (pref != null)
            {
                if (!pref.firstStart && currentTab == 0)
                {
                    tab = -1;
                }
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
                if (pref.firstStart)
                {
                    CharacterInfo.SetCharacterCharacteristicsWU(PlayerPrefsManager.storeManager.CharacterItems[currentChar].playerAvatar);
                }
                // set new character scene to be seen and saved info
                DatabaseManager.UpdateField("AccountStats", "CharSceneV2", "true");

                if (DatabaseManager.FetchField("AccountStats", "TutorialCompleted") == "true")
                {
                    bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
                }
                else
                {
                    bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Tutorial", "scenes_tutorial");
                }
            }
        }
    }

}
