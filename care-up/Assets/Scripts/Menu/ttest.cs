using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ttest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        var rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;
        Debug.Log(width.ToString() + "  " + height.ToString());
    }
}
