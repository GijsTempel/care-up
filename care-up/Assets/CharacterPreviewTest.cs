using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;

public class CharacterPreviewTest : MonoBehaviour
{
    public void PreviewCharacter()
    {
        PlayerAvatar mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();
        PlayerAvatarData prevCharData = new PlayerAvatarData();
        prevCharData.headType = 5;
        mainAvatar.avatarData = prevCharData;
        mainAvatar.UpdateCharacter();
    }
}
