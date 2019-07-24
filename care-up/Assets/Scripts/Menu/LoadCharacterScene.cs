using UnityEngine;

public class LoadCharacterScene : MonoBehaviour
{
    public void LoadScene()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation");
    }

    public void LoadCharacter()
    {
        //characters.SetActive(true);
        GameObject.FindObjectOfType<CharacterCreationScene>()
            .ShowCharacter();
    }

    public void Update()
    {
        //if(characters != null)
        //{
        //    if (GameObject.Find("LoaderRoot") != null || (GameObject.Find("Account") == false && characters.activeSelf))
        //    {
        //        characters.SetActive(false);
        //    }
        //    else if (GameObject.Find("Account") && characters.activeSelf == false)
        //    {
        //        characters.SetActive(true);
        //    }
        //}        
    }
}
