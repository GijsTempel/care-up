using UnityEngine;
using CareUpAvatar;

public class CharacterInfo : MonoBehaviour
{
    public static bool isMain;
    public static int index;
    public static bool purchased;
    public static int price;
    public static string sex;
    public static int headType;
    public static int bodyType;
    public static int glassesType;
    public static string heat;

    public static void SetCharacterCharacteristicsWU(CharacterItem data)
    {
        bool isMain = data.isMain;
        int index = data.index;
        int price = data.price;
        bool purchased = data.purchased;
        string sex = ((data.playerAvatar.gender == Gender.Female) ? "Female" : "Male");
        int headType = data.playerAvatar.headType;
        int bodyType = data.playerAvatar.bodyType;
        int glassesType = data.playerAvatar.glassesType;

        SetCharacterCharacteristicsWU(sex, headType, bodyType, glassesType, index, price, purchased, isMain);
    }

    public static void SetCharacterCharacteristicsWU(string sexType, int head, int body, int glasses, int index = 0, int price = 0, bool purchased = false, bool isMain = false)
    {
        CharacterInfo.isMain = isMain;
        CharacterInfo.index = index;
        CharacterInfo.price = price;
        CharacterInfo.purchased = purchased;
        CharacterInfo.sex = sexType;
        CharacterInfo.headType = head;
        CharacterInfo.bodyType = body;
        CharacterInfo.glassesType = glasses;

        string[][] data = new string[][]
        {
            new string[] { "CharacterMain", CharacterInfo.isMain.ToString() },
            new string[] { "CharacterIndex", CharacterInfo.index.ToString() },
            new string[] { "CharacterPrice", CharacterInfo.price.ToString() },
            new string[] { "CharacterPurchased", CharacterInfo.purchased.ToString() },
            new string[] { "CharacterCreated", "true" },
            new string[] { "CharacterSex", CharacterInfo.sex },
            new string[] { "CharacterHeadType", CharacterInfo.headType.ToString() },
            new string[] { "CharacterBodyType", CharacterInfo.bodyType.ToString() },
            new string[] { "CharacterBodyType", CharacterInfo.bodyType.ToString() },
            new string[] { "CharacterGlassesType", CharacterInfo.glassesType.ToString() },
            new string[] { "CharacterHeat", CharacterInfo.heat },
        };
        DatabaseManager.UpdateCategory("CharacterItem_" + index.ToString(), data);
    }
}