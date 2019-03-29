using UnityEngine;

public class LoadCharacterScene : MonoBehaviour
{
    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    public void LoadScene()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation"); 
    }

    public void LoadCharacter()
    {
        CharacterCreationScene character = new CharacterCreationScene(); 
        character.ShowCharacter(maleCharacter, femaleCharacter);
    }
}
