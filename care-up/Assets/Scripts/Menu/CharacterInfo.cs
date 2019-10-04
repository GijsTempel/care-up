using UnityEngine;
using CareUpAvatar;

public class CharacterInfo : MonoBehaviour
{
    public static int index;
    public static bool purchased;
    public static int price;
    public static string sex;
    public static int headType;
    public static int bodyType;
    public static int glassesType;
    public static string heat;

    private static string[][] data;

    public static void SetCharacterCharacteristicsWU(CharacterItem data)
    {
        int index = data.index;
        int price = data.price;
        bool purchased = data.purchased;
        string sex = ((data.playerAvatar.gender == Gender.Female) ? "Female" : "Male");
        int headType = data.playerAvatar.headType;
        int bodyType = data.playerAvatar.bodyType;
        int glassesType = data.playerAvatar.glassesType;
        string heat = data.playerAvatar.heat;

        SetCharacterCharacteristicsWU(sex, headType, bodyType, glassesType, heat, index, price, purchased);
    }

    public static void SetCharacterCharacteristicsWU(string sexType, int head, int body, int glasses, string heat = "", int index = 0, int price = 0, bool purchased = false)
    {
        CharacterInfo.index = index;
        CharacterInfo.price = price;
        CharacterInfo.purchased = purchased;
        CharacterInfo.sex = sexType;
        CharacterInfo.headType = head;
        CharacterInfo.bodyType = body;
        CharacterInfo.glassesType = glasses;
        CharacterInfo.heat = heat;

        data = new string[][]
        {
            new string[] { "Index", CharacterInfo.index.ToString() },
            new string[] { "Price", CharacterInfo.price.ToString() },
            new string[] { "Purchased", CharacterInfo.purchased.ToString() },
            new string[] { "Created", "true" },
            new string[] { "Sex", CharacterInfo.sex },
            new string[] { "Head", CharacterInfo.headType.ToString() },
            new string[] { "Body", CharacterInfo.bodyType.ToString() },
            new string[] { "Glasses", CharacterInfo.glassesType.ToString() },
            new string[] { "Heat", CharacterInfo.heat },
        };
        DatabaseManager.UpdateCategory("CharacterItem_" + index.ToString(), data);
    }

    public void UpdateCharacter(StoreItem item)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i][0] == item.category)
            {
                DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), data[i][0], item.index.ToString());
            }
        }
    }
}