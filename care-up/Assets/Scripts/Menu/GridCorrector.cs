using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GridCorrector : MonoBehaviour {
    Vector2 dCellSize = new Vector2(400, 376);
    Vector2 dScreenSize = new Vector2(1920, 1080);
    //Vector2 ScreenSize;
    float wRatio = 4.8f;

    // Use this for initialization
    void Start () {
        //ScreenSize = new Vector2(Screen.width, Screen.height);
     }

    private void Update()
    {
        float xx = ((dScreenSize.y / Screen.height) * Screen.width) / wRatio;
        float yy = xx / (dCellSize.x / dCellSize.y);
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(xx, yy);
    }
}
