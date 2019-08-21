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
}
