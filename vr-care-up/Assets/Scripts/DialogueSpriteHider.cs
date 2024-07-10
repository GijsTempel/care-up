using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSpriteHider : MonoBehaviour
{
    public Text text;
    public Image image;

    void Update()
    {
        image.enabled = text.text != "";
    }
}
