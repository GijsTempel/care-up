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
    public static string hat;

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
        string hat;
        if (data.playerAvatar.hat != null)
            hat = data.playerAvatar.hat;
        else
            hat = "";

        SetCharacterCharacteristicsWU(sex, headType, bodyType, glassesType, hat, index, price, purchased);
    }

    public static void SetCharacterCharacteristicsWU(string sexType, int head, int body, int glasses, string hat = "", int index = 0, int price = 0, bool purchased = false)
    {
        CharacterInfo.index = index;
        CharacterInfo.price = price;
        CharacterInfo.purchased = purchased;
        CharacterInfo.sex = sexType;
        CharacterInfo.headType = head;
        CharacterInfo.bodyType = body;
        CharacterInfo.glassesType = glasses;
        CharacterInfo.hat = hat;

        data = new string[][]
        {
                new string[] { "Index", CharacterInfo.index.ToString() },
                new string[] { "Price", CharacterInfo.price.ToString() },
                new string[] { "Purchased", CharacterInfo.purchased.ToString() },
                new string[] { "CharacterCreated", "true" },
                new string[] { "Sex", CharacterInfo.sex },
                new string[] { "Head", CharacterInfo.headType.ToString() },
                new string[] { "Body", CharacterInfo.bodyType.ToString() },
                new string[] { "Glasses", CharacterInfo.glassesType.ToString() },
                new string[] { "Hat", CharacterInfo.hat },
        };
        DatabaseManager.UpdateCategory("CharacterItem_" + index.ToString(), data);
        DatabaseManager.UpdateCategory("AccountStats", data);
    }

    public static void UpdateCharacter(StoreItem item)
    {
        switch (item.category)
        {
            case "Hat":

                CharacterInfo.hat = item.name;
                DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Hat", CharacterInfo.hat);
                DatabaseManager.UpdateField("AccountStats", "Hat", CharacterInfo.hat);

                foreach (CharacterItem characterItem in PlayerPrefsManager.storeManager.CharacterItems)
                {
                    if (characterItem.index == index)
                    {
                        characterItem.playerAvatar.hat = CharacterInfo.hat;
                        break;
                    }
                }

                break;

            case "Glasses":
                CharacterInfo.glassesType = item.index;
                DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Glasses", CharacterInfo.glassesType.ToString());
                DatabaseManager.UpdateField("AccountStats", "Glasses", CharacterInfo.glassesType.ToString());
                foreach (CharacterItem characterItem in PlayerPrefsManager.storeManager.CharacterItems)
                {
                    if (characterItem.index == index)
                    {
                        characterItem.playerAvatar.glassesType = CharacterInfo.glassesType;
                        break;
                    }
                }
                break;

            case "Body":
                CharacterInfo.bodyType = item.index;
                DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Body", CharacterInfo.bodyType.ToString());
                DatabaseManager.UpdateField("AccountStats", "Body", CharacterInfo.bodyType.ToString());

                foreach (CharacterItem characterItem in PlayerPrefsManager.storeManager.CharacterItems)
                {
                    if (characterItem.index == index)
                    {
                        characterItem.playerAvatar.bodyType = CharacterInfo.bodyType;
                        break;
                    }
                }
                break;
        }
    }
}