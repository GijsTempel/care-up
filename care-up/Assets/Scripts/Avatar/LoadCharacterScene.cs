using UnityEngine;
using CareUpAvatar;

public class LoadCharacterScene : MonoBehaviour
{
    PlayerPrefsManager pref;
    public void LoadScene()
    {
        pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        pref.firstStart = false;
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation");
    }

    public void LoadCharacter()
    {
        PlayerAvatar mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();

        mainAvatar.avatarData = GetCurrentData();
        print(GetCurrentData().eyeType);
        mainAvatar.UpdateCharacter();
    }

    public void LoadCharacter(PlayerAvatar avatar)
    {
        avatar.avatarData = GetCurrentData();
        avatar.UpdateCharacter();
    }

    

    public PlayerAvatarData GetCurrentData()
    {
        if (pref == null)
            pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        Gender gender = CharacterInfo.sex == "Female" ? Gender.Female : Gender.Male;

        PlayerAvatarData data = new PlayerAvatarData(gender, CharacterInfo.headType,
               CharacterInfo.bodyType, CharacterInfo.glassesType);
        data.hat = CharacterInfo.hat;
        
        data.eyeType = PlayerPrefsManager.storeManager.CharacterItems[pref.CarouselPosition].playerAvatar.eyeType;
        return data;
    }
}
