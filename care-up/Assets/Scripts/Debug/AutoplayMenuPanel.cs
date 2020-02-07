using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoplayMenuPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefsManager.simulatePlayerActions)
        {
            gameObject.SetActive(false);
        }
    }
}
