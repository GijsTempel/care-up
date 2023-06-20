using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CABackground : MonoBehaviour, IPointerClickHandler
{
    public CAScreenKeyboard mainObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        // when we detect click, act as if we're closing CAScreenKeyboard 
        mainObject.EnterValue();
    }
}
