using UnityEngine;
using CareUpAvatar;

public class LoadCharacterScene : MonoBehaviour
{
    public void LoadScene()
    {
        GameObject.FindObjectOfType<PlayerPrefsManager>().firstStart = false;
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation");
    }

    public void LoadCharacter()
    {
        PlayerAvatar mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();

        mainAvatar.avatarData = GetCurrentData();
        mainAvatar.UpdateCharacter();
    }

    public void LoadCharacter(PlayerAvatar avatar)
    {
        avatar.avatarData = GetCurrentData();
        avatar.UpdateCharacter();
    }


    public PlayerAvatarData GetCurrentData()
    {
        Gender gender = CharacterInfo.sex == "Female" ? Gender.Female : Gender.Male;

        PlayerAvatarData data = new PlayerAvatarData(gender, CharacterInfo.headType,
               CharacterInfo.bodyType, CharacterInfo.glassesType);
        data.hat = CharacterInfo.hat;

        return data;
    }
}
