using MBS;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public string sex;
    public int headType;
    public int bodyType;
    public int glassesType;

    //indicates that the current character state has been checked
    static bool currentStateChecked = false;

    //is used for character characteristics comparison between data received form database and newly entered
    public static CharacterInfo currentCharacter = new CharacterInfo();

    public void SetCharacterCharacteristicsWU(string sexType, int head, int body, int glasses)
    {
        sex = sexType;
        headType = head;
        bodyType = body;
        glassesType = glasses;

        CMLData data = new CMLData();

        if (currentStateChecked)
        {
            if (sex != currentCharacter.sex)
                if (!string.IsNullOrEmpty(sex))
                    data.Set("CharacterSex", sex);

            if (headType != currentCharacter.headType)
                data.Set("CharacterHeadType", headType.ToString());

            if (bodyType != currentCharacter.bodyType)
                data.Set("CharacterBodyType", bodyType.ToString());

            if (glassesType != currentCharacter.glassesType)
                data.Set("CharacterGlassesType", glassesType.ToString());
        }
        else
        {
            if (!string.IsNullOrEmpty(sex))
                data.Set("CharacterSex", sex);
            data.Set("CharacterHeadType", headType.ToString());
            data.Set("CharacterBodyType", bodyType.ToString());
            data.Set("CharacterGlassesType", glassesType.ToString());
        }

        WUData.UpdateCategory("AccountStats", data);
        currentStateChecked = false;
    }

    public void GetCharacterCharacteristicsWU()
    {
        WUData.FetchCategory("AccountStats", GetCharacterCharacteristics);
    }

    void GetCharacterCharacteristics(CML response)
    {
        for (int i = 0; i < response.Elements[1].Keys.Length; ++i)
        {
            switch (response.Elements[1].Keys[i])
            {
                case "CharacterSex":
                    currentCharacter.sex = response.Elements[1].Values[i];
                    continue;

                case "CharacterHeadType":
                    int.TryParse(response.Elements[1].Values[i], out currentCharacter.headType);
                    continue;

                case "CharacterBodyType":
                    int.TryParse(response.Elements[1].Values[i], out currentCharacter.bodyType);
                    continue;

                case "CharacterGlassesType":
                    int.TryParse(response.Elements[1].Values[i], out currentCharacter.glassesType);
                    continue;

                default:
                    break;
            }
        }
        currentStateChecked = true;
    }


    public void GetCharacterSexWU()
    {
        WUData.FetchField("CharacterSex", "AccountStats", GetCharacterSex, -1);
    }

    static void GetCharacterSex(CML response)
    {
        currentCharacter.sex = response[1].String("CharacterSex");
        print(currentCharacter.sex);
    }


    public void GetCharacterHeadTypeWU()
    {
        WUData.FetchField("CharacterHeadType", "AccountStats", GetCharacterHeadType, -1);
    }

    static void GetCharacterHeadType(CML response)
    {
        currentCharacter.headType = response[1].Int("CharacterHeadType");
    }


    public void GetCharacterBodyTypeWU()
    {
        WUData.FetchField("CharacterBodyType", "AccountStats", GetCharacterBodyType, -1);
    }

    static void GetCharacterBodyType(CML response)
    {
        currentCharacter.bodyType = response[1].Int("CharacterBodyType");
    }


    public void GetCharacterGlassesTypeWU()
    {
        WUData.FetchField("CharacterGlassesType", "AccountStats", GetCharacterGlassesType, -1);
    }

    static void GetCharacterGlassesType(CML response)
    {
        currentCharacter.glassesType = response[1].Int("CharacterGlassesType");
    }


    public void SetCharacterCreationCompleted()
    {
        CMLData data = new CMLData();
        data.Set("CharacterCreated", "true");
        WUData.UpdateCategory("AccountStats", data);
    }

    public void GetCharacterCreationCompletionWU()
    {
        WUData.FetchField("CharacterCreated", "AccountStats", GetCharacterCreationCompletion,
            -1, GetCharacterCreationCompletion_Error);
    }

    static void GetCharacterCreationCompletion(CML response)
    {
        WULogin.characterCreated = response[1].Bool("CharacterCreated");
        Debug.Log("Character created = " + WULogin.characterCreated);
    }

    static void GetCharacterCreationCompletion_Error(CMLData response)
    {
        if ((response["message"] == "WPServer error: Empty response. No data found"))
        {
            CMLData data = new CMLData();
            data.Set("CharacterCreated", "false");
            WUData.UpdateCategory("AccountStats", data);
        }
    }
}