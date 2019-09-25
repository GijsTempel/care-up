using UnityEngine;
using CareUpAvatar;

public class CharacterInfo : MonoBehaviour
{
    public static string sex;
    public static int headType;
    public static int bodyType;
    public static int glassesType;

    public static void SetCharacterCharacteristicsWU(PlayerAvatarData data)
    {
        string sex = ((data.gender == Gender.Female) ? "Female" : "Male");
        int headType = data.headType;
        int bodyType = data.bodyType;
        int glassesType = data.glassesType;
        SetCharacterCharacteristicsWU(sex, headType, bodyType, glassesType);
    }

    public static void SetCharacterCharacteristicsWU(string sexType, int head, int body, int glasses)
    {
        CharacterInfo.sex = sexType;
        CharacterInfo.headType = head;
        CharacterInfo.bodyType = body;
        CharacterInfo.glassesType = glasses;

        string[][] data = new string[][]
        {
            new string[] { "CharacterCreated", "true" },
            new string[] { "CharacterSex", CharacterInfo.sex },
            new string[] { "CharacterHeadType", CharacterInfo.headType.ToString() },
            new string[] { "CharacterBodyType", CharacterInfo.bodyType.ToString() },
            new string[] { "CharacterGlassesType", CharacterInfo.glassesType.ToString() },
        };
        DatabaseManager.UpdateCategory("AccountStats", data);
    }
    
}