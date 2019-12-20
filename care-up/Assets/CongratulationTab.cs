using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationTab : MonoBehaviour
{
    public GameObject diamantEffect;

    public void ShowDiamantEffect(bool value)
    {
        diamantEffect.SetActive(value);
    }
}
