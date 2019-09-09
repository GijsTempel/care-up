using UnityEngine;

public class LoadCharacterScene : MonoBehaviour
{
    [SerializeField]
    private GameObject characters;

    public void LoadScene()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation");
    }

    public void LoadCharacter()
    {
        characters.SetActive(true);
        GameObject.FindObjectOfType<CharacterCreationScene>()
            .ShowCharacter();
    }

    public void HideCharacter()
    {
        if (characters.activeSelf)
            characters.SetActive(false);
    }
}
