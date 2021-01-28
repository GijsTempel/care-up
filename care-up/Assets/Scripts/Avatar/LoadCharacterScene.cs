using UnityEngine;
using CareUpAvatar;

public class LoadCharacterScene : MonoBehaviour
{
    PlayerPrefsManager pref;
    public void LoadScene()
    {
        PlayerPrefsManager.firstStart = false;
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

    public void LoadCharacter(PlayerAvatarData avatarData)
    {
        PlayerAvatar mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();

        mainAvatar.avatarData = avatarData;
        mainAvatar.UpdateCharacter();
    }

    public PlayerAvatarData GetCurrentData()
    {
        if (pref == null)
            pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        //-
        CharacterItem Itam = PlayerPrefsManager.storeManager.GetAvatarData(CharacterInfo.index);
        if (Itam == null)
            return null;
        Gender gender = Itam.playerAvatar.gender;
        PlayerAvatarData data = new PlayerAvatarData(gender, Itam.playerAvatar.headType,
               Itam.playerAvatar.bodyType, Itam.playerAvatar.glassesType);
        data.hat = Itam.playerAvatar.hat;
        data.eyeType = Itam.playerAvatar.eyeType;
        return data;
    }
}
