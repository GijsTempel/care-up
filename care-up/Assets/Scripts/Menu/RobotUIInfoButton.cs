using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUIInfoButton : MonoBehaviour {
    
    private GameObject ui;

    public void Set(string name)
    {
        ui = Instantiate(Resources.Load<GameObject>("Prefabs\\UI\\" + name),
                            Vector3.zero, Quaternion.identity, transform.parent.parent.parent.parent.parent) as GameObject;
        ui.SetActive(false);
    }

    public void Toggle(bool value)
    {
        if (ui != null)
            ui.SetActive(value);
    }
}
