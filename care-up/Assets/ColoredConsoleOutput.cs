using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredConsoleOutput : MonoBehaviour
{
    // Start is called before the first frame update
   
    public static void Print(Color debugColor, string messageText)
    {
        Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(debugColor.r), (byte)(debugColor.g), (byte)(debugColor.b), messageText)) ;
    }
    public static void Print(string debugColorHex, string messageText)
    {
        Color hexColorConverted;

        if (ColorUtility.TryParseHtmlString(debugColorHex, out hexColorConverted))
        {
            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(hexColorConverted.r*255f), (byte)(hexColorConverted.g * 255f), (byte)(hexColorConverted.b * 255f), messageText));
        }
        else
        {
            Debug.Log(messageText);
        }
    }

}
public class RGBColor
{
    public static Color Red = new Color(255, 0, 0);
    public static Color White = new Color(255, 255, 255);
    public static Color Black = new Color(0, 0, 0);
    public static Color Green = new Color(0, 255, 0);
    public static Color Blue = new Color(0, 0, 255);
    public static Color Cyan = new Color(0, 255, 255);
    public static Color Magenta = new Color(255, 0, 255);
    public static Color Yellow = new Color(255, 255, 0);
    public static Color Gray = new Color(128, 128, 128);
    public static Color Silver = new Color(192, 192, 192);
    public static Color Maroon = new Color(128, 0, 0);
    public static Color Olive = new Color(128, 128, 0);
    public static Color Purple = new Color(128, 0, 128);
    public static Color Teal = new Color(0, 128, 128);
    public static Color Navy = new Color(0, 0, 128);

}
