using UnityEngine;
using CareUpAvatar;

public class LoadCharacterScene : MonoBehaviour
{
    [SerializeField]
    private GameObject characters = default(GameObject);

    public void LoadScene()
    {
        GameObject.FindObjectOfType<PlayerPrefsManager>().firstStart = false;
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation");
    }

    public void LoadCharacter()
    {
        PlayerAvatar mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();
        Gender gender = CharacterInfo.sex == "Female" ? Gender.Female : Gender.Male;
        PlayerAvatarData _data = new PlayerAvatarData(gender, CharacterInfo.headType, 
                CharacterInfo.bodyType, CharacterInfo.glassesType);

        mainAvatar.avatarData = _data;
        mainAvatar.UpdateCharacter();
    }


}
